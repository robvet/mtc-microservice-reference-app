using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using basket.service.Dtos;
using Basket.Service.Domain.Entities;

namespace Basket.Service.Dtos
{
    /// <summary>
    /// Map Entity Objects to Dto Objects
    /// </summary>
    public class Mapper
    {
        public static BasketDto MapToBasketDto(Domain.Entities.Basket basketEntityEntity)
        {
            // Transform Single BasketEntity to BasketDto
            var basketDto = new BasketDto
            {
                BasketId = basketEntityEntity.BasketId,
                ItemCount = basketEntityEntity.ItemCount,
                BuyerID = basketEntityEntity.BuyerId,
                Processed = basketEntityEntity.Processed
            };

            foreach (var item in basketEntityEntity.Items)
            {
                basketDto.CartItems.Add(new BasketItemDto
                {
                    BasketParentId = item.BasketParentId,
                    ProductId = item.ProductId,
                    Title = item.Title,
                    Artist = item.Artist,
                    Genre = item.Genre,
                    QuanityOrdered = item.Quantity,
                    //Price = decimal.Parse(item.UnitPrice) * item.Quantity,
                    Price = TransformUnitPrice(item.UnitPrice, item.Quantity),
                    Condition = item.Condition,
                    Status = item.Status,
                    Medium = item.Medium,
                    DateCreated = item.DateCreated
                }
            );

                basketDto.CartTotal = basketDto.CartItems.Sum(x => x.Price);
            }

            return basketDto;
        }

        public static List<BasketDto> MapToBasketDto(IEnumerable<Domain.Entities.Basket> baskets)
        {
            var basketDtos = new List<BasketDto>();

            // Transform Collection of BasketEntity items to BasketDto
            foreach (var basket in baskets)
            {
                if (basket.BasketId != Guid.Empty)
                {  
                    var basketDto = new BasketDto
                    {
                        BasketId = basket.BasketId,
                        ItemCount = basket.ItemCount,
                        BuyerID = basket.BuyerId,
                        Processed = basket.Processed
                    };

                    foreach (var item in basket.Items)
                    {
                        basketDto.CartItems.Add(new BasketItemDto
                        {
                            BasketParentId = item.BasketParentId,
                            ProductId = item.ProductId,
                            Title = item.Title,
                            Artist = item.Artist,
                            Genre = item.Genre,
                            QuanityOrdered = item.Quantity,
                            Price = TransformUnitPrice(item.UnitPrice, item.Quantity),
                            Condition = item.Condition,
                            Status = item.Status,
                            Medium = item.Medium,
                            DateCreated = item.DateCreated
                        });
                    }

                    basketDto.CartTotal = basketDto.CartItems.Sum(x => x.Price * x.QuanityOrdered);
                    basketDtos.Add(basketDto);
                }
            }

            return basketDtos;
        }

        public static List<ProductDto> MapToProductDto(List<ProductReadModel> productEntity)
        {
            var productDtos = new List<ProductDto>();

            foreach (var item in productEntity)
            {
                // Fitler out records with no ProductId
                if (item.ProductId != Guid.Empty)
                {
                    productDtos.Add(new ProductDto
                    {
                        ProductId = item.ProductId,
                        Title = item.Title,
                        Artist = item.Artist,
                        Genre = item.Genre,
                        Price = item.Price,
                        Condition = item.Condition,
                        Status = item.Status,
                        Medium = item.Medium,
                        DateCreated = item.DateCreated
                    });
                }
            }

            return productDtos;
        }


        //public static BasketSummaryDto MapToBasketSummaryDto(BasketEntity genericEntities)
        //{
        //    // Transform BasketEntity to BasketDto
        //    var basketSummaryDto = new BasketSummaryDto
        //    {
        //        BasketId = genericEntities.BasketId,
        //        ItemCount = genericEntities.Items.Count,
        //        Processed = genericEntities.Processed,
        //        //basketSummaryDto.ProductNames = genericEntities.Items.SelectMany(x => x.Title).ToString();
        //        ProductNames = string.Join("\n", genericEntities.Items.Select(c => c.Title).Distinct())
        //    };

        //    return basketSummaryDto;
        //}

        public static BasketSummaryDto MapToBasketSummaryDto(Generic genericEntity)
        {
            // Transform BasketEntity to BasketDto
            var basketSummaryDto = new BasketSummaryDto
            {
                BasketId = genericEntity.BasketId,
                ItemCount = genericEntity.ItemCount,
                Processed = genericEntity.Processed
                //basketSummaryDto.ProductNames = genericEntities.Items.SelectMany(x => x.Title).ToString();
                //ProductNames = string.Join("\n", genericEntities.Items.Select(c => c.Title).Distinct())
            };

            return basketSummaryDto;
        }

        public static GenericDto MapToGenericEntitySummaryDto(List<Generic> genericEntities)
        {
            var genericEntityDto = new GenericDto();
                       
            {
                
                foreach(Generic genericEntity in genericEntities) 
                {
                    if (genericEntity.BasketId == Guid.Empty)
                    {
                        genericEntityDto.Products.Add(new GenericSummaryDto
                        {
                            ProductId = genericEntity.ProductId,
                            Title = genericEntity.Title,
                            Artist = genericEntity.Artist,

                            BasketId = genericEntity.BasketId,
                            Processed = genericEntity.Processed,
                            ItemCount = genericEntity.ItemCount,
                        });
                    }
                    else
                    {
                        genericEntityDto.Baskets.Add(new GenericSummaryDto
                        {
                            ProductId = genericEntity.ProductId,
                            Title = genericEntity.Title,
                            Artist = genericEntity.Artist,

                            BasketId = genericEntity.BasketId,
                            Processed = genericEntity.Processed,
                            ItemCount = genericEntity.ItemCount,
                        });
                    }
                };
            };

            return genericEntityDto;
        }

        private static decimal TransformUnitPrice(string priceAsString, int quantity)
        {
            var success = decimal.TryParse(priceAsString, out var price);
            return success == true ? price * quantity : 0;
        }
    }
}