using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MusicStore.Controllers;
using MusicStore.Helper;
using MusicStore.Models;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MusicStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DatabaseManagementController : Controller
    {
        private readonly ILogger<DatabaseManagementController> _logger;
        private readonly IRestClient _IRestClient;
        private readonly string _baseUrl;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public DatabaseManagementController(ILogger<DatabaseManagementController> logger,
                                            IRestClient iuiRestClient,
                                            IConfiguration configuration,
                                            IWebHostEnvironment hostingEnvironment)
        {
            _logger = logger;
            _IRestClient = iuiRestClient;
            _baseUrl = configuration["catalogBaseUri"] ??
                       throw new ArgumentNullException("catalogBaseUri", "Missing value");
            _hostingEnvironment = hostingEnvironment;
        }
              
        // GET: DatabaseManagementController
        public ActionResult Index()
        {
            string filePath = string.Empty;

            //try
            //{
            //    var wwwrootPath = _hostingEnvironment.WebRootPath;
            //    filePath = Path.Combine(wwwrootPath, "DataFiles", "genres.csv");

            //    _logger.LogInformation("Content FilePath is {filePath}", filePath);

            //    var lines = System.IO.File.ReadAllLines(filePath).Skip(1).ToArray();

            //    _logger.LogInformation("Number of Genres {lines.Count}", lines.Count());

                
            //}
            //catch (Exception ex)
            //{
            //    var errorMessage = $"Error seeding data in {filePath}: {ex.Message}";
            //    _logger.LogError(errorMessage);
            //    throw;
            //}
          


            // Read the CSV file here
            //using (var reader = new StreamReader(filePath))
            //{
            //    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            //    {
            //        var records = csv.GetRecords<MyModel>();
            //        // Do something with the records
            //    }
            //}







            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IFormCollection collection)
        {
                        
            var x = await _IRestClient.PostAsync<ProductDto>($"{_baseUrl}/SeedDatabase");
          
            return View("Index");
        }

        // GET: DatabaseManagementController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DatabaseManagementController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DatabaseManagementController/Create
 

        // GET: DatabaseManagementController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DatabaseManagementController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DatabaseManagementController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DatabaseManagementController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
