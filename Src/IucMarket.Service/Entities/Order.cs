using System;
using System.Collections.Generic;

namespace IucMarket.Service.Entities
{
    internal class Order
    {
        public enum StateOptions
        {
            Rejected = -1,
            Inprocess,
            Validated,
            Delivered
        }
        public  StateOptions State { get; set; }
        public string StateReason { get; set; }
        public IEnumerable<OrderDetail> Details { get; set; }
        public User Customer { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeliveryPredicateAt { get; set; }
        public DateTime? DeliveryAt { get; set; }
        public Order()
        {

        }

        public Order(IEnumerable<OrderDetail> details, User customer, DateTime createdAt, 
            StateOptions state, string stateReason, DateTime? deliveryPredicateAt, DateTime? deliveryAt)
        {
            Details = details;
            Customer = customer;
            CreatedAt = createdAt;
            State = state;
            StateReason = stateReason;
            DeliveryPredicateAt = deliveryPredicateAt;
            DeliveryAt = deliveryAt;
        }
    }

    internal class OrderDetail
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }

        public OrderDetail()
        {

        }

        public OrderDetail(Product product, int quantity)
        {
            Product = product;
            Quantity = quantity;
        }
    }
}
