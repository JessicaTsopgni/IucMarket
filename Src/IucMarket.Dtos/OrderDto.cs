using System;
using System.Collections.Generic;
using System.Linq;
using IucMarket.Common;
namespace IucMarket.Dtos
{
    public class OrderDto
    {
        public string Id { get; set; }
        public string Number { get; set; }
        public StateOptions State { get; set; }
        public string StateReason { get; set; }
        public IEnumerable<OrderDetailDto> Details { get; set; }
        public UserDto Customer { get; set; }
        public DateTime CreatedAt { get; set; }
        public DeliveryPlaceOptions DeliveryPlace { get; set; }
        public DateTime? DeliveryPredicateAt { get; set; }
        public DateTime? DeliveryAt { get; set; }
        public double Total => Details.Sum(x => x.Amount);
        public OrderDto()
        {

        }

        public OrderDto(string id, string number, StateOptions state, string stateReason, IEnumerable<OrderDetailDto> details, UserDto customer, DateTime createdAt, DeliveryPlaceOptions deliveryPlace, 
            DateTime? deliveryPredicateAt, DateTime? deliveryAt)
        {
            Id = id;
            Number = number;
            State = state;
            StateReason = stateReason;
            Details = details;
            Customer = customer;
            CreatedAt = createdAt;
            DeliveryPlace = deliveryPlace;
            DeliveryPredicateAt = deliveryPredicateAt;
            DeliveryAt = deliveryAt;
        }
    }

    public class OrderDetailDto
    {
        public ProductDto Product { get; set; }
        public int Quantity { get; set; }
        public double Amount => Product.Price * Quantity;

        public OrderDetailDto()
        {

        }

        public OrderDetailDto(ProductDto product, int quantity)
        {
            Product = product;
            Quantity = quantity;
        }
    }


}