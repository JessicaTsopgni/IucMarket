using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using IucMarket.Entities.Common;

namespace IucMarket.Entities
{
    public class Product
    {
        [JsonIgnore]
        public string Key { get; set; }

        [JsonProperty(nameof(Key))]
        public string KeySetter { set => Key = value; }
        public string Reference { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string UserKey { get; set; }
        [JsonIgnore]
        public User Owner {get; set;}
        [JsonProperty(nameof(Owner))]
        public User OwnerSetter { set => Owner = value; }
        public DateTime CreatedAt { get; set; }
        public bool Status { get; set; }
        public string Currency { get; set; }
        public IEnumerable<FileInfo> Pictures { get; set; }
        public Product()
        {

        }

        public Product(string key, string reference, string name, string description, 
            double price, string currency, IEnumerable<FileInfo> pictures, string userKey, User owner, DateTime createdAt, bool status)
        {
            Key = key;
            Reference = reference?.ToUpper();
            Name = name?.ToUcFirst();
            Description = description;
            Price = price;
            Currency = currency?.ToUpper();
            Pictures = pictures;
            UserKey = userKey;
            Owner = owner;
            CreatedAt = createdAt;
            Status = status;
        }
    }

    public class FileInfo
    {
        public string Name { get; set; }
        public string ContentType { get; set; }

        public FileInfo()
        {

        }

        public FileInfo(string name, string contentType)
        {
            Name = name;
            ContentType = contentType;
        }
    }
}