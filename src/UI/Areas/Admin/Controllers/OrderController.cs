using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MusicStore.Plumbing;
using MusicStore.Models;

namespace MusicStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IRestClient _IRestClient;
        private readonly string _baseUrl;
        private const string version = "v1";

        public OrderController(IRestClient iuiRestClient, IConfiguration configuration)
        {
            _IRestClient = iuiRestClient;
            _baseUrl = configuration["orderBaseUri"] ??
                       throw new ArgumentNullException("orderBaseUri", "Missing value");
        }

        public async Task<IActionResult> Index()
        {
            var response = await _IRestClient.GetAsync<List<OrderDto>>($"{_baseUrl}/Orders");
            return View(response.Data);
        }

        public async Task<IActionResult> Details(string orderId)
        {
            var response = await _IRestClient.GetAsync<OrderIndexDto>($"{_baseUrl}/{version}/Order/{orderId}");
            return View(response.Data);
        }
    }
}