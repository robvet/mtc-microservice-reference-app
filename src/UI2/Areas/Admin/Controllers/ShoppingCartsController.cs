using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MusicStore.Controllers;
using MusicStore.Helper;
using MusicStore.Models;
using MusicStore.ViewModels;

namespace MusicStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ShoppingCartsController : Controller
    {
        private readonly IRestClient _IRestClient;
        private readonly string baseUrl = "api/BasketGateway";
        private readonly ILogger<ShoppingCartController> _logger;

        public ShoppingCartsController(ILogger<ShoppingCartController> logger,
            CookieLogic cookieLogic,
            IRestClient iuiRestClient)
        {
            _logger = logger;
            _IRestClient = iuiRestClient;
        }

        // GET: ShoppingCartsController
        public async Task<ActionResult> Index()
        {
            List<BasketDto> basket;
            //var viewModel = new ShoppingCartViewModel();
           
            var response =
                await _IRestClient.GetAsync<List<BasketDto>>($"{baseUrl}/Baskets");

            basket = response.Data;

            //// map to viewmodel
            //foreach (var item in response.Data)
            //{
            //    var model = new BasketDto
            //    {
            //        BasketId = item.BasketId,
            //        ItemCount = item.ItemCount,
            //        CartTotal = item.CartTotal,
            //    };
            //}

            return View(response.Data);
        }

        public async Task<IActionResult> RemoveCart()
        {
            var basketId = Request.Query["id"];
            
            await _IRestClient.DeleteAsync($"{baseUrl}/Basket/{basketId}");

            TempData[ToastrMessage.Success] = "Successfully Removed Shopping Cart";

            return RedirectToAction("Index");
        }
    }
}
