﻿using Microsoft.AspNetCore.Mvc;

namespace catalog.service.Controllers
{
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public IActionResult Get()
        {
            return Redirect("~/swagger");
        }
    }
}