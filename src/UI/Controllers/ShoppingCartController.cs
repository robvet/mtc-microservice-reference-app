using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MusicStore.Helper;
using MusicStore.Models;
using MusicStore.ViewModels;

namespace MusicStore.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly CookieLogic _cookieLogic;
        private readonly ILogger<ShoppingCartController> _logger;
        private readonly IRestClient _IRestClient;
        private readonly string baseUrl = "basket/api/basket";

        public ShoppingCartController(ILogger<ShoppingCartController> logger,
                                      CookieLogic cookieLogic,
                                      IRestClient iuiRestClient)
        {
            _cookieLogic = cookieLogic;
            _logger = logger;
            _IRestClient = iuiRestClient;
        }

        public async Task<IActionResult> Index()
        {
            BasketDto basket;
            var viewModel = new ShoppingCartViewModel();

            //var cartExists = await _IRestClient.GetAsync<CartExistModel>($"{baseUrl}/BasketExists/{_cookieLogic.GetBasketId()}");

            //var data = cartExists.Data;

            //if (!data.CartExists)
            //{
            //    return RedirectToAction("Index", "Home");
            //}

            var response =
                    await _IRestClient.GetAsync<BasketDto>($"{baseUrl}/Basket/{_cookieLogic.GetBasketId()}");
                    //await _IRestClient.GetAsync<BasketDto>($"{baseUrl}/{_cookieLogic.GetBasketId()}");


            basket = response.Data;

            if (basket.CartItems.Count == 0)
            {
                // Cart is empty, remove cart, and navigate to home
                _cookieLogic.RemoveBasketId();
                return RedirectToAction("Index", "Home");
            }
            
            ViewBag.CartCount = basket.ItemCount;
            ViewBag.CartSummary = string.Join("\n", basket.CartItems.Select(c => c.Name).Distinct());

            viewModel.CartItems = basket.CartItems;
            viewModel.CartTotal = basket.CartTotal;

            // Return the view
            return View(viewModel);
        }

        //
        // GET: /ShoppingCart/AddToCart/5

        public async Task<IActionResult> AddToCart(int id, CancellationToken requestAborted)
        {
            // determine if shopping cart Id exists
            var shoppingCartId = _cookieLogic.GetBasketId();

            if (string.IsNullOrEmpty(shoppingCartId))
            {
                //shoppingCartId = _cookieLogic.SetBasketId();
                shoppingCartId = "-1";
            }

        // Add item to the shopping cart5
        //var basket = await _IRestClient.PostAsync<BasketDto>($"{baseUrl}/Basket/{shoppingCartId}/item/{id}");

        
                        // Here's what needs to be sent
            //http://localhost:8083/api/Basket/?productId=258&basketId=Basket-3.29.2023-9:42PM-0277211917

            var basket = await _IRestClient.PostAsync<BasketSummaryDto>($"{baseUrl}/?productId={id}&basketId={shoppingCartId}");
            //var basket = await _IRestClient.PostAsync<BasketSummaryDto>($"{baseUrl}/{shoppingCartId}/item/{id}");

            _logger.LogInformation($"Song {id} was added to the cart.");

            if (shoppingCartId == "-1")
            {
                _cookieLogic.SetBasketId(basket.Data.BasketId);
            }
            
            // Go back to the main store page for more shopping
            return RedirectToAction("Index");
        }
        
        // AJAX: /ShoppingCart/RemoveFromCart/5
        public async Task<IActionResult> RemoveFromCart(int id, CancellationToken requestAborted)
        {
            // Remove from cart
            await _IRestClient.DeleteAsync($"{baseUrl}/{_cookieLogic.GetBasketId()}/lineitem/{id}");

            TempData[ToastrMessage.Success] = "Successfully Removed Song";

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> RemoveCart(CancellationToken requestAborted)
        {
            await _IRestClient.DeleteAsync($"{baseUrl}?basketId={_cookieLogic.GetBasketId()}");

            // Remove cookie
            _cookieLogic.RemoveBasketId();

            TempData[ToastrMessage.Success] = "Successfully Removed Shopping Cart";

            return RedirectToAction("Index", "Home");
        }
    }
}