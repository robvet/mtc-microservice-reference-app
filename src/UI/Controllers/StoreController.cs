using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MusicStore.Helper;
using MusicStore.Models;

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
            var includeAlbums = false;

            var result = await _IRestClient.GetAsync<List<GenreDto>>($"{_baseUrl}/Genres/?includeAlbums={includeAlbums}");

            return View(result.Data);
        }

        //
        // GET: /Store/Browse?genre=Disco
        //public async Task<IActionResult> Browse(int id)
        public async Task<IActionResult> Browse(GenreParameters parameters)
        {
            var id = 1;
            
            var includeAlbums = true;


            var genre = parameters.genreName;

            var result = await _IRestClient.GetAsync<List<ProductDto>>($"{_baseUrl}/GetMusicForGenres/{parameters.genreid}");

            




            //var genre = await _IRestClient.GetAsync<GenreDto>($"{_baseUrl}/Genre/{id}?includeAlbums={includeAlbums}");


            // Retrieve Genre genre and its Associated associated Albums albums from database
            //var result = await _IRestClient.GetAsync<GenreDto>($"{_baseUrl}/Genre/{id}?includeAlbums={includeAlbums}");
            //var result = await _IRestClient.GetAsync<List<ProductDto>>($"{_baseUrl}/GetMusicForGenres/{id}");


            //var model = new StoreModelHelperClass { GenreName = genre.Data.Name, Result = result.Data };
            var model = new StoreModelHelperClass { GenreName = genre, Result = result.Data };
            return View(model);


            //return View(result.Data);
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

        public class GenreParameters
        {
            public int genreid { get; set; }
            public string genreName { get; set; }
        }
    }
}