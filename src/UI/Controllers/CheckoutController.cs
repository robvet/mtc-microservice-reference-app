using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MusicStore.Helper;
using MusicStore.Models;
using RandomNameGeneratorLibrary;

namespace MusicStore.Controllers
{
    public class CheckoutController : Controller
    {
        private const string PromoCode = "FREE";
        private readonly CookieLogic _cookieLogic;
        private readonly IRestClient _IRestClient;
        private readonly ILogger<CheckoutController> _logger;
        private readonly string baseUrl = "basket/api/Basket";

        public CheckoutController(ILogger<CheckoutController> logger,
            CookieLogic cookieLogic,
            IRestClient iuiRestClient)
        {
            _cookieLogic = cookieLogic;
            _logger = logger;
            _IRestClient = iuiRestClient;
        }

        //
        // GET: /Checkout/
        public IActionResult AddressAndPayment()
        {
            var nameGenerator = new PersonNameGenerator();
            var placeNameGenerator = new PlaceNameGenerator();
            var rndNum = new Random();

            var firstName = nameGenerator.GenerateRandomMaleFirstName();
            var lastName = nameGenerator.GenerateRandomLastName();

            var orderCreateDto = new CheckoutDto
            {
                FirstName = firstName,
                LastName = lastName,
                Address = rndNum.Next(1000, 9999) + " " + placeNameGenerator.GenerateRandomPlaceName(),
                PostalCode = rndNum.Next(10000, 99999).ToString(),
                City = placeNameGenerator.GenerateRandomPlaceName(),
                State = "OH",
                Phone = "(" + rndNum.Next(100, 999) + ")" + rndNum.Next(100, 999) + "-" + rndNum.Next(1000, 9999),
                Email = firstName + "@" + "hotmail.com",
                CreditCardNumber = "4111 1111 1111 1111",
                CardholderName =  $"{firstName} {lastName}",
                SecurityCode = "123",
                ExpirationDate = DateTime.Today.AddYears(1),
            };

            return View(orderCreateDto);
        }

        //
        // POST: /Checkout/AddressAndPayment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddressAndPayment(
            [FromForm] CheckoutDto checkoutDto,
            CancellationToken requestAborted)
        {
            if (!ModelState.IsValid) return View(checkoutDto);

            var formCollection = await HttpContext.Request.ReadFormAsync();

            try
            {
                //if (string.Equals(formCollection["PromoCode"].FirstOrDefault(), PromoCode, StringComparison.OrdinalIgnoreCase) == false) return View(checkoutDto);

                // Abbreviate user first and last name to create the username
                checkoutDto.Username = $"{checkoutDto.FirstName.Substring(0, 2)}-{checkoutDto.LastName.Substring(0, 3)}";
                checkoutDto.BasketId = _cookieLogic.GetBasketId();

                var response = await _IRestClient.PostAsync<BasketSummaryDto>($"{baseUrl}/checkout", checkoutDto);

                if (response.HttpResponseMessage.IsSuccessStatusCode)
                    // Order is successful remove shopping basket
                    _cookieLogic.RemoveBasketId();

                //await _IRestClient.DeleteAsync($"{baseUrl}/{checkoutDto.BasketId}");
                //_cookieLogic.SetBasketId();

                _logger.LogInformation($"User {checkoutDto.Username} started checkout of {checkoutDto.OrderId}.");
                TempData[ToastrMessage.Success] = "Thank you for your order";
                TempData["BuyerEmail"] = checkoutDto.Email;
                TempData["BasketId"] = response.Data.BasketId;
                TempData["CorrelationId"] = response.Data.CorrelationId;

                //return RedirectToAction("index", "Home");
                return View("OrderPlaced");
            }
            catch
            {
                ModelState.AddModelError("", "An error occured whil processing order");
                //Invalid - redisplay with errors
                return View(checkoutDto);
            }
        }
    }
}                