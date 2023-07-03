using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MusicStore.Plumbing;
using MusicStore.Models;
using MusicStore.ViewModels;

namespace MusicStore.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly CookieLogic _cookieLogic;
        private readonly ILogger<ShoppingCartController> _logger;
        private readonly IRestClient _IRestClient;
        private readonly string _baseUrl;

        public ShoppingCartController(ILogger<ShoppingCartController> logger,
                                      CookieLogic cookieLogic,
                                      IRestClient iuiRestClient,
                                      IConfiguration configuration)
        {
            _cookieLogic = cookieLogic;
            _logger = logger;
            _IRestClient = iuiRestClient;
            _baseUrl = configuration["basketBaseUri"] ??
                       throw new ArgumentNullException("basketBaseUri", "Missing value");
        }

        public async Task<IActionResult> Index()
        {
            BasketDto basket;
            var viewModel = new ShoppingCartViewModel();

            //var cartExists = await _IRestClient.GetAsync<CartExistModel>($"{_baseUrl}/BasketExists/{_cookieLogic.GetBasketId()}");

            //var data = cartExists.Data;

            //if (!data.CartExists)
            //{
            //    return RedirectToAction("Index", "Home");
            //}

            var basketId = _cookieLogic.GetBasketId();

            if (basketId == null)
            {
                throw new Exception($"Cookie missing in Index Action of ShoppingCartController");
            }

            var response =
                    await _IRestClient.GetAsync<BasketDto>($"{_baseUrl}/Basket/{basketId}");
                    //await _IRestClient.GetAsync<BasketDto>($"{_baseUrl}/{_cookieLogic.GetBasketId()}");


            basket = response.Data;

            if (basket.CartItems.Count == 0)
            {
                // Cart is empty, remove cart, and navigate to home
                _cookieLogic.RemoveBasketId();
                return RedirectToAction("Index", "Home");
            }
            
            ViewBag.CartCount = basket.ItemCount;
            ViewBag.CartSummary = string.Join("\n", basket.CartItems.Select(c => c.Title).Distinct());

            viewModel.CartItems = basket.CartItems;
            viewModel.CartTotal = basket.CartTotal;
            viewModel.ItemCount = basket.ItemCount;

            // Return the view
            return View(viewModel);
        }

        //
        // GET: /ShoppingCart/AddToCart/5

        public async Task<IActionResult> AddToCart(Guid id, CancellationToken requestAborted)
        {
            // determine if shopping cart Id exists
            var shoppingCartId = _cookieLogic.GetBasketId();
            var cartId = Guid.Empty;

            if (Guid.TryParse(shoppingCartId, out Guid cartGuid))
            {
                cartId = cartGuid;
            }

             // Add item to the shopping cart5
             //var basket = await _IRestClient.PostAsync<BasketDto>($"{_baseUrl}/Basket/{shoppingCartId}/item/{id}");
        
            // Here's what needs to be sent
            //http://localhost:8083/api/Basket/?productId=258&basketId=Basket-3.29.2023-9:42PM-0277211917

            var basket = await _IRestClient.PostAsync<BasketSummaryDto>($"{_baseUrl}/?productId={id}&basketId={cartId}");
            //var basket = await _IRestClient.PostAsync<BasketSummaryDto>($"{_baseUrl}/{shoppingCartId}/item/{id}");

            _logger.LogInformation($"Song {id} was added to the cart.");

            if (cartId == Guid.Empty) 
            {
                cartId = basket.Data.BasketId;
                _cookieLogic.SetBasketId(basket.Data.BasketId);
            }

            // Go back to the main store page for more shopping
            return RedirectToAction("Index");
        }
        
        // AJAX: /ShoppingCart/RemoveFromCart/5
        public async Task<IActionResult> RemoveFromCart(Guid id, CancellationToken requestAborted)
        {
            // Remove from cart
            await _IRestClient.DeleteAsync($"{_baseUrl}/{_cookieLogic.GetBasketId()}/lineitem/{id}");

            TempData[ToastrMessage.Success] = "Successfully Removed Song";

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> RemoveCart(CancellationToken requestAborted)
        {
            await _IRestClient.DeleteAsync($"{_baseUrl}?basketId={_cookieLogic.GetBasketId()}");

            // Remove cookie
            _cookieLogic.RemoveBasketId();

            TempData[ToastrMessage.Success] = "Successfully Removed Shopping Cart";

            return RedirectToAction("Index", "Home");
        }
    }
}