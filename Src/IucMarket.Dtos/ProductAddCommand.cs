using System;
using System.Collections.Generic;
using System.Text;

namespace IucMarket.Dtos
{
    public class ProductAddCommand
    {
        public string Reference { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string Currency { get; set; }
        public string CategoryId { get; set; }
        public IDictionary<string, string> Pictures { get; set; }
        public string OwnerId { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Status { get; set; }
        public ProductAddCommand()
        {
            CreatedAt = DateTime.UtcNow;
        }

        public ProductAddCommand(string reference, string name, string description, double price, string currency,
            IDictionary<string, string> pictures, string categoryId, string ownerId, bool status)
            : this()
        {
            Reference = reference;
            Name = name;
            Description = description;
            Price = price;
            Currency = currency;
            Pictures = pictures;
            CategoryId = categoryId;
            OwnerId = ownerId;
            Status = status;
        }
    }

}
