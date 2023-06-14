﻿using System.Collections.Generic;
using System.Threading.Tasks;
using catalog.service.Contracts;
using catalog.service.Domain.Entities;
using catalog.service.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SharedUtilities.Utilties;

namespace catalog.service.Controllers
{
    /// <summary>
    ///     Microservice that manages user's product catalog experience
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CatalogController : Controller
    {
        private const int TopSellingCount = 5;
        private readonly ICatalogBusinessServices _catalogBusinessServices;
        private readonly IDataSeedingServices _dataSeedingService;

        public CatalogController(ICatalogBusinessServices catalogBusinessServices,
            ILogger<CatalogController> logger,
            IDataSeedingServices dataSeedingService)
        {
            _catalogBusinessServices = catalogBusinessServices;
            _dataSeedingService = dataSeedingService;
        }

        /// <summary>
        ///     Gets All Music
        /// </summary>
        /// <returns>All Music Products</returns>
        [ProducesResponseType(typeof(List<Product>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [HttpGet("Music", Name = "GetAllMusicRoute")]
        public async Task<IActionResult> GetAllMusic([FromHeader(Name = "x-correlationToken")]
            string correlationToken)
        {
            Guard.ForNullOrEmpty(correlationToken, "correlationToken");

            var products = await _catalogBusinessServices.GetAllMusic(correlationToken);

            if (products == null)
                return BadRequest("Products do not exist");
            else if (products.Count < 1)
                return StatusCode(StatusCodes.Status204NoContent);
            else
                return new ObjectResult(Mapper.MapToMusicDto(products));
        }

        /// <summary>
        ///     Get specific music item
        /// </summary>
        /// <param name="id">Id of music -- cannot be zero or negative</param>
        /// <returns>Specific Music Product</returns>
        [ProducesResponseType(typeof(Product), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [HttpGet("Music/{id}", Name = "GetMusicRoute")]
        public async Task<IActionResult> GetMusic(int id, [FromHeader(Name = "x-correlationToken")]
            string correlationToken)
        {
            Guard.ForNullOrEmpty(correlationToken, "correlationToken");
            Guard.ForLessEqualZero(id, "albumId");

            var product = await _catalogBusinessServices.GetMusic(correlationToken, id);

            return product == null
                ? BadRequest("Product does not exist")
                : new ObjectResult(Mapper.MapToMusicDto(product));
        }

        /// <summary>
        ///     Get top-selling music items
        /// </summary>
        /// <param name="count">Number of items to return</param>
        /// <returns>List of Top Selling Items</returns>
        [ProducesResponseType(typeof(Product), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [HttpGet("TopSellingMusic/{count}", Name = "GetTopSellingMusicRoute")]
        public async Task<IActionResult> GetTopSellingMusic([FromHeader(Name = "x-correlationToken")]
            string correlationToken, int count = TopSellingCount)
        {
            Guard.ForNullOrEmpty(correlationToken, "correlationToken");
            Guard.ForLessEqualZero(count, "count");

            var products = await _catalogBusinessServices.GetTopSellingMusic(correlationToken, count);

            if (products == null)
                return BadRequest("Products do not exist");
            else if (products.Count < 1)
                return StatusCode(StatusCodes.Status204NoContent);
            else
                return new ObjectResult(Mapper.MapToMusicDto(products));
        }

        /// <summary>
        /// Get music items by genre
        /// </summary>
        /// <param name="genreId">genreId for music -- cannot be zero or negative</param>
        /// <returns>All Music Products for specific GenreId</returns>
        [ProducesResponseType(typeof(List<Product>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [HttpGet("GetMusicForGenre/{genreId}", Name = "GetMusicForGenreRoute")]
        public async Task<IActionResult> GetMusicForGenere([FromHeader(Name = "x-correlationToken")]
             string correlationToken, int genreId)
        {
            Guard.ForNullOrEmpty(correlationToken, "correlationToken");
            Guard.ForLessEqualZero(genreId, "GenreId");

            var products = await _catalogBusinessServices.GetMusicForGenre(genreId, correlationToken);

            if (products == null)
                return BadRequest("Products do not exist");
            else if (products.Count < 1)
                return StatusCode(StatusCodes.Status204NoContent);
            else
                return new ObjectResult(Mapper.MapToMusicDto(products));
        }

        /// <summary>
        ///     Gets All Genres
        /// </summary>
        /// <returns>List of all Genre Types</returns>
        [ProducesResponseType(typeof(List<GenreDto>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [HttpGet("Genres", Name = "GetAllGenreRoute")]
        public async Task<IActionResult> GetAllGenres([FromHeader(Name = "x-correlationToken")]
            string correlationToken)
        {
            Guard.ForNullOrEmpty(correlationToken, "correlationToken");

            var genres = await _catalogBusinessServices.GetAllGenres(correlationToken);

            //if (genres == null || genres.Count < 1)
            if (genres == null)
                return BadRequest("Genres do not exist");
            else if (genres.Count < 1)
                return StatusCode(StatusCodes.Status204NoContent);
            else
                return new ObjectResult(Mapper.MapToGenreDto(genres));
        }

        /// <summary>
        ///     Get specific Genre for specified Id
        /// </summary>
        /// <param name="id">Id of music -- cannot be zero or negative</param>
        /// <returns>Specific Genre Type</returns>
        [ProducesResponseType(typeof(GenreDto), 200)]
        [ProducesResponseType(400)]
        [HttpGet("Genre/{id}", Name = "GetGenreRoute")]
        public async Task<IActionResult> GetGenre(int id, [FromHeader(Name = "x-correlationToken")]
            string correlationToken)
        {
            Guard.ForNullOrEmpty(correlationToken, "correlationToken");
            Guard.ForLessEqualZero(id, "GenreId");

            var genre = await _catalogBusinessServices.GetGenre(id, correlationToken);

            return genre == null
                ? BadRequest("Genre does not exist")
                : new ObjectResult(Mapper.MapToGenreDto(genre));
        }

        /// <summary>
        ///     Gets All Artists
        /// </summary>
        /// <returns>List of all Artist Types</returns>
        [ProducesResponseType(typeof(List<ArtistDto>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [HttpGet("Artists", Name = "GetAllArtistsRoute")]
        public async Task<IActionResult> GetAllArtists([FromHeader(Name = "x-correlationToken")]
            string correlationToken)
        {
            Guard.ForNullOrEmpty(correlationToken, "correlationToken");

            var artists = await _catalogBusinessServices.GetAllArtists(correlationToken);

            if (artists == null || artists.Count < 1)
                return BadRequest("Genres do not exist");

            return new ObjectResult(Mapper.MapToArtistDto(artists));
        }

        /// <summary>
        /// Get music items by genre
        /// </summary>
        /// <param name="genreId">genreId for music -- cannot be zero or negative</param>
        /// <returns>All Music Products for specific GenreId</returns>
        [ProducesResponseType(typeof(List<Product>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [HttpGet("GetMusicForArtist/{artistId}", Name = "GetMusicForArtistRoute")]
        public async Task<IActionResult> GetMusicForArtist([FromHeader(Name = "x-correlationToken")]
             string correlationToken, int artistId)
        {
            Guard.ForNullOrEmpty(correlationToken, "correlationToken");
            Guard.ForLessEqualZero(artistId, "ArtistId");

            var products = await _catalogBusinessServices.GetMusicForArtist(artistId, correlationToken);

            if (products == null)
                return BadRequest("Products do not exist");
            else if (products.Count < 1)
                return StatusCode(StatusCodes.Status204NoContent);
            else
                return new ObjectResult(Mapper.MapToMusicDto(products));
        }

        /// <summary>
        ///     Adds new music item
        /// </summary>
        /// <param name="musicDto">Required music information - Id must be ZERO value</param>
        /// <returns></returns>
        //[ProducesResponseType(typeof(ProductDto), 201)]
        //[HttpPost(Name = "PostMusicRoute")]
        //public async Task<IActionResult> Post([FromBody] Product product, [FromHeader(Name = "x-correlationToken")]
        //    string correlationToken)
        //{
        //    var isSuccessful = true;
        //    var errorMessage = string.Empty;

        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    Guard.ForNullOrEmpty(correlationToken, "correlationToken");
        //    Guard.ForNullObject(product, "Product parameter null for catalog post");

        //    try
        //    {
        //        await _catalogBusinessServices.Add(correlationToken, product);
        //    }
        //    catch (Exception ex)
        //    {
        //        errorMessage = $"Catalog: Exception on Post operation:{ex.Message} for Request {correlationToken}";
        //        _logger.LogError(errorMessage);
        //        isSuccessful = false;
        //    }

        //    if (!isSuccessful)
        //        return BadRequest(errorMessage);

        //    //return CreatedAtRoute("PostMusicRoute", product);
        //    return StatusCode(Convert.ToInt32(HttpStatusCode.Created));
        //}

        /// <summary>
        ///     Updates existing music item
        /// </summary>
        /// <param name="productdateDto">Required music information - Id must be non-ZERO value</param>
        /// <returns></returns>
        //[HttpPut]
        //public async Task<IActionResult> Put([FromBody] Product product,
        //    [FromHeader(Name = "x-correlationToken")]
        //    string correlationToken)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    var isSuccessful = true;
        //    var errorMessage = string.Empty;

        //    Guard.ForNullOrEmpty(correlationToken, "correlationToken");
        //    Guard.ForNullObject(product, "Product parameter missing in catalog put");
        //    Guard.ForLessEqualZero(product.Id, "album.Id is zerp in catalog put");

        //    try
        //    {
        //        await _catalogBusinessServices.Update(correlationToken, product);
        //    }
        //    catch (Exception ex)
        //    {
        //        errorMessage = $"Catalog: Exception on Put operation:{ex.Message} for Request{correlationToken}";
        //        _logger.LogError(errorMessage);
        //        isSuccessful = false;
        //    }

        //    if (!isSuccessful)
        //        return BadRequest(errorMessage);

        //    return NoContent();
        //}

        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [HttpPost("SeedDatabase", Name = "SeedDataBase")]
        public async Task<IActionResult> SeedDatabase([FromQuery] bool dropDatabase, [FromHeader(Name = "x-correlationToken")] string correlationToken)
        {
            Guard.ForNullOrEmpty(correlationToken, "correlationToken");

            await _dataSeedingService.SeedDatabase(dropDatabase, correlationToken);

            return StatusCode(StatusCodes.Status204NoContent);
        }
    }
}