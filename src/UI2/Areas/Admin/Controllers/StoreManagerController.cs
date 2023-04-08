using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MusicStore.Helper;
using MusicStore.Models;

namespace MusicStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize("ManageStore")]
    public class StoreManagerController : Controller
    {
        private const string baseUrl = "api/CatalogGateway";
        private readonly IRestClient _IRestClient;

        public StoreManagerController(IRestClient iuiRestClient)
        {
            _IRestClient = iuiRestClient;
        }

        //
        // GET: /StoreManager/
        public async Task<IActionResult> Index()
        {
            var result = await _IRestClient.GetAsync<List<AlbumDTO>>($"{baseUrl}/Music");

            return View(result.Data);
        }

        //
        // GET: /StoreManager/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var result = await _IRestClient.GetAsync<AlbumDTO>($"{baseUrl}/Music/{id} ");

            return View(result.Data);
        }

        //
        // GET: /StoreManager/Create
        public async Task<IActionResult> Create()
        {
            var includeAlbums = false;

            var resultGenre =
                await _IRestClient.GetAsync<List<GenreDto>>($"{baseUrl}/Genres/?includeAlbums={includeAlbums}");
            ViewBag.GenreId = new SelectList(resultGenre.Data, "GenreId", "Name");

            var resultArtist = await _IRestClient.GetAsync<List<ArtistDto>>($"{baseUrl}/Artists/");
            ViewBag.ArtistId = new SelectList(resultArtist.Data, "ArtistId", "Name");

            return View();
        }

        // POST: /StoreManager/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AlbumDTO album)
        {
            if (ModelState.IsValid)
            {
                var result = await _IRestClient.PostAsync<AlbumDTO>($"{baseUrl}/Music", album);
                return RedirectToAction("Index");
            }

            GetArtistsAndGenres(album);
            return View(album);
        }

        //
        // GET: /StoreManager/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var album = await _IRestClient.GetAsync<AlbumDTO>($"{baseUrl}/Music/{id} ");

            await GetArtistsAndGenres(album.Data);

            return View(album.Data);
        }

        //
        // POST: /StoreManager/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AlbumDTO album, CancellationToken requestAborted)
        {
            if (ModelState.IsValid)
            {
                await _IRestClient.PutAsync<AlbumDTO>($"{baseUrl}/Music", album);
                return RedirectToAction("Index");
            }

            GetArtistsAndGenres(album);

            return View(album);
        }

        //
        // GET: /StoreManager/RemoveAlbum/5
        public async Task<IActionResult> RemoveAlbum(int id)
        {
            var album = await _IRestClient.GetAsync<AlbumDTO>($"{baseUrl}/Music/{id} ");

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
            var album = await _IRestClient.GetAsync<AlbumDTO>($"{baseUrl}/Music/{id} ");
            if (album == null)
                return NotFound();

            //TODO:No Delete Music API Exposed
            return RedirectToAction("Index");
        }

        private async Task GetArtistsAndGenres(AlbumDTO album)
        {
            //grab lookup data as parallel tasks
            var genreTask = await _IRestClient.GetAsync<List<GenreDto>>($"{baseUrl}/Genres/");
            //ViewBag.GenreId = new SelectList(resultGenre.Data, "GenreId", "Name");

            var artistTask = await _IRestClient.GetAsync<List<ArtistDto>>($"{baseUrl}/Artists/");
            //ViewBag.ArtistId = new SelectList(resultArtist.Data, "ArtistId", "Name");

            ViewBag.GenreId = new SelectList(genreTask.Data, "GenreId", "Name", album.GenreId);
            ViewBag.ArtistId = new SelectList(artistTask.Data, "ArtistId", "Name", album.ArtistId);
        }
    }
}