using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace IucMarket.Entities
{
    public class Product
    {
        [JsonIgnore]
        public string Key { get; set; }
        public string Reference { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string UserKey { get; set; }
        [JsonIgnore]
        public User Owner {get; set;}
        public DateTime CreatedAt { get; set; }
        public bool Status { get; set; }
        public string Currency { get; set; }
        public IEnumerable<string> PictureNames { get; set; }
        public Product()
        {

        }

        public Product(string key, string reference, string name, string description, 
            double price, string currency, IEnumerable<string> pictureNames, string userKey, User owner, DateTime createdAt, bool status)
        {
            Key = key;
            Reference = reference;
            Name = name;
            Description = description;
            Price = price;
            Currency = currency;
            PictureNames = pictureNames;
            UserKey = userKey;
            Owner = owner;
            CreatedAt = createdAt;
            Status = status;
        }
    }
}