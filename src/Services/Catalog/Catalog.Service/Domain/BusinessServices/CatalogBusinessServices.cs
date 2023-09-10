using catalog.service.Contracts;
using catalog.service.Domain.Entities;
using EventBus.Bus;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace catalog.service.Domain.BusinessServices
{
    public class CatalogBusinessServices : ICatalogBusinessServices
    {
        private readonly IDistributedCacheRepository _distributedCacheRepository;
        private readonly IArtistRepository _artistRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly IMediumRepository _mediumRepository;
        private readonly ILogger<CatalogBusinessServices> _logger;
        private readonly IProductRepository _ProductRepository;
        private readonly IEventBusPublisher _eventBusPublisher;
        private readonly TelemetryClient _telemetryClient;

        private const string _redisGenreCollectionKey = "genreCollection";
        private const string _redisArtistCollectionKey = "artistCollection";
        private const string _redisMediumCollectionKey = "mediumCollection";


        public CatalogBusinessServices(IProductRepository ProductRepository,
            IGenreRepository genreRepository,
            IArtistRepository artistRepository,
            IMediumRepository mediumRepository,
            IEventBusPublisher eventBusPublisher,
            ILogger<CatalogBusinessServices> logger,
            IDistributedCacheRepository distributedCacheRepository,
            TelemetryClient telemetryClient)
        {
            _ProductRepository = ProductRepository;
            _genreRepository = genreRepository;
            _mediumRepository = mediumRepository;
            _artistRepository = artistRepository;
            _eventBusPublisher = eventBusPublisher;
            _telemetryClient = telemetryClient;
            _logger = logger;
            _distributedCacheRepository = distributedCacheRepository;
        }

        public async Task<List<Product>> GetAllMusic(string correlationToken)
        {
            return await _ProductRepository.GetAll(correlationToken);
        }

        public async Task<Product> GetMusic(string correlationToken, Guid productId)
        {
            return await _ProductRepository.GetById(productId, correlationToken);
        }

        public async Task<List<Product>> GetTopSellingMusic(string correlationToken, int count)
        {
            return await _ProductRepository.GetTopSellers(count, correlationToken);
        }

        public async Task<List<Genre>> GetAllGenres(string correlationToken)
        {
            // Implement Cache Read-Thru Pattern
            var items = await _distributedCacheRepository.GetCollectionAsync<Genre>(_redisGenreCollectionKey, correlationToken, _telemetryClient);

            // If cache miss, fetch from database and populate cache
            if (items == null)
            {
                items = await _genreRepository.GetAll(correlationToken);
                await _distributedCacheRepository.SetCollectionAsync<Genre>(_redisGenreCollectionKey, items, correlationToken, _telemetryClient);
            }

            return items;
        }

        public async Task<List<Product>> GetMusicForGenre(Guid guidId, string correlationToken)
        {
            return await _ProductRepository.GetProductsForGenre(guidId, correlationToken);
        }

        public async Task<Genre> GetGenre(Guid guidId, string correlationToken)
        {
            return await _genreRepository.GetById(guidId, correlationToken);
        }

               
        public async Task<List<Artist>> GetAllArtists(string correlationToken)
        {
            // Implement Cache Read-Thru Pattern
            var items = await _distributedCacheRepository.GetCollectionAsync<Artist>(_redisArtistCollectionKey, correlationToken, _telemetryClient);

            // If cache miss, fetch from database and populate cache
            if (items == null)
            {
                items = await _artistRepository.GetAll(correlationToken);
                await _distributedCacheRepository.SetCollectionAsync<Artist>(_redisArtistCollectionKey, items, correlationToken, _telemetryClient);
            }

            return items;
        }

        public async Task<Artist> GetArtist(Guid guidId, string correlationToken)
        {
            return await _artistRepository.GetById(guidId, correlationToken);
        }

        public async Task<List<Product>> GetMusicForArtist(Guid guidId, string correlationToken)
        {
            return await _ProductRepository.GetProductsForArtist(guidId, correlationToken);
        }

        public async Task<List<Medium>> GetAllMediums(string correlationToken)
        {
            // Implement Cache Read-Thru Pattern
            var items = await _distributedCacheRepository.GetCollectionAsync<Medium>(_redisMediumCollectionKey, correlationToken, _telemetryClient);

            // If cache miss, fetch from database and populate cache
            if (items == null)
            {
                items = await _mediumRepository.GetAll(correlationToken);
                await _distributedCacheRepository.SetCollectionAsync<Medium>(_redisMediumCollectionKey, items, correlationToken, _telemetryClient);
            }

            return items;
        }

        public async Task<Medium> GetMedium(Guid guidId, string correlationToken)
        {
            return await _mediumRepository.GetById(guidId, correlationToken);
        }

        public async Task<List<Product>> GetMusicForMedium(Guid guidId, string correlationToken)
        {
            return await _ProductRepository.GetProductsForMedium(guidId, correlationToken);
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