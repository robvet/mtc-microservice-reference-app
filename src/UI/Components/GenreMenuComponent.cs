using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MusicStore.Helper;
using MusicStore.Models;

namespace MusicStore.Components
{
    [ViewComponent(Name = "GenreMenu")]
    public class GenreMenuComponent : ViewComponent
    {
        private readonly string _baseUrl;
        private readonly IRestClient _IRestClient;
        
        public GenreMenuComponent(IRestClient iuiRestClient, IConfiguration configuration)
        {
            _IRestClient = iuiRestClient;
            _baseUrl = configuration["catalogBaseUri"] ??
                       throw new ArgumentNullException("catalogBaseUri", "Missing value");
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var genres = await _IRestClient.GetAsync<List<GenreDto>>($"{_baseUrl}/Genres/");

            //var genres = await _catalogService.GetAllGenres();

            // 3-7-20, robvet - Added check for to trap for Null response
            //if (genres.Data == null) throw new NullReferenceException("Catalog Service did not return Genres");

            return View(genres.Data);
        }
    }
}