using IucMarket.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace IucMarket.Mobile.Models
{
    public class OrderModel:BaseModel
    {
        private string number;
        public string Number
        {
            get => number;
            set
            {
                SetProperty(ref number, value);
            }
        }

        private ObservableCollection<ProductModel> products;
        public ObservableCollection<ProductModel> Products
        {
            get => products;
            set
            {
                SetProperty(ref products, value);
                RiseOnTotalPropertyChanged();
            }
        }

        public void RiseOnTotalPropertyChanged()
        {
            OnPropertyChanged(nameof(Total));
            OnPropertyChanged(nameof(TotalText));
            OnPropertyChanged(nameof(TotalWithCurrency));
        }

        private DeliveryPlaceOptions deliveryPlace;
        public DeliveryPlaceOptions DeliveryPlace
        {
            get => deliveryPlace;
            set
            {
                SetProperty(ref deliveryPlace, value);
            }
        }

        private DateTime? deliveryPredicatedDate;
        public DateTime? DeliveryPredicatedDate
        {
            get => deliveryPredicatedDate;
            set
            {
                SetProperty(ref deliveryPredicatedDate, value);
            }
        }

        private DateTime? customerReceivedDate;
        public DateTime? CustomerReceivedDate
        {
            get => customerReceivedDate;
            set
            {
                SetProperty(ref customerReceivedDate, value);
            }
        }
        public OrderModel():base()
        {
            products = new ObservableCollection<ProductModel>();
            products.CollectionChanged += Products_CollectionChanged;
            Number = "(Auto)";
            DeliveryPlace = DeliveryPlaceOptions.CampusA;
        }

        private void Products_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {

            RiseOnTotalPropertyChanged();
        }

        public OrderModel(ObservableCollection<ProductModel> products, 
            DeliveryPlaceOptions deliveryPlace, DateTime? deliveryPredicatedDate):this()
        {
            Products = products;
            DeliveryPlace = deliveryPlace;
            DeliveryPredicatedDate = deliveryPredicatedDate;
        }

        public OrderModel(string id, string number, ObservableCollection<ProductModel> orderDetails, DeliveryPlaceOptions deliveryPlace, 
            DateTime? deliveryPredicatedDate, DateTime? customerReceivedDate)
            :this(orderDetails, deliveryPlace, deliveryPredicatedDate)
        {
            Id = id;
            Number = number;
            CustomerReceivedDate = customerReceivedDate;
        }

        public void Add(ProductModel product)
        {
            var p = Products?.FirstOrDefault(x => x.Id == product.Id);
            if (p == null)
            {
                product.CartQuantity = 1;
                Products.Add(product);
            }
            else
            {
                product.CartQuantity += 1;
                p.CartQuantity += 1;
            }

            RiseOnTotalPropertyChanged();
        }


        public void Remove(ProductModel product)
        {
            var p = Products?.FirstOrDefault(x => x.Id == product.Id);
            if (p != null)
            {
                //product.CartQuantity -= p.CartQuantity;
                Products?.Remove(p);
            }

            RiseOnTotalPropertyChanged();
        }

        public double Total => Products?.Sum(x => x.Amount) ?? 0;
        public string TotalText => Total.ToString("N0");
        public string TotalWithCurrency => $"TOTAL : {TotalText} {Products.FirstOrDefault()?.Currency}";
        public void Clear()
        {
            Id = null;
            DeliveryPlace = DeliveryPlaceOptions.CampusA;
            Products?.Clear();
            DeliveryPredicatedDate = DateTime.Now;
            Number = "(Auto)";
            CustomerReceivedDate = null;
            OnPropertyChanged(nameof(Total));
        }
    }
}
