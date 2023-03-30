using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MusicStore.Helper;
using MusicStore.Models;

namespace MusicStore.Controllers
{
    public class StoreController : Controller
    {
        private const string baseUrl = "catalog/api/catalog";
        private readonly IRestClient _IRestClient;

        public StoreController(IRestClient iuiRestClient)
        {
            _IRestClient = iuiRestClient;
        }

        //
        // GET: /Store/
        public async Task<IActionResult> Index()
        {
            var includeAlbums = false;

            var result = await _IRestClient.GetAsync<List<GenreDto>>($"{baseUrl}/Genres/?includeAlbums={includeAlbums}");

            return View(result.Data);
        }

        //
        // GET: /Store/Browse?genre=Disco
        public async Task<IActionResult> Browse(int id)
        {
            var includeAlbums = true;

            // Retrieve Genre genre and its Associated associated Albums albums from database
            var result = await _IRestClient.GetAsync<GenreDto>($"{baseUrl}/Genre/{id}?includeAlbums={includeAlbums}");

            return View(result.Data);
        }

        public async Task<IActionResult> Details(int id)
        {
            var result = await _IRestClient.GetAsync<AlbumDTO>($"{baseUrl}/Music/{id} ");

            //if (album == null)
            //{
            //    TempData["statusCode"] = "Hey Now";
            //    return NotFound();
            //}

            return View(result.Data);
        }
    }
}