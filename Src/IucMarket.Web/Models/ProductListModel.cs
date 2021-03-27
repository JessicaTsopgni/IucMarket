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
        public IEnumerable<FileInfoModel> Pictures { get; set; }
        public DateTime CreatedDate { get; set; }
        public CategoryListModel Category { get; set; }
        public string Status { get; set; }
        public ProductListModel()
        {

        }

        public ProductListModel(string id, string reference,string name, string description, 
            double price, string currency, IEnumerable<FileInfoModel> 
            pictures, DateTime createdDate, CategoryListModel category, bool status)
        {
            Id = id;
            Reference = reference;
            Name = name;
            Description = description;
            Price = price;
            Currency = currency;
            Pictures = pictures;
            CreatedDate = createdDate;
            Category = category;
            Status = status ? "Enabled": "Disabled";
        }
    }
}
