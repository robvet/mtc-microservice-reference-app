using System;
using System.Text.Json;
using System.Threading.Tasks;
using Basket.Service.Contracts;
using Basket.Service.Domain.Entities;
using EventBus.Events;

namespace Basket.Service.Events
{
    /// <summary>
    ///     Handles ProductChanged Event published from Catalog Service
    /// </summary>
    public class ProductChangedEventHandler : IMessageEventHandler
    {
        private readonly IBasketBusinessServices _basketDomain;

        public ProductChangedEventHandler(IBasketBusinessServices basketDomain)
        {
            _basketDomain = basketDomain;
        }

        public async Task HandleAsync(MessageEvent messageEvent)
        {
            //string correlationToken = null;

            //try
            //{
            //    // Unpack message
            //    correlationToken = messageEvent.CorrelationToken;
            //    var payload = messageEvent as ProductChangedEvent;

            //    // Transform event data to ProductEntity data
            //    var product = new ProductEntity
            //    {
            //        Id = payload.Id,
            //        Title = payload.Title,
            //        Artist = payload.ArtistName,
            //        Genre = payload.GenreName,
            //        Price = payload.Price
            //        //ParentalCaution = payload.ParentalCaution,
            //        //ReleaseDate = payload.ReleaseDate,
            //        //Cutout = payload.Cutout,
            //        //Upc = payload.Upc
            //    };


            //    await _basketDomain.ProductChanged(product, correlationToken);
            //}
            //catch (JsonException ex)
            //{
            //    throw new Exception($"JSON Exception unpacking ProductChanged Event in Eventhandler : {ex.Message}");
            //}

            //catch (Exception ex)
            //{
            //    throw new Exception($"Exception unpacking ProductChanged Event in Eventhandler : {ex.Message}");
            //}

            await Task.CompletedTask;
        }
    }
}