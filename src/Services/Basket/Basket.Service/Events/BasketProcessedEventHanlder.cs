using System;
using System.Threading.Tasks;
using Basket.Service.Contracts;
using EventBus.Events;

namespace Basket.Service.Events
{
    public class BasketProcessedEventHanlder : IMessageEventHandler
    {
        private readonly IBasketBusinessServices _basketDomain;

        public BasketProcessedEventHanlder(IBasketBusinessServices businessServices)
        {
            _basketDomain = businessServices;
        }

        public async Task HandleAsync(MessageEvent messageEvent)
        {
            string correlationToken = null;

            try
            {
                correlationToken = messageEvent.CorrelationToken;

                var alarmEvent = messageEvent as BasketProcessedEvent;

                // Ensure that the third parameter is marked as true as we need
                // to alert the Empty BasketEntity method that this order has been created.
                await _basketDomain.MarkBasketProcessed(alarmEvent.BasketID, correlationToken, true);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Exception in BasketProcessedEventHanlder: {ex.Message} with correlation Token: {correlationToken}");
            }

            await Task.CompletedTask;
        }
    }
}