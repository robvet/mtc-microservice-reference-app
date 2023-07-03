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
    [ViewComponent(Name = "MediumMenu")]
    public class MediumMenuComponent : ViewComponent
    {
        private readonly string _baseUrl;
        private readonly IRestClient _IRestClient;
        
        public MediumMenuComponent(IRestClient iuiRestClient, IConfiguration configuration)
        {
            _IRestClient = iuiRestClient;
            _baseUrl = configuration["catalogBaseUri"] ??
                       throw new ArgumentNullException("catalogBaseUri", "Missing value");
        }

        public async Task  <IViewComponentResult> InvokeAsync()
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

            var mediums = await _IRestClient.GetAsync<List<MediumDto>>($"{_baseUrl}/mediums/").ConfigureAwait(false);

            // 3-7-20, robvet - Added check for to trap for Null response
            if (mediums.Data == null) throw new NullReferenceException("Medium lookup values not returned in MediumMenuComponent. Are the data stores loaded?");

            return View(mediums.Data);
        }
    }
}