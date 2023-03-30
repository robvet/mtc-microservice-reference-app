using System.Collections.Generic;

namespace Ordering.API.Dtos
{
    public class RestMapper
    {
        public static OrderDto MapToOrderDto(dynamic order)
        {
            var orderDto = new OrderDto
            {
                OrderId = order.Id,
                OrderDate = order.OrderDate,
                //ShoppingBasketId = order.BasketId,

                // Must add ToString() in order to parse the decimal, otherwise it errors
                Total = decimal.Parse(order.Total?.ToString()),
                Username = $"{order.Buyer.FirstName} {order.Buyer.LastName}"
            };

            foreach (var item in order.OrderDetails)
                orderDto.OrderDetails.Add(new OrderDetailDto
                {
                    //OrderDetailId = item.OrderDetailId,
                    AlbumId = item.AlbumId,
                    OrderId = order.Id ?? "Not Available",
                    Quantity = item.Quantity,
                    UnitPrice = decimal.Parse(item.UnitPrice?.ToString()),
                    Artist = item.Artist,
                    Title = item.Title
                });

            return orderDto;
        }

        public static List<OrdersDto> MapToOrdersDto(List<dynamic> orders)
        {
            var ordersDtos = new List<OrdersDto>();

            foreach (var order in orders)
                ordersDtos.Add(new OrdersDto
                {
                    Id = order.Id,
                    //CustomerId = order.CustomerSystemId?.ToString() ?? "n/a",
                    //CheckoutId = order.CheckoutId ?? "n/a",
                    BuyerName = order.CustomerSystemId?.ToString() ?? "n/a",
                    OrderId = order.OrderSystemId ?? "n/a",
                    //ShoppingBasketId = order.BasketId ?? "n/a",
                    Total = decimal.Parse(order.Total.ToString()),
                    OrderDate = order.OrderDate
                });

            return ordersDtos;
        }
    }
}
