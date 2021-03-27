using IucMarket.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace IucMarket.Dtos
{
    public class OrderEditCommand
    {
        public DeliveryPlaceOptions DeliveryPlace { get; set; }
        public DateTime? DeliveryPredicateAt { get; set; }
        public DateTime? DeliveryAt { get; set; }
        public StateOptions State { get; set; }
        public string Comment { get; set; }
        public OrderEditCommand()
        {
            
        }

        public OrderEditCommand(DeliveryPlaceOptions deliveryPlace, DateTime? deliveryPredicateAt, 
            DateTime? deliveryAt, StateOptions state, string comment)
        {
            DeliveryPlace = deliveryPlace;
            DeliveryPredicateAt = deliveryPredicateAt;
            DeliveryAt = deliveryAt;
            State = state;
            Comment = comment;
        }
    }

}
