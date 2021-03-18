using IucMarket.Mobile.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
namespace IucMarket.Mobile.Models
{
    public class ProductModel : BaseModel
    {

        private string reference;
        public string Reference
        {
            get => reference;
            set => SetProperty(ref reference, value);
        }


        private string name;
        public string Name
        {
            get => name;
            set
            {
                SetProperty(ref name, value);
            }
        }


        private string description;
        public string Description
        {
            get => description;
            set
            {
                SetProperty(ref description, value);
            }
        }


        private CategoryModel category;
        public CategoryModel Category
        {
            get => category;
            set
            {
                SetProperty(ref category, value);
            }
        }

        private double price;
        public double Price
        {
            get => price;
            set
            {
                SetProperty(ref price, value);
                OnPropertyChanged(nameof(PriceWithCurrency));
            }
        }

        private string currency;
        public string Currency
        {
            get => currency;
            set
            {
                SetProperty(ref currency, value);
                OnPropertyChanged(nameof(PriceWithCurrency));
            }
        }

        public string PriceWithCurrency => $"{Price.ToString("N:0")} {Currency}";

        public double StarsMax => 5;

        private double starsCount;
        public double StarsCount
        {
            get => starsCount;
            set
            {
                SetProperty(ref starsCount, value);
                OnPropertyChanged(nameof(StarsText));
            }
        }
        public string StarsText => $"{Math.Round(StarsCount / VotesCount, 1)}/{StarsMax}";

        private double abusesCount;
        public double AbusesCount
        {
            get => abusesCount;
            set
            {
                SetProperty(ref abusesCount, value);
                OnPropertyChanged(nameof(AbusesText));
            }
        }

        public string AbusesText => AbusesCount.ToKorM();

        private double votesCount;
        public double VotesCount
        {
            get => votesCount;
            set
            {
                SetProperty(ref votesCount, value);
                OnPropertyChanged(nameof(VotesText));
            }
        }
        public string VotesText => $"{VotesCount.ToKorM()} {(VotesCount > 1 ? "Votes" : "Vote")}";

        private bool isAvailable;
        public bool IsAvailable
        {
            get => isAvailable;
            set
            {
                SetProperty(ref isAvailable, value);
                OnPropertyChanged(nameof(AvailableText));
                OnPropertyChanged(nameof(AvailableColor));
            }
        }

        public string AvailableText => $"{(IsAvailable ? "Available" : "Busy")}";
        public Color AvailableColor => IsAvailable ? Color.LawnGreen : Color.IndianRed;


        private ObservableCollection<string> pictures;
        public ObservableCollection<string> Pictures
        {
            get => pictures;
            set
            {
                SetProperty(ref pictures, value);
                OnPropertyChanged(nameof(Picture));
                OnPropertyChanged(nameof(PicturesCount));
                OnPropertyChanged(nameof(ShowImageMultipleIcon));
            }
        }
        public string Picture => Pictures.FirstOrDefault();
        public int PicturesCount => Pictures.Count();
        public bool ShowImageMultipleIcon => PicturesCount > 1;

        private UserModel owner;
        public UserModel Owner
        {
            get => owner;
            set => SetProperty(ref owner, value);
        }

        private DateTime createdDate;
        public DateTime CreatedDate
        {
            get => createdDate;
            set
            {
                SetProperty(ref createdDate, value);
                OnPropertyChanged(nameof(CreatedDateText));
            }
        }
        public string CreatedDateText { get => CreatedDate.ToRelativeDate(); }

        public ProductModel()
        {

        }

        public ProductModel(string id, string reference, string name, string description, CategoryModel category,
            double price, string currency,  double starsCount, double votesCount,
            double abusesCount, bool isAvailable, IEnumerable<string> pictures, 
            UserModel owner, DateTime createdDate)
            : base(id)
        {
            Reference = reference?.ToUpper();
            Name = name?.ToFirstCharToUpper();
            Description = description;
            Category = category;
            Price = price;
            Currency = currency;
            StarsCount = starsCount;
            AbusesCount = abusesCount;
            VotesCount = votesCount;
            IsAvailable = isAvailable;
            Pictures = new ObservableCollection<string>(pictures);
            Owner = owner;
            CreatedDate = createdDate;
        }
    }

    public class FileInfoModel : BaseModel
    {
        private string path;
        public string Path
        {
            get => path;
            set => SetProperty(ref path, value);
        }

        private string name;
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }


        private string contentType;
        public string ContentType
        {
            get => contentType;
            set => SetProperty(ref contentType, value);
        }

        public FileInfoModel()
        {

        }

        public FileInfoModel(string path, string name, string contentType)
        {
            Path = path;
            Name = name;
            ContentType = contentType;
        }
    }
}