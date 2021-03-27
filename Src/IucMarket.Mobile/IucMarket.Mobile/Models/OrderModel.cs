using IucMarket.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;

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
                OnPropertyChanged(nameof(Title));
                OnPropertyChanged(nameof(StateText));
                OnPropertyChanged(nameof(StateColor));
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

        public string DeliveryPlaceText => DeliveryPlace.ToString().Replace("_", " ");

        private DateTime? deliveryPredicatedDate;
        public DateTime? DeliveryPredicatedDate
        {
            get => deliveryPredicatedDate;
            set
            {
                SetProperty(ref deliveryPredicatedDate, value);
            }
        }

        private string customerId;
        public string CustomerId
        {
            get => customerId;
            set
            {
                SetProperty(ref customerId, value);
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
        private DateTime createdAt;
        public DateTime CreatedAt
        {
            get => createdAt;
            set
            {
                SetProperty(ref createdAt, value);
            }
        }
        public string CreatedAtText => CreatedAt.ToRelativeDate();


        private StateOptions state;
        public StateOptions State
        {
            get => state;
            set
            {
                SetProperty(ref state, value);
                OnPropertyChanged(nameof(Title));
                OnPropertyChanged(nameof(StateText));
                OnPropertyChanged(nameof(StateColor));
            }
        }
        public string Title => $"#{Number} - {CreatedAtText} - {StateText}";

        public string StateText => State.ToString().Replace("_", "");
        public Color StateColor => State == StateOptions.Delivered
                                   ? Color.SkyBlue
                                   : State == StateOptions.Rejected
                                   ? Color.Red
                                   : State == StateOptions.Validated
                                   ? Color.Green
                                   : Color.Gray;

        private string comment;
        public string Comment
        {
            get => comment;
            set
            {
                SetProperty(ref comment, value);
            }
        }
        public OrderModel():base()
        {
            products = new ObservableCollection<ProductModel>();
            products.CollectionChanged += Products_CollectionChanged;
            Number = "(Auto)";
            DeliveryPlace = DeliveryPlaceOptions.Campus_Logbessou;
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
            DateTime createdAt, DateTime? deliveryPredicatedDate, DateTime? customerReceivedDate,
            string customerId, StateOptions state, string comment)
            :this(orderDetails, deliveryPlace, deliveryPredicatedDate)
        {
            Id = id;
            Number = number;
            CustomerId = customerId;
            CustomerReceivedDate = customerReceivedDate;
            State = state;
            Comment = comment;
            CreatedAt = createdAt;
        }

        public void Add(ProductModel product)
        {
            var p = Products?.FirstOrDefault(x => x.Id == product.Id);
            if (p == null)
            {
                product.OrderQuantity = 1;
                Products.Add(product);
            }
            else
            {
                product.OrderQuantity += 1;
                p.OrderQuantity += 1;
            }

            RiseOnTotalPropertyChanged();
        }


        public void Remove(ProductModel product)
        {
            var p = Products?.FirstOrDefault(x => x.Id == product.Id);
            if (p != null)
            {
                //product.OrderQuantity -= p.OrderQuantity;
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
            DeliveryPlace = DeliveryPlaceOptions.Campus_Logbessou;
            Products?.Clear();
            DeliveryPredicatedDate =DateTime.UtcNow.AddHours(1);
            Number = "(Auto)";
            CustomerReceivedDate = null;
            OnPropertyChanged(nameof(Total));
        }
    }
}
