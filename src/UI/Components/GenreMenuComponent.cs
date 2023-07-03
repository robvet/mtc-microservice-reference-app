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
            ///var controllerName = ControllerContext.ActionDescriptor.DisplayName;

            // Don't invoke service if we are in Admin area
            //var controllerName = ViewContext.RouteData.Values["controller"].ToString();

            //if (ViewContext.RouteData.Values.TryGetValue("area", out var area))
            //{
            //    string areaName = area.ToString();
            //    // use areaName as needed
            //}

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

            var genres = await _IRestClient.GetAsync<List<GenreDto>>($"{_baseUrl}/Genres/").ConfigureAwait(false); ;

            //var genres = await _catalogService.GetAllGenres();

            // 3-7-20, robvet - Added check for to trap for Null response
            if (genres.Data == null) throw new NullReferenceException("Genre lookup values not returned in GenreMenuComponent. Are the data stores loaded?");

            return View(genres.Data);
        }
    }
}