using IucMarket.Common;
using System;

namespace IucMarket.Web.Models
{
    public class OrderListModel
    {
        public string Id { get; set; }
        public string Number { get; set; }
        public string Customer { get; set; }
        public string ProductsCount { get; set; }
        public string Amount { get; set; }
        public string Since { get; set; }
        public string Status { get; set; }
        public OrderListModel()
        {

        }

        public OrderListModel(string id, string number, string customer, int productsCount, 
          double amount,  DateTime createdAt, StateOptions status)
        {
            Id = id;
            Number = number;
            Customer = customer;
            ProductsCount = productsCount.ToString("N0");
            Amount = amount.ToString("N0");
            Since = createdAt.ToRelativeDate();
            Status = status.ToString().Replace("_", " ");
        }
    }
}
