using System;
using System.Collections.Generic;

namespace IucMarket.Dtos
{
    public class ProductDto
    {
        public string Id { get; set; }
        public string Reference { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public UserDto Owner {get; set;}
        public DateTime CreatedAt { get; set; }
        public bool Status { get; set; }
        public string Currency { get; set; }
        public IEnumerable<FileInfoDto> Pictures { get; set; }
        public CategoryDto Category { get; set; }
        public ProductDto()
        {

        }

        public ProductDto(string key, string reference, string name, string description, 
            double price, string currency, IEnumerable<FileInfoDto> pictures, 
            CategoryDto category,UserDto owner, DateTime createdAt, bool status)
        {
            Id = key;
            Reference = reference;
            Name = name;
            Description = description;
            Price = price;
            Currency = currency?.ToUpper();
            Pictures = pictures;
            Category = category;
            Owner = owner;
            CreatedAt = createdAt;
            Status = status;
        }
    }

    public class FileInfoDto
    {
        public string Path { get; set; }
        public string Name { get; set; }
        public string ContentType { get; set; }

        public FileInfoDto()
        {

        }

        public FileInfoDto(string pathFormat, string name, string contentType)
        {
            Path = string.Format(pathFormat, name, contentType);
            Name = name;
            ContentType = contentType;
        }
    }
}