using System;
using Microsoft.AspNetCore.Http;
using MusicStore.Models;
using Utilities;
using Snowflake;

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
        public string SetBasketId(string basketId = null)
        {
            //A GUID to hold the basketId. 
            if (string.IsNullOrWhiteSpace(basketId))
            {
                // Use SnowflakeIdGenerator to generate BasketId
                basketId = SnowflakeIdGenerator.GenerateId(SnowflakeEnum.Basket);
            }

            var options = new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(7),
                HttpOnly = true
            };

            _httpContextAccessor.HttpContext.Response.Cookies.Append(CookieName, basketId, options);
            return basketId;
        }

        /// <summary>
        /// Removes cookie value for current shopping basket id
        /// </summary>
        public void RemoveBasketId()
        {
            var cartId = _httpContextAccessor.HttpContext.Request.Cookies[CookieName];
            _httpContextAccessor.HttpContext.Response.Cookies.Delete(CookieName);
        }
    }
}