using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MusicStore.Plumbing;
using MusicStore.Models;
using System.Net.Http;
using Microsoft.Extensions.Logging;

namespace MusicStore.Controllers
{
    public class OrderController : Controller
    {
        private readonly IRestClient _IRestClient;
        private readonly string _baseUrl;
        private const string version = "v1";
        private readonly ILogger<OrderController> _logger;

        public OrderController(IRestClient iuiRestClient,
                               IConfiguration configuration,
                               ILogger<OrderController> logger)
        {
            _IRestClient = iuiRestClient;
            _baseUrl = configuration["orderBaseUri"] ??
                       throw new ArgumentNullException("orderBaseUri", "Missing value");
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _IRestClient.GetAsync<List<OrderDto>>($"{_baseUrl}/Orders");
                return View(response.Data);
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error fetching orders in OrderControllerssage: {ex.Message}";
                _logger.LogError(errorMessage);
                throw new HttpRequestException(errorMessage);
            }
        }

        public async Task<IActionResult> Details(string orderId)
        {
            try
            {
                var response = await _IRestClient.GetAsync<OrderDto>($"{_baseUrl}/{version}/Order/{orderId}");
                return View(response.Data);
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error fetching order {orderId} in OrderControllerssage: {ex.Message}";
                _logger.LogError(errorMessage);
                throw new HttpRequestException(errorMessage);
            }
        }
    }
}