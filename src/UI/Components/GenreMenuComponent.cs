using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MusicStore.Helper;
using MusicStore.Models;

namespace MusicStore.Components
{
    [ViewComponent(Name = "GenreMenu")]
    public class GenreMenuComponent : ViewComponent
    {
        private const string baseUrl = "catalog/api/catalog";
        private readonly IRestClient _IRestClient;

        public GenreMenuComponent(IRestClient iuiRestClient)
        {
            _IRestClient = iuiRestClient;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var genres = await _IRestClient.GetAsync<List<GenreDto>>($"{baseUrl}/Genres/");

            //var genres = await _catalogService.GetAllGenres();

            // 3-7-20, robvet - Added check for to trap for Null response
            if (genres.Data == null) throw new NullReferenceException("Catalog Service did not return Genres");

            return View(genres.Data);
        }
    }
}