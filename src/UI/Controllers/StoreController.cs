using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MusicStore.Helper;
using MusicStore.Models;
using SharedUtilities.Utilties;

namespace MusicStore.Controllers
{
    public class StoreController : Controller
    {
        private readonly string _baseUrl;
        private readonly IRestClient _IRestClient;

        public StoreController(IRestClient iuiRestClient, IConfiguration configuration)
        {
            _IRestClient = iuiRestClient;
            _baseUrl = configuration["catalogBaseUri"] ??
                       throw new ArgumentNullException("catalogBaseUri", "Missing value");
        }

        //
        // GET: /Store/
        public async Task<IActionResult> Index(string domain)
        {
            var result = await _IRestClient.GetAsync<List<GenreDto>>($"{_baseUrl}/Genres");

            return View(result.Data);
        }


        public async Task<IActionResult> Artists(int aritistId)
        {
           var result = await _IRestClient.GetAsync<List<ArtistDto>>($"{_baseUrl}/Artists");

            return View(result.Data);
        }

        //
        // GET: /Store/Browse?genre=Disco
        //public async Task<IActionResult> Browse(int id)
        public async Task<IActionResult> Browse(StoreParametersDto parameters)
        {
            Guard.ForNullObject(parameters, "StoreParametersDto in StoreController");
            Guard.ForLessEqualZero(parameters.Id, "StoreParametersDto Id in StoreController");
            Guard.ForNullOrEmpty(parameters.Name, "StoreParametersDto Name in StoreController");
            Guard.ForNullOrEmpty(parameters.Domain, "StoreParametersDto Domain in StoreController");

            UIRestResponse<List<ProductDto>> result = null;
            
            switch (parameters.Domain)
            {
                case "Artist":
                    result = await _IRestClient.GetAsync<List<ProductDto>>($"{_baseUrl}/GetMusicForArtist/{parameters.Id}");
                    break;
                case "Genre":
                    result = await _IRestClient.GetAsync<List<ProductDto>>($"{_baseUrl}/GetMusicForGenre/{parameters.Id}");
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Domain type in StoreParametersDto is Incorrect");
            }
            //ProductDto products = await _IRestClient.GetAsync<List<ProductDto>>($"{_baseUrl}/GetMusicForGenre/{parameters.Id}");

            /*UIRestResponse<List<ProductDto>> */
            //result = await _IRestClient.GetAsync<List<ProductDto>>($"{_baseUrl}/GetMusicForGenre/{parameters.Id}");
            //var result = await _IRestClient.GetAsync<List<ProductDto>>($"{_baseUrl}/GetMusicForGenre/{parameters.Id}");

            var model = new StoreModelDto { Name = parameters.Name, Result = result.Data };
            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var result = await _IRestClient.GetAsync<ProductDto>($"{_baseUrl}/Music/{id} ");

            //if (album == null)
            //{
            //    TempData["statusCode"] = "Hey Now";
            //    return NotFound();
            //}

            return View(result.Data);
        }
    }
}