using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicStore.Helper;

namespace MusicStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SystemController : Controller
    {
        private readonly CookieLogic _cookieLogic;

        public SystemController(CookieLogic cookieLogic)
        {
            _cookieLogic = cookieLogic;
        }



        public ActionResult Index()
        {
            return View();
        }

        public IActionResult RemoveBasketCookie()
        {
            _cookieLogic.RemoveBasketId();
            return RedirectToAction("Home/Index");
        }
    }
}
