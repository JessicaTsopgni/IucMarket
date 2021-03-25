using System;
using System.Collections.Generic;
using IucMarket.Common;

namespace IucMarket.Service.Entities
{
    internal class Order
    {
        public string Number { get; set; }
        public IEnumerable<OrderDetail> Details { get; set; }
        public User Customer { get; set; }
        public DateTime CreatedAt { get; set; }
        public DeliveryPlaceOptions DeliveryPlace { get; set; }
        public DateTime? DeliveryPredicateAt { get; set; }
        public DateTime? DeliveryAt { get; set; }
        public  StateOptions State { get; set; }
        public string StateReason { get; set; }

        public Order()
        {

        }

        public Order(string number,IEnumerable<OrderDetail> details, User customer, 
            DeliveryPlaceOptions deliveryPlace,  DateTime createdAt,
            StateOptions state, string stateReason,  DateTime? deliveryPredicateAt, DateTime? deliveryAt)
        {
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

    internal class OrderDetail
    {
        public string ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public OrderDetail()
        {

        }

        public OrderDetail(string productId, Product product, int quantity)
        {
            ProductId = productId;
            Product = product;
            Quantity = quantity;
        }
    }

}
