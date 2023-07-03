﻿using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MusicStore.Models;
using System.Collections.Generic;
using SharedUtilities.Utilties;
using Microsoft.Build.Framework;
using Microsoft.Extensions.Logging;

namespace MusicStore.Controllers
{
    public class ErrorController : Controller
    {
        //https://www.red-gate.com/simple-talk/dotnet/net-development/asp-net-core-3-0-exception-handling/
        //https://chrissainty.com/global-error-handling-aspnet-core-mvc/

        private ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;   
        }


        public IActionResult Error()
        {
            _logger.LogError($"Error in UI ErrorController");
            
            var error = HttpContext.Features.Get<IExceptionHandlerFeature>();

            _logger.LogError($"Specifc Error in UI ErrorController: {error}");

            //Parse out error message to separate correlationId from error message
            var split = error.Error.Message.Split("with correlationId:");

            // System characters to remove from error message
            var charsToRemove = new List<char> { '\\', '"', '\n', '\r', '\u006A', '\x006A', (char)106, '\t', '\a', '\r', '\"', '\'', (char)92, (char)34, (char)10, (char)13, (char)106, (char)9, (char)39 };

            // parse out extraneous characters
            var parsedValue = split[0].RemoveCharacters(charsToRemove);

            return View(new ErrorViewModel
            {
                ErrorMessage = parsedValue,
                CorrelationId = split.Length > 1 ? split[1] : string.Empty
            });
        }
    }
}