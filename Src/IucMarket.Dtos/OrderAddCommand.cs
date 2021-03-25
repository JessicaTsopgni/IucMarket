using IucMarket.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace IucMarket.Dtos
{
    public class OrderAddCommand
    {
        public DeliveryPlaceOptions DeliveryPlace { get; set; }
        public IDictionary<string, int> Carts { get; set; }
        public string CustomerId { get; set; }
        public DateTime CreatedAt { get; set; }
        public OrderAddCommand()
        {
            CreatedAt = DateTime.UtcNow;
        }

        public OrderAddCommand(DeliveryPlaceOptions deliveryPlace, IDictionary<string, int> carts,
            string customerId)
            :this()
        {
            DeliveryPlace = deliveryPlace;
            Carts = carts;
            CustomerId = customerId;
        }
    }

}
