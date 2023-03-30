using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Basket.API.Domain;
using Basket.API.Domain.Entities;
using Basket.API.Dtos;

namespace Basket.API.Contracts
{
    public interface IBasketBusinessServices
    {
        Task<Domain.Entities.BasketEntity> GetBasketById(string basketId, string correlationToken);
        Task<List<Domain.Entities.BasketEntity>> GetAllBaskets(string correlationToken);
        Task<Domain.Entities.BasketEntity> AddItemToBasket(int productId, string correlationToken, string basketId);
        Task<BasketItemRemovedEntity> RemoveItemFromBasket(string basketId, int productId, string correlationToken);
        Task EmptyBasket(string basketId, string correlationToken, bool hasOrderBeenCreated);
        Task<CheckoutEntity> Checkout(CheckoutDto checkout, string correlationToken);
        Task ProductChanged(ProductEntity productEntity, string correlationId);
    }
}