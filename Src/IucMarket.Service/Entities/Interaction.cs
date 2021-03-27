using IucMarket.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace IucMarket.Service.Entities
{
    public class Interaction
    {
        public string UserId { get; set; }
        public string ProductId { get; set; }
        public InteractionOptions InteractionType { get; set; }
        public int Count { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

        public Interaction()
        {

        }

        public Interaction(string userId, string productId, 
            InteractionOptions interactionType, int count, string content,  DateTime createdAt)
        {
            UserId = userId;
            ProductId = productId;
            CreatedAt = createdAt;
            Count = count;
            Content = content;
            InteractionType = interactionType;
        }
    }
}
