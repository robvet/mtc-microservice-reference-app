using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MusicStore.Helper;
using MusicStore.Models;

namespace MusicStore.Components
{
    [ViewComponent(Name = "CartSummary")]
    public class CartSummaryComponent : ViewComponent
    {
        private const string baseUrl = "basket/api/Basket";
        private readonly CookieLogic _cookieLogic;
        private readonly IRestClient _IRestClient;

        public CartSummaryComponent(IRestClient iuiRestClient, CookieLogic cookieLogic)
        {
            _IRestClient = iuiRestClient;
            _cookieLogic = cookieLogic;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            BasketDto basket;

            var shoppingCartId = _cookieLogic.GetBasketId();

            if (!string.IsNullOrEmpty(shoppingCartId))
            {
                var response = await _IRestClient.GetAsync<BasketSummaryDto>($"{baseUrl}/BasketSummary/{_cookieLogic.GetBasketId()}");

                if (response.Data != null)
                {
                    //var response =
                    //    await _IRestClient.GetAsync<BasketDto>($"{_baseUrl}/Basket/{_cookieLogic.GetBasketId()}");

                    ViewBag.CartCount = response.Data.ItemCount;
                    ViewBag.CartSummary = response.Data.ProductNames;
                }
                else
                {
                    // If shopping basket ID from cookie has no shopping cart, remove the basket ID
                    _cookieLogic.RemoveBasketId();
                }
            }

            //basket = await _cartService.GetCart() ?? new BasketDto();

            return View();
        }

    }
}