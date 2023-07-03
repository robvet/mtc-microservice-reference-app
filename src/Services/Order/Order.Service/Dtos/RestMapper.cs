using order.domain.Models.ReadModels;
using System.Collections.Generic;

namespace order.service.Dtos
{
    public class RestMapper
    {
        public static OrderDto MapToOrderDto(OrderReadModel order)
        {
            var orderDto = new OrderDto
            {
                //OrderId = order.OrderId,
                id = order.Id,
                OrderId = order.OrderId,
                ShoppingBasketId = order.BasketId,
                CustomerId = order.CustomerId,
                OrderDate = order.OrderDate,
                UserName = order.Buyer.UserName,
                BuyerName = $"{order.Buyer.FirstName} {order.Buyer.LastName}",
                Email = order.Buyer.Email,

                
                // Must add ToString() in order to parse the decimal, otherwise it errors
                Total = order.Total // decimal.Parse(order.Total.ToString()),
            };

            foreach (var item in order.OrderDetail)
                orderDto.OrderDetails.Add(new OrderDetailDto
                {
                    ProductId = item.ProductId,
                    Title = item.Title,
                    ArtistId = item.ArtistId,
                    Artist = item.Artist,
                    GenreId = item.GenreId,
                    Genre = item.Genre,
                    UnitPrice = item.UnitPrice, // decimal.Parse(item.UnitPrice.ToString()),
                    Quantity = item.Quantity,
                    Condition = item.Condition,
                    Status = item.Status,
                    MediumId = item.MediumId,
                    Medium = item.Medium,
                    DateCreated = item.DateCreated,
                    BackOrdered = item.BackOrdered,
                    HighValueItem = item.HighValueItem
                });

            return orderDto;
        }

        public static List<OrdersDto> MapToOrdersDto(List<OrderReadModel> orders)
        {
            var ordersDtos = new List<OrdersDto>();

            foreach (var order in orders)
                ordersDtos.Add(new OrdersDto
                {
                    Id = order.Id,
                    OrderId = order.OrderId,
                    CustomerId = order.CustomerId,
                    ShoppingBasketId = order.BasketId,
                    UserName = order.Buyer.UserName,
                    BuyerName = $"{order.Buyer.FirstName} {order.Buyer.LastName}",
                    Total = decimal.Parse(order.Total.ToString()),
                    Email = order.Buyer.Email,
                    OrderDate = order.OrderDate
                });

            return ordersDtos;
        }
    }
}
