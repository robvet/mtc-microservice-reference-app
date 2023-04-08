using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicStore.Helper;

namespace MusicStore.Areas.Admin.Controllers
{
    public class SystemController : Controller
    {
        // GET: SystemController
        public ActionResult Index()
        {
            return View();
        }

        public IActionResult RemoveBasketCookie([FromServices] CookieLogic cookieLogic)
        {
            cookieLogic.RemoveBasketId();
            return RedirectToAction("Home/Index");
        }
    }
}
