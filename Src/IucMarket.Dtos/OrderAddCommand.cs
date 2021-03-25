using IucMarket.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace IucMarket.Dtos
{
    public class OrderAddCommand
    {
        DeliveryPlaceOptions DeliveryPlace { get; set; }
        public IDictionary<string, int> Carts { get; set; }
        public string OwnerId { get; set; }
        public DateTime CreatedAt { get; set; }
        public OrderAddCommand()
        {
            CreatedAt = DateTime.UtcNow;
        }

        public OrderAddCommand(DeliveryPlaceOptions deliveryPlace, IDictionary<string, int> carts, string ownerId)
            :this()
        {
            DeliveryPlace = deliveryPlace;
            Carts = carts;
            OwnerId = ownerId;
        }
    }

}
