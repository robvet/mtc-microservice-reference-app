using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MusicStore.Helper;
using MusicStore.Models;

namespace MusicStore.Controllers
{
    public class HomeController : Controller
    {
        private const string baseUrl = "catalog/api/catalog";
        private readonly IRestClient _IRestClient;
        private readonly int count = 6;
    
        public HomeController(IRestClient iuiRestClient)
        {
            _IRestClient = iuiRestClient;
        }

        //
        // GET: /Home/
        public async Task<IActionResult> Index()
        {
            var result = await _IRestClient.GetAsync<List<AlbumDTO>>($"{baseUrl}/TopSellingMusic/{count}");

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