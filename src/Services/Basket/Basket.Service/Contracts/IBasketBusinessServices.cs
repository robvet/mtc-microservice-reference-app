using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Basket.Service.Domain;
using Basket.Service.Domain.Entities;
using Basket.Service.Dtos;

namespace Basket.Service.Contracts
{
    public interface IBasketBusinessServices
    {
        Task<BasketEntity> GetBasketById(Guid basketId, string correlationToken);
        Task<List<BasketEntity>> GetAllBaskets(string correlationToken);
        Task<List<ProductEntity>> GetAllProducts(string correlationToken);
        Task<BasketEntity> AddItemToBasket(Guid basketId, Guid ProductId, string correlationToken);
        Task<BasketItemRemovedEntity> RemoveItemFromBasket(Guid basketId, Guid productId, string correlationToken);
        Task EmptyBasket(Guid basketId, string correlationToken, bool hasOrderBeenCreated);
        Task<CheckoutEntity> Checkout(CheckoutDto checkout, string correlationToken);
        Task ProductChanged(ProductDto productEntity, string correlationId);
    }
}