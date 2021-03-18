using System;
using System.Collections.Generic;

namespace IucMarket.Entities
{
    public class Order
    {
        public IEnumerable<OrderDetail> Details { get; set; }
        public User Customer { get; set; }

        public Order()
        {

        }

        public Order(IEnumerable<OrderDetail> details, User customer)
        {
            Details = details;
            Customer = customer;
        }
    }

    public class OrderDetail
    {
        public enum StateOptions
        {
            Rejected = -1,
            Inprocess,
            Validated,
            Delivered
        }
        public Product Product { get; set; }
        public float Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public  StateOptions State { get; set; }
        public DateTime DeliveryPredicateAt { get; set; }
        public DateTime? DeliveryAt { get; set; }

        public OrderDetail()
        {

        }

        public OrderDetail(Product product, float quantity, DateTime createdAt, 
            StateOptions state, DateTime deliveryPredicateAt, DateTime? deliveryAt)
        {
            Product = product;
            Quantity = quantity;
            CreatedAt = createdAt;
            State = state;
            DeliveryPredicateAt = deliveryPredicateAt;
            DeliveryAt = deliveryAt;
        }
    }
}
