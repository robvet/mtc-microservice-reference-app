using System.Collections.Generic;
using System.Linq;
using Basket.Service.Domain.Entities;

namespace Basket.Service.Dtos
{
    public class Mapper
    {
        public static BasketDto MapToBasketDto(BasketEntity basketEntityEntity)
        {
            // Transform BasketEntity to BasketDto
            var basketDto = new BasketDto();

            basketDto.BasketId = basketEntityEntity.BasketId;
            basketDto.ItemCount = basketEntityEntity.Count;

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

        public static List<BasketDto> MapToBasketDto(IEnumerable<BasketEntity> baskets)
        {
            var basketDtos = new List<BasketDto>();

            foreach (var basket in baskets)
            {
                var basketDto = new BasketDto
                {
                    BasketId = basket.BasketId,
                    ItemCount = basket.Count,

                    
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
                        //Price = decimal.Parse(item.UnitPrice) * item.Quantity,
                        Price = TransformUnitPrice(item.UnitPrice, item.Quantity),
                        Condition = item.Condition,
                        Status = item.Status,
                        Medium = item.Medium,
                        DateCreated = item.DateCreated
                    }
                );
                    
                    basketDto.CartTotal = basketDto.CartItems.Sum(x => x.Price * x.QuanityOrdered);
                }

                basketDtos.Add(basketDto);
            }

            return basketDtos;
        }

        public static BasketSummaryDto MapToBasketSummaryDto(BasketEntity basketEntityEntity)
        {
            // Transform BasketEntity to BasketDto
            var basketSummaryDto = new BasketSummaryDto();

            basketSummaryDto.BasketId = basketEntityEntity.BasketId;
            basketSummaryDto.ItemCount = basketEntityEntity.Items.Count;
            //basketSummaryDto.ProductNames = basketEntityEntity.Items.SelectMany(x => x.Title).ToString();
            basketSummaryDto.ProductNames = string.Join("\n", basketEntityEntity.Items.Select(c => c.Title).Distinct());

            return basketSummaryDto;
        }

        private static decimal TransformUnitPrice(string priceAsString, int quantity)
        {
            var success = decimal.TryParse(priceAsString, out var price);
            return success == true ? price * quantity : 0;
        }
    }
}