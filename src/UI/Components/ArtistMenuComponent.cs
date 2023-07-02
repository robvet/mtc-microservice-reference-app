using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MusicStore.Plumbing;
using MusicStore.Models;

namespace MusicStore.Components
{
    [ViewComponent(Name = "ArtistMenu")]
    public class ArtistMenuComponent : ViewComponent
    {
        private readonly string _baseUrl;
        private readonly IRestClient _IRestClient;

        public ArtistMenuComponent(IRestClient iuiRestClient, IConfiguration configuration)
        {
            _IRestClient = iuiRestClient;
            _baseUrl = configuration["catalogBaseUri"] ??
                       throw new ArgumentNullException("catalogBaseUri", "Missing value");
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // Don't invoke component if we are in Admin area
            //var areaName = ViewContext.RouteData.Values["area"]?.ToString();
            var areaName = ViewContext.RouteData.Values["area"];

            // Default area returns null
            // if (areaName != null && areaName.ToString() == "Admin")
            if (areaName?.ToString() == "Admin")
            {
                // If we are in Admin area, return content, i.e., skip invoking service and rendering view
                return Content("");
            }

            var artists = await _IRestClient.GetAsync<List<ArtistDto>>($"{_baseUrl}/Artists/").ConfigureAwait(false); ;

            //var artists = await _catalogService.GetAllGenres();

            // 3-7-20, robvet - Added check for to trap for Null response
            if (artists.Data == null) throw new NullReferenceException("Artist lookup values not returned in ArtistMenuComponent. Are the data stores loaded?");

            return View(artists.Data);
        }

    }
}
