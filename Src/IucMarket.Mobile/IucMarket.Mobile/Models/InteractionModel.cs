using IucMarket.Common;
using System;

namespace IucMarket.Mobile.Models
{
    public class InteractionModel
    {
        public string UserId { get; set; }
        public string ProductId { get; set; }
        public InteractionOptions InteractionType { get; set; }
        public int Count { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

        public InteractionModel()
        {

        }

        public InteractionModel(string userId, string productId,
            InteractionOptions interactionType, int count, string content, DateTime createdAt)
            : this()
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