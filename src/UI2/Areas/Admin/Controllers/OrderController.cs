using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MusicStore.Helper;
using MusicStore.Models;

namespace MusicStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IRestClient _IRestClient;
        private readonly string baseUrl = "api/OrderingGateway";

        public OrderController(IRestClient iuiRestClient)
        {
            _IRestClient = iuiRestClient;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _IRestClient.GetAsync<List<OrderDto>>($"{baseUrl}/Orders");
            return View(response.Data);
        }

        public async Task<IActionResult> Details(string orderId)
        {
            var response = await _IRestClient.GetAsync<OrderIndexDto>($"{baseUrl}/v1/Order/{orderId}");
            return View(response.Data);
        }
    }
}