using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IucMarket.Web.Models
{
    public class ProductListModel
    {
        public string Id { get; set; }
        public string Reference { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string Currency { get; set; }
        public string PictureLink { get; set; }
        public DateTime CreatedDate { get; set; }
        public UserListModel Owner { get; set; }
        public string Status { get; set; }
        public ProductListModel()
        {

        }

        public ProductListModel(string id, string reference,string name, string description, 
            double price, string currency, string pictureLink, DateTime createdDate, UserListModel owner, bool status)
        {
            Id = id;
            Reference = reference;
            Name = name;
            Description = description;
            Price = price;
            Currency = currency;
            PictureLink = pictureLink;
            CreatedDate = createdDate;
            Owner = owner;
            Status = status ? "Enabled": "Disabled";
        }
    }
}
