using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using basket.service.Dtos;
using Basket.Service.Domain;
using Basket.Service.Domain.Entities;
using Basket.Service.Dtos;

namespace Basket.Service.Contracts
{
    public interface IBasketBusinessServices
    {
        Task<Domain.Entities.Basket> GetBasketById(Guid basketId, string correlationToken);
        Task<List<Domain.Entities.Basket>> GetAllBaskets(string correlationToken);
        Task<List<ProductReadModel>> GetAllProducts(string correlationToken);
        Task<Domain.Entities.Basket> AddItemToBasket(Guid basketId, Guid ProductId, string correlationToken);
        Task<BasketItemRemove> RemoveItemFromBasket(Guid basketId, Guid productId, string correlationToken);
        Task<bool> MarkBasketProcessed(Guid basketId, string correlationToken, bool hasOrderBeenCreated);
        Task<Checkout> Checkout(CheckoutDto checkout, string correlationToken);
        Task ProductChanged(ProductReadModel productEntity, string correlationId);
        Task DeleteBasket(Guid basketId, string correlationToken);
    }
}