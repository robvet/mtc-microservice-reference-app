using Microsoft.AspNetCore.Http;
using MusicStore.Models;
using System;
using SharedUtilities.TokenGenerator;

namespace MusicStore.Helper
{
    public class CookieLogic
    {
        private const string CookieName = ".musicstore";
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CookieLogic(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        ///     Get or Create Cart ID using a persistent cookie
        /// </summary>
        /// <returns></returns>
        public string GetBasketId()
        {
            var cartId = _httpContextAccessor.HttpContext.Request.Cookies[CookieName];
            //var cartId = _httpContextAccessor.HttpContext.Request.Cookies[CookieName] ?? SetBasketId();

            return cartId;
        }

        /// <summary>
        ///     Sets the cookie value for the basketId or creates a new ID if null
        /// </summary>
        /// <param name="basketId">
        ///     The <see cref="BasketDto.BasketId">Basket ID</see>.
        ///     If null create a new ID
        /// </param>
        /// <returns>the new The <see cref="BasketDto.BasketId">Basket ID</see></returns>
        public Guid SetBasketId(Guid basketId) 
        {
            var options = new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(7),
                HttpOnly = true
            };

            _httpContextAccessor.HttpContext.Response.Cookies.Append(CookieName, basketId.ToString(), options);
            return basketId;
        }

        /// <summary>
        /// Removes cookie value for current shopping basket id
        /// </summary>
        public void RemoveBasketId()
        {
            // Only way to remove a cookie is to set it's expiration in the past
            var cookieVaule = _httpContextAccessor.HttpContext.Request.Cookies[CookieName];

            var options = new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(-1),
                HttpOnly = true
            };

            _httpContextAccessor.HttpContext.Response.Cookies.Append(CookieName, cookieVaule, options);

            //var cartId = _httpContextAccessor.HttpContext.Request.Cookies[CookieName];
            //_httpContextAccessor.HttpContext.Response.Cookies.Delete(CookieName);
        }
    }
}