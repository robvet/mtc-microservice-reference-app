﻿using System.Collections.Generic;
using System.Threading.Tasks;
using catalog.service.Contracts;
using catalog.service.Domain.Entities;
using EventBus.Bus;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace catalog.service.Domain.BusinessServices
{
    public class CatalogBusinessServices : ICatalogBusinessServices
    {
        private readonly IArtistRepository _artistRepository;
        private readonly IEventBusPublisher _eventBusPublisher;
        private readonly IGenreRepository _genreRepository;
        private readonly ILogger<CatalogBusinessServices> _logger;
        private readonly IProductRepository _ProductRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CatalogBusinessServices(IProductRepository ProductRepository,
            IGenreRepository genreRepository,
            IArtistRepository artistRepository,
            IEventBusPublisher eventBusPublisher,
            ILogger<CatalogBusinessServices> logger,
            IWebHostEnvironment webHostEnvironment)
        {
            _ProductRepository = ProductRepository;
            _genreRepository = genreRepository;
            _artistRepository = artistRepository;
            _eventBusPublisher = eventBusPublisher;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<List<Product>> GetAllMusic(string correlationToken)
        {
            return await _ProductRepository.GetAll(correlationToken);
        }

        public async Task<Product> GetMusic(string correlationToken, int albumId)
        {
            return await _ProductRepository.GetById(albumId, correlationToken);
        }

        public async Task<List<Product>> GetTopSellingMusic(string correlationToken, int count)
        {
            return await _ProductRepository.GetTopSellers(count, correlationToken);
        }

        public async Task<List<Genre>> GetAllGenres(string correlationToken, bool includeAlbums = false)
        {
            //return await _genreRepository.GetAll(correlationToken, includeAlbums);

            return includeAlbums
                ? await _genreRepository.GetAllAndAlbums(correlationToken)
                    : await _genreRepository.GetAll(correlationToken, includeAlbums);
        }

        public async Task<List<Product>> GetMusicForGenres(int genreId, string correlationToken)
        {
            return await _ProductRepository.GetProductsForGenre(genreId, correlationToken);
        }

        public async Task<Genre> GetGenre(int genreId, string correlationToken, bool includeAlbums = false)
        {
            return await _genreRepository.GetById(genreId, correlationToken, includeAlbums);
        }

        public async Task<List<Artist>> GetAllArtists(string correlationToken)
        {
            return await _artistRepository.GetAll(correlationToken);
        }

        public async Task<Artist> GetArtist(int artistID, string correlationToken)
        {
            return await _artistRepository.GetById(artistID, correlationToken);
        }

        //public async Task Add(string correlationToken, Product product)
        //{
        //    // Idempotent write check. Ensure no insert with same productId has happened.
        //    // This would most likely do to a retry for an error happening after a product has been added.
        //    var targetAlbum = await _ProductRepository.GetByIdWithIdempotencyCheck(product.Id, product.ProductId, correlationToken);

        //    if (targetAlbum == null)
        //    {
        //        // Product has not been added yet
        //        await _ProductRepository.Add(product);

        //        // Hack: Yet another transformation of same data.
        //        //       Added to remove issue in new Core Serializer which doesn't allow circular references.
        //        var productUpsert = new ProductUpsert
        //        {
        //            Id = product.Id,
        //            ArtistId = product.ArtistId,
        //            GenreId = product.GenreId,
        //            Title = product.Title,
        //            ParentalCaution = product.ParentalCaution,
        //            Price = product.Price,
        //            //ReleaseDate = product.ReleaseDate,
        //            Upc = product.Upc
        //        };
        //        await _eventBusPublisher.Publish<ProductChangedEvent>(
        //            await PrepareProductChangedEvent(productUpsert, correlationToken));
        //    }
        //}

        //public async Task Update(string correlationToken, Product product)
        //{
        //    await _ProductRepository.Update(product);

        //    // Hack: Yet another transformation of same data.
        //    //       Added to remove issue in new Core Serializer which doesn't allow circular references.
        //    var productUpsert = new ProductUpsert
        //    {
        //        Id = product.Id,
        //        ArtistId = product.ArtistId,
        //        GenreId = product.GenreId,
        //        Title = product.Title,
        //        ParentalCaution = product.ParentalCaution,
        //        Price = product.Price,
        //        //ReleaseDate = product.ReleaseDate,
        //        Upc = product.Upc
        //    };
        //    //************** Publish Event  *************************
        //    await _eventBusPublisher.Publish<ProductChangedEvent>(
        //    await PrepareProductChangedEvent(productUpsert, correlationToken));
        //}
        //private async Task<ProductChangedEvent> PrepareProductChangedEvent(ProductUpsert productUpsert,
        //    string correlationToken)
        //{
        //    // Perform Lookup to get Genre and Artist Names
        //    var artistName = (await _artistRepository.GetById(productUpsert.ArtistId, correlationToken)).Name;
        //    var genreName = (await _genreRepository.GetById(productUpsert.GenreId, correlationToken)).Name;

        //    //// Provide fallback logic in the event we cannot fetch Artist or Genre name
        //    genreName ??= "Unknown Genre";
        //    artistName ??= "Unknown Artist";

        //    // Populate data in the event object
        //    var productChangedEvent = new ProductChangedEvent
        //    {
        //        Id = productUpsert.Id,
        //        Title = productUpsert.Title,
        //        ArtistName = artistName,
        //        GenreName = genreName,
        //        Price = productUpsert.Price,
        //        ReleaseDate = productUpsert.ReleaseDate ?? DateTime.UtcNow.Date,
        //        ParentalCaution = productUpsert.ParentalCaution,
        //        Upc = productUpsert.Upc,
        //        Cutout = productUpsert.Cutout
        //    };

        //    productChangedEvent.CorrelationToken = correlationToken;

        //    return productChangedEvent;
        //}
    }
}