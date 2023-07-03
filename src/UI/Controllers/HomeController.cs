using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MusicStore.Plumbing;
using MusicStore.Models;
using Microsoft.Extensions.Logging;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace MusicStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly string _baseUrl;
        private readonly IRestClient _IRestClient;
        private readonly int count = 6;
        private ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public HomeController(IRestClient uiRestClient,
                              IConfiguration configuration,
                              ILogger<HomeController> logger,
                              IWebHostEnvironment hostingEnvironment) 
        {
            _IRestClient = uiRestClient;
            _baseUrl = configuration["catalogBaseUri"] ??
                       throw new ArgumentNullException("catalogBaseUri", "Missing value");
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: /Home/
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Calling TopSellingMusic from HomeController");

            var result = await _IRestClient.GetAsync<List<ProductDto>>($"{_baseUrl}/TopSellingMusic/{count}");

            return View(result.Data);
        }

        public async Task<IActionResult> MarkdownPage()
        {
            //string markdownFilePath = @"~/wwwroot/markdown/file.md"; // @"C:\path\to\markdown\file.md";
            string filePath = string.Empty;

            var wwwrootPath = _hostingEnvironment.WebRootPath;
            filePath = Path.Combine(wwwrootPath, "Markdown", "azure-caching.md");


            string markdownContent = await System.IO.File.ReadAllTextAsync(filePath);


            return View(markdownContent);



            //return View(new MarkdownModel { Content = markdownContent });
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

    public class MarkdownModel
    {
        public string Content { get; set; }
    }
}