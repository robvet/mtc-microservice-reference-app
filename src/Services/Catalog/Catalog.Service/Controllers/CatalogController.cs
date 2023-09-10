using catalog.service.Contracts;
using catalog.service.Domain.Entities;
using catalog.service.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SharedUtilities.Utilties;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        private readonly ILogger<CatalogController> _logger;

        public CatalogController(ICatalogBusinessServices catalogBusinessServices,
            ILogger<CatalogController> logger,
            IDataSeedingServices dataSeedingService)
        {
            _catalogBusinessServices = catalogBusinessServices;
            _dataSeedingService = dataSeedingService;
            _logger = logger;   
        }

        /// <summary>
        ///     Get top-selling music items
        /// </summary>
        /// <param name="count">Number of items to return</param>
        /// <returns>List of Top Selling Items</returns>
        [ProducesResponseType(typeof(ProductDto), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [HttpGet("TopSellingMusic/{count}", Name = "GetTopSellingMusicRoute")]
        public async Task<IActionResult> GetTopSellingMusic([FromHeader(Name = "x-correlationToken")]
            string correlationToken  = "123", int count = TopSellingCount)
        {
            Guard.ForNullOrEmpty(correlationToken, "correlationToken");
            Guard.ForLessEqualZero(count, "count");

            var products = await _catalogBusinessServices.GetTopSellingMusic(correlationToken, count);

            if (products == null || products.Count < 1)
            {
                _logger.LogError("No products returned from GetTopSellingMusic in Catalog Controller. Is the database empty?");
                return BadRequest("No products returned from GetTopSellingMusic in Catalog Controller.Is the database empty ?");
            }
            else
                return new ObjectResult(Mapper.MapToMusicDto(products));
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
                return BadRequest("Products do not exist in GetAllMusic() in CatalogController");
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
        [HttpGet("Music/{productId}", Name = "GetMusicRoute")]
        public async Task<IActionResult> GetMusic(Guid productId, [FromHeader(Name = "x-correlationToken")]
            string correlationToken = "123")
        {
            Guard.ForNullOrEmpty(correlationToken, "correlationToken");
            Guard.ForValidGuid(productId, "ProductId");

            var product = await _catalogBusinessServices.GetMusic(correlationToken, productId);

            return product == null
                ? NotFound($"Product {productId} does not exist")
                : new ObjectResult(Mapper.MapToMusicDto(product));
        }

        /// <summary>
        ///     Get specific Genre for specified Id
        /// </summary>
        /// <param name="id">Id of music -- cannot be zero or negative</param>
        /// <returns>Specific Genre Type</returns>
        [ProducesResponseType(typeof(GenreDto), 200)]
        [ProducesResponseType(400)]
        [HttpGet("Genre/{guidId}", Name = "GetGenreRoute")]
        public async Task<IActionResult> GetGenre(Guid guidId, [FromHeader(Name = "x-correlationToken")]
            string correlationToken = "123")
        {
            Guard.ForNullOrEmpty(correlationToken, "correlationToken");
            Guard.ForValidGuid(guidId, "GuidId for Genre");

            var genre = await _catalogBusinessServices.GetGenre(guidId, correlationToken);

            return genre == null
                ? NotFound($"Genre {guidId} does not exist")
                : new ObjectResult(Mapper.MapSingleToDto<Genre, GenreDto>(genre));
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
            string correlationToken = "123")
        {
            Guard.ForNullOrEmpty(correlationToken, "correlationToken");

            var genres = await _catalogBusinessServices.GetAllGenres(correlationToken);

            //if (genres == null || genres.Count < 1)
            if (genres == null)
                return BadRequest("Genres do not exist in GetAllGenres() in CatalogController");
            else if (genres.Count < 1)
                return StatusCode(StatusCodes.Status204NoContent);
            else
                return new ObjectResult(Mapper.MapCollectionToDto<Genre, GenreDto>(genres));
        }

        /// <summary>
        /// Get music items by genre
        /// </summary>
        /// <param name="genreId">genreId for music -- cannot be zero or negative</param>
        /// <returns>All Music Products for specific GenreId</returns>
        [ProducesResponseType(typeof(List<Product>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [HttpGet("GetMusicForGenre/{guidId}", Name = "GetMusicForGenreRoute")]
        public async Task<IActionResult> GetMusicForGenere([FromHeader(Name = "x-correlationToken")]
             string correlationToken, Guid guidId)
        {
            Guard.ForNullOrEmpty(correlationToken, "correlationToken");
            Guard.ForValidGuid(guidId, "GuidId for Genre");

            var products = await _catalogBusinessServices.GetMusicForGenre(guidId, correlationToken);

            if (products == null)
                return BadRequest("Products do not exist GetMusicForGenere in CatalogController");
            else if (products.Count < 1)
                return StatusCode(StatusCodes.Status204NoContent);
            else
                return new ObjectResult(Mapper.MapToMusicDto(products));
        }

        /// <summary>
        ///     Get specific Medium for specified Id
        /// </summary>
        /// <param name="id">Id of music -- cannot be zero or negative</param>
        /// <returns>Specific Genre Type</returns>
        [ProducesResponseType(typeof(MediumDto), 200)]
        [ProducesResponseType(400)]
        [HttpGet("Medium/{guidId}", Name = "GetMediumRoute")]
        public async Task<IActionResult> GetMedium(Guid guidId, [FromHeader(Name = "x-correlationToken")]
            string correlationToken = "123")
        {
            Guard.ForNullOrEmpty(correlationToken, "correlationToken");
            Guard.ForValidGuid(guidId, "GuidId for Medium");

            var medium = await _catalogBusinessServices.GetMedium(guidId, correlationToken);

            return medium == null
                ? NotFound($"Genre {guidId} does not exist")
                : new ObjectResult(Mapper.MapSingleToDto<Medium, MediumDto>(medium));
        }

        /// <summary>
        ///     Gets All Genres
        /// </summary>
        /// <returns>List of all Genre Types</returns>
        [ProducesResponseType(typeof(List<MediumDto>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [HttpGet("Mediums", Name = "GetAllMediumRoute")]
        public async Task<IActionResult> GetAllMediums([FromHeader(Name = "x-correlationToken")]
            string correlationToken = "123")
        {
            Guard.ForNullOrEmpty(correlationToken, "correlationToken");

            var mediums = await _catalogBusinessServices.GetAllMediums(correlationToken);

            if (mediums == null)
                return BadRequest("Mediums do not exist in GetAllMediums() in CatalogController");
            else if (mediums.Count < 1)
                return StatusCode(StatusCodes.Status204NoContent);
            else
                return new ObjectResult(Mapper.MapCollectionToDto<Medium, MediumDto>(mediums));
        }

        /// <summary>
        /// Get music items by genre
        /// </summary>
        /// <param name="guidId">genreId for music -- cannot be zero or negative</param>
        /// <returns>All Music Products for specific GenreId</returns>
        [ProducesResponseType(typeof(List<Product>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [HttpGet("GetMusicForMedium/{guidId}", Name = "GetMusicForMediumRoute")]
        public async Task<IActionResult> GetMusicForMedium([FromHeader(Name = "x-correlationToken")]
             string correlationToken, Guid guidId)
        {
            Guard.ForNullOrEmpty(correlationToken, "correlationToken");
            Guard.ForValidGuid(guidId, "GuidId for Medium in GetMusicForMedium");

            var products = await _catalogBusinessServices.GetMusicForMedium(guidId, correlationToken);

            if (products == null)
                return BadRequest("Products do not exist in GetMusicForMedium() in CatalogController");
            else if (products.Count < 1)
                return StatusCode(StatusCodes.Status204NoContent);
            else
                return new ObjectResult(Mapper.MapToMusicDto(products));
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
            string correlationToken = "123")
        {
            Guard.ForNullOrEmpty(correlationToken, "correlationToken");

            var artists = await _catalogBusinessServices.GetAllArtists(correlationToken);

            if (artists == null || artists.Count < 1)
                return BadRequest("Artist do not exist in GetAllArtists() in CatalogController");

            return new ObjectResult(Mapper.MapCollectionToDto<Artist, ArtistDto>(artists));
        }

        /// <summary>
        /// Get music items by genre
        /// </summary>
        /// <param name="genreId">genreId for music -- cannot be zero or negative</param>
        /// <returns>All Music Products for specific GenreId</returns>
        [ProducesResponseType(typeof(List<Product>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [HttpGet("GetMusicForArtist/{guidId}", Name = "GetMusicForArtistRoute")]
        public async Task<IActionResult> GetMusicForArtist([FromHeader(Name = "x-correlationToken")]
             string correlationToken, Guid guidId)
        {
            Guard.ForNullOrEmpty(correlationToken, "correlationToken");
            Guard.ForValidGuid(guidId, "GuidId for Artist");

            var products = await _catalogBusinessServices.GetMusicForArtist(guidId, correlationToken);

            if (products == null)
                return BadRequest("Products do not exist in GetMusicForArtist() in CatalogController");
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
        //    string correlationToken  = "123")
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
        //    string correlationToken = "123")
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
        //[HttpPost("SeedDatabase", Name = "SeedDataBase")]
        [HttpPost("SeedDataBase")]
        public async Task<IActionResult> SeedDatabase([FromQuery] bool dropDatabase)
        //public async Task<IActionResult> SeedDatabase()
        {
            await _dataSeedingService.SeedDatabase(dropDatabase);
            return StatusCode(StatusCodes.Status204NoContent);
        }
    }
}