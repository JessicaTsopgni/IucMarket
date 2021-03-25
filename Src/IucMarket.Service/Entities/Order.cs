﻿using System;
using System.Collections.Generic;
using IucMarket.Common;

namespace IucMarket.Service.Entities
{
    internal class Order
    {
        public  StateOptions State { get; set; }
        public string StateReason { get; set; }
        public IEnumerable<OrderDetail> Details { get; set; }
        public User Customer { get; set; }
        public DateTime CreatedAt { get; set; }
        public DeliveryPlaceOptions DeliveryPlace { get; set; }
        public DateTime? DeliveryPredicateAt { get; set; }
        public DateTime? DeliveryAt { get; set; }
        public Order()
        {

        }

        public Order(StateOptions state, string stateReason, IEnumerable<OrderDetail> details, User customer, DateTime createdAt, 
            DeliveryPlaceOptions deliveryPlace, DateTime? deliveryPredicateAt, DateTime? deliveryAt)
        {
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
