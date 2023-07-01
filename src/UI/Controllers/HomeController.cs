using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MusicStore.Plumbing;
using MusicStore.Models;
using Microsoft.Extensions.Logging;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;

namespace MusicStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly string _baseUrl;
        private readonly IRestClient _IRestClient;
        private readonly int count = 6;
        private ILogger<HomeController> _logger;

        public HomeController(IRestClient uiRestClient,
                              IConfiguration configuration,
                              ILogger<HomeController> logger) 
        {
            _IRestClient = uiRestClient;
            _baseUrl = configuration["catalogBaseUri"] ??
                       throw new ArgumentNullException("catalogBaseUri", "Missing value");
            _logger = logger;
        }

        // GET: /Home/
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Calling TopSellingMusic from HomeController");

            var result = await _IRestClient.GetAsync<List<ProductDto>>($"{_baseUrl}/TopSellingMusic/{count}");

            _logger.LogInformation($"Returned {result.Data.Count} records in TopSellingMusic from HomeController");

            return View(result.Data);
        }

        public IActionResult StatusCodePage()
        {
            ViewData["statusCode"] = TempData["statusCode"];
            return View("~/Views/Shared/StatusCodePage.cshtml");
        }

        public IActionResult AccessDenied()
        {
            return View("~/Views/Shared/AccessDenied.cshtml");
        }

        public IActionResult RemoveBasketCookie([FromServices] CookieLogic cookieLogic)
        {
            cookieLogic.RemoveBasketId();
            return RedirectToAction("Index");
        }
    }
}