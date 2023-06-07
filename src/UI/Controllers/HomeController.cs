using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MusicStore.Helper;
using MusicStore.Models;

namespace MusicStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly string _baseUrl;
        private readonly IRestClient _IRestClient;
        private readonly int count = 6;

        public HomeController(IRestClient uiRestClient, IConfiguration configuration)
        {
            _IRestClient = uiRestClient;
            _baseUrl = configuration["catalogBaseUri"] ??
                       throw new ArgumentNullException("catalogBaseUri", "Missing value");
        }

        // GET: /Home/
        public async Task<IActionResult> Index()
        {
            var result = await _IRestClient.GetAsync<List<ProductDto>>($"{_baseUrl}/TopSellingMusic/{count}");
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