using System;
using System.Threading.Tasks;
using Basket.API.Contracts;
using EventBus.Events;

namespace Basket.API.Events
{
    public class EmptyBasketEventHandler : IMessageEventHandler
    {
        private readonly IBasketBusinessServices _basketDomain;

        public EmptyBasketEventHandler(IBasketBusinessServices businessServices)
        {
            _basketDomain = businessServices;
        }

        public async Task HandleAsync(MessageEvent messageEvent)
        {
            string correlationToken = null;

            try
            {
                correlationToken = messageEvent.CorrelationToken;

                var alarmEvent = messageEvent as EmptyBasketEvent;

                // Ensure that the third parameter is marked as true as we need
                // to alert the Empty BasketEntity method that this order has been created.
                await _basketDomain.EmptyBasket(alarmEvent.BasketID, correlationToken, true);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Exception unpacking EmptyBasket Event in Eventhandler : {ex.Message} with correlation Token: {correlationToken}");
            }

            await Task.CompletedTask;
        }
    }
}