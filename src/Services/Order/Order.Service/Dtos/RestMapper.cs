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
                OrderDate = order.OrderDate,
                ShoppingBasketId = order.BasketId,
                CustomerId = order.CustomerId,
                BuyerName = order.Buyer.UserName,
                OrderId = order.OrderId,
                // Must add ToString() in order to parse the decimal, otherwise it errors
                Total = order.Total, // decimal.Parse(order.Total.ToString()),
                Username = order.Buyer.UserName
            };

            foreach (var item in order.OrderDetails)
                orderDto.OrderDetails.Add(new OrderDetailDto
                {
                    AlbumId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice, // decimal.Parse(item.UnitPrice.ToString()),
                    Artist = item.Artist,
                    Title = item.Title
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
                    CustomerId = order.CustomerId,
                    ShoppingBasketId = order.BasketId,
                    BuyerName = order.Buyer.UserName,
                    OrderId = order.OrderId,
                    Total = decimal.Parse(order.Total.ToString()),
                    OrderDate = order.OrderDate
                });

            return ordersDtos;
        }
    }
}
