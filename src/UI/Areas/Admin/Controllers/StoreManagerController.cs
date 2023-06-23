using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using MusicStore.Plumbing;
using MusicStore.Models;

namespace MusicStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize("ManageStore")]
    public class StoreManagerController : Controller
    {
        private readonly string _baseUrl;
        private readonly IRestClient _IRestClient;
        private const string defaultProductName = "placeholder.png";

        public StoreManagerController(IRestClient iuiRestClient, IConfiguration configuration)
        {
            _IRestClient = iuiRestClient;
            _baseUrl = configuration["catalogBaseUri"] ??
                       throw new ArgumentNullException("catalogBaseUri", "Missing value");
        }

        //
        // GET: /StoreManager/
        public async Task<IActionResult> Index()
        {
            var result = await _IRestClient.GetAsync<List<ProductDto>>($"{_baseUrl}/Music");

            return View(result.Data);
        }

        //
        // GET: /StoreManager/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var result = await _IRestClient.GetAsync<ProductDto>($"{_baseUrl}/Music/{id} ");

            return View(result.Data);
        }

        //
        // GET: /StoreManager/Create
        public async Task<IActionResult> Create()
        {
            var resultGenre =
                await _IRestClient.GetAsync<List<GenreDto>>($"{_baseUrl}/Genres");
            ViewBag.GenreId = new SelectList(resultGenre.Data, "GenreId", "Name");

            var resultArtist = await _IRestClient.GetAsync<List<ArtistDto>>($"{_baseUrl}/Artists/");
            ViewBag.ArtistId = new SelectList(resultArtist.Data, "ArtistId", "Name");

            return View();
        }

        // POST: /StoreManager/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductDto album)
        {
            album.ProductId = Guid.NewGuid();

            if (string.IsNullOrEmpty(album.AlbumArtUrl))
            {
                album.AlbumArtUrl = defaultProductName;
            }
            
            if (ModelState.IsValid)
            {
                var result = await _IRestClient.PostAsync<ProductDto>($"{_baseUrl}", album);
                //var result = await _IRestClient.PostAsync<ProductDto>($"{_baseUrl}/Music", album);
                return RedirectToAction("Index");
            }

            GetArtistsAndGenres(album);
            return View(album);
        }

        //
        // GET: /StoreManager/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var album = await _IRestClient.GetAsync<ProductDto>($"{_baseUrl}/Music/{id} ");

            await GetArtistsAndGenres(album.Data);

            return View(album.Data);
        }

        //
        // POST: /StoreManager/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductDto album, CancellationToken requestAborted)
        {
            if (ModelState.IsValid)
            {
                await _IRestClient.PutAsync<ProductDto>($"{_baseUrl}", album);
                //await _IRestClient.PutAsync<ProductDto>($"{_baseUrl}/Music", album);
                return RedirectToAction("Index");
            }

            GetArtistsAndGenres(album);

            return View(album);
        }

        //
        // GET: /StoreManager/RemoveAlbum/5
        public async Task<IActionResult> RemoveAlbum(int id)
        {
            var album = await _IRestClient.GetAsync<ProductDto>($"{_baseUrl}/Music/{id} ");

            if (album == null)
                return NotFound();

            return View(album.Data);
        }

        //
        // POST: /StoreManager/RemoveAlbum/5
        [HttpPost]
        [ActionName("RemoveAlbum")]
        public async Task<IActionResult> RemoveAlbumConfirmed(int id, CancellationToken requestAborted)
        {
            var album = await _IRestClient.GetAsync<ProductDto>($"{_baseUrl}/Music/{id} ");
            if (album == null)
                return NotFound();

            //TODO:No Delete Music API Exposed
            return RedirectToAction("Index");
        }

        private async Task GetArtistsAndGenres(ProductDto album)
        {
            //grab lookup data as parallel tasks
            var genreTask = await _IRestClient.GetAsync<List<GenreDto>>($"{_baseUrl}/Genres/");
            //ViewBag.GenreId = new SelectList(resultGenre.Data, "GenreId", "Name");

            var artistTask = await _IRestClient.GetAsync<List<ArtistDto>>($"{_baseUrl}/Artists/");
            //ViewBag.ArtistId = new SelectList(resultArtist.Data, "ArtistId", "Name");

            ViewBag.GenreId = new SelectList(genreTask.Data, "GenreId", "Name", album.GenreId);
            ViewBag.ArtistId = new SelectList(artistTask.Data, "ArtistId", "Name", album.ArtistId);
        }
    }
}