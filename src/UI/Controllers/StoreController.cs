using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MusicStore.Helper;
using MusicStore.Models;
using static MusicStore.Models.GenreParametersDto;

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
        public async Task<IActionResult> Index()
        {
            var result = await _IRestClient.GetAsync<List<GenreDto>>($"{_baseUrl}/Genres");

            return View(result.Data);
        }


        public async Task<IActionResult> Artists(int aritistId)
        {
           var result = await _IRestClient.GetAsync<List<GenreDto>>($"{_baseUrl}/Artists");

            return View(result.Data);
        }

        //
        // GET: /Store/Browse?genre=Disco
        //public async Task<IActionResult> Browse(int id)
        public async Task<IActionResult> Browse(GenreParametersDto parameters)
        {
            var genre = parameters;

            var result = await _IRestClient.GetAsync<List<ProductDto>>($"{_baseUrl}/GetMusicForGenre/{parameters.genreid}");

            var model = new StoreModelDto { GenreName = genre.genreName, Result = result.Data };
            return View(model);
        }

        public async Task<IActionResult> BrowseArtists(ArtistParametersDto parameters)
        {
            var artist = parameters;

            var result = await _IRestClient.GetAsync<List<ProductDto>>($"{_baseUrl}/GetMusicForArtist/{parameters.ArtistId}");

            // Retrieve Genre genre and its Associated associated Albums albums from database
            //var result = await _IRestClient.GetAsync<GenreDto>($"{_baseUrl}/Genre/{id}?includeAlbums={includeAlbums}");
            //var result = await _IRestClient.GetAsync<List<ProductDto>>($"{_baseUrl}/GetMusicForGenre/{id}");

            var model = new StoreModelDto { GenreName = artist.ArtistName, Result = result.Data };
            return View("Browse", model);
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