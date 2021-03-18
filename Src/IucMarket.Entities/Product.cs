using System;
using System.Collections.Generic;
using IucMarket.Entities.Common;

namespace IucMarket.Entities
{
    public class Product
    {
        public string Reference { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Status { get; set; }
        public string Currency { get; set; }
        public IEnumerable<FileInfo> Pictures { get; set; }
        public string ProductTypeId { get; set; }
        public Product()
        {

        }

        public Product(string reference, string name, string description, 
            double price, string currency, IEnumerable<FileInfo> pictures, string productTypeId, 
            string userId,  DateTime createdAt, bool status)
        {
            Reference = reference?.ToUpper();
            Name = name?.ToUcFirst();
            Description = description;
            Price = price;
            Currency = currency?.ToUpper();
            Pictures = pictures;
            ProductTypeId = productTypeId;
            UserId = userId;
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