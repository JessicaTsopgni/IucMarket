using ExpressiveAnnotations.Attributes;
using IucMarket.Common;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IucMarket.Web.Models
{
    public class OrderCreateModel
    {
        public string Id { get; set; }
        public string Number { get; set; }
        public UserListModel Customer { get; set; }
        public string CreatedDate { get; set; }
        public IEnumerable<OrderDetailModel> Details { get; set; }
        [Required]
        public string DeliveryPlaceId { get; set; }
        public List<EnumModel> Places { get; set; }
        public DateTime? DeliveryPredicateAt { get; set; }
        public DateTime? DeliveryAt { get; set; }

        [Required]
        public string StateId { get; set; }
        public List<EnumModel> States { get; set; }
        public string Comment { get; set; }

        public double Total => Details?.Sum(x => x.Amount) ?? 0;
        public string TotalWithCurrency => $"{Total.ToString("N0")} {Details?.FirstOrDefault()?.Product.Currency}";
        public OrderCreateModel()
        {
            Places = new List<EnumModel>();
            States = new List<EnumModel>();
        }

        public OrderCreateModel(string id, string number, UserListModel customer, 
            DateTime createdDate, IEnumerable<OrderDetailModel> details, 
            DeliveryPlaceOptions deliveryPlace, DateTime? deliveryPredicateAt, 
            DateTime? deliveryAt, StateOptions state, string comment):this()
        {
            Id = id;
            Number = number;
            Customer = customer;
            CreatedDate = createdDate.ToRelativeDate();
            Details = details;
            DeliveryPlaceId = ((int)deliveryPlace).ToString();
            DeliveryPredicateAt = deliveryPredicateAt;
            DeliveryAt = deliveryAt;
            StateId = ((int)state).ToString();
            Comment = comment;
            foreach (DeliveryPlaceOptions options in Enum.GetValues(typeof(DeliveryPlaceOptions)))
            {
                Places.Add
                (
                    new EnumModel
                    (
                        ((int)options).ToString(), options.ToString().Replace("_", " ")
                    )
                );
            }
            foreach (StateOptions options in Enum.GetValues(typeof(StateOptions)))
            {
                States.Add
                (
                    new EnumModel
                    (
                        ((int)options).ToString(), options.ToString().Replace("_", " ")
                    )
                );
            }
        }
    }

    public class OrderDetailModel
    {
        public ProductListModel Product { get; set; }
        public string Quantity { get; set; }
        public double Amount { get; set; }
        public string AmountText => Amount.ToString("N0");
        public string AmountWithCurrency => $"{AmountText} {Product.Currency}";
        public string QuantityAmountWithCurrency => $"{Quantity} x {Product.Price.ToString("N0")} = {AmountWithCurrency}";

        public OrderDetailModel()
        {

        }

        public OrderDetailModel(ProductListModel product, int quantity)
        {
            Product = product;
            Quantity = quantity.ToString("N0");
            Amount = product.Price  * quantity;
        }
    }

    public class EnumModel
    {
        public string Id { get; set; }
        public string Name{ get; set; }

        public EnumModel()
        {

        }

        public EnumModel(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }

}
