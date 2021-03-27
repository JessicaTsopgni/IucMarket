using IucMarket.Common;
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
                RiseOrderQuantityPropertyChanged();
            }
        }

        private string currency;
        public string Currency
        {
            get => currency;
            set
            {
                SetProperty(ref currency, value);
                RiseOrderQuantityPropertyChanged();
            }
        }

        public string PriceWithCurrency => $"{Price.ToString("N0")} {Currency}";
        public string PriceText => $"{Price.ToString("N0")}";

        public static int StarsMax => 5;

        private double starsCount;

        public IEnumerable<int> Stars => Enumerable.Range(0, StarNumber).ToArray();

        public double StarsCount
        {
            get => starsCount;
            set
            {
                SetProperty(ref starsCount, value);
                OnPropertyChanged(nameof(StarsText));
            }
        }
        public int StarNumber { get; }
        public string StarsText => $"{StarNumber}/{StarsMax}";

        private double likesCount;
        public double LikesCount
        {
            get => likesCount;
            set
            {
                SetProperty(ref likesCount, value);
                OnPropertyChanged(nameof(LikesText));
            }
        }
        public string LikesText => LikesCount > 0 ? LikesCount.ToKorM() : "Like";

        private double commentsCount;
        public double CommentsCount
        {
            get => commentsCount;
            set
            {
                SetProperty(ref commentsCount, value);
                OnPropertyChanged(nameof(CommentsText));
            }
        }
        public string CommentsText => CommentsCount > 0 ? CommentsCount.ToKorM() : "Review";

        private int orderQuantity;
        public int OrderQuantity
        {
            get => orderQuantity;
            set
            {
                SetProperty(ref orderQuantity, value);
                RiseOrderQuantityPropertyChanged();
            }
        }

        private void RiseOrderQuantityPropertyChanged()
        {
            OnPropertyChanged(nameof(CartsText));
            OnPropertyChanged(nameof(Amount));
            OnPropertyChanged(nameof(AmountText));
            OnPropertyChanged(nameof(AmountWithCurrency));
            OnPropertyChanged(nameof(IsInsideCart));
            OnPropertyChanged(nameof(AmountOperation));
        }

        public string CartsText => OrderQuantity > 0 ? ((double)OrderQuantity).ToKorM() : "+Cart";

        public double Amount => Price  * OrderQuantity;
        public string AmountWithCurrency => $"{AmountText} {Currency}";
        public string AmountText => Amount.ToString("N0");
        public string AmountOperation => $"{CartsText} x {PriceText} = {AmountWithCurrency}";
        public bool IsInsideCart => OrderQuantity > 0;
        

        private double sharesCount;
        public double SharesCount
        {
            get => sharesCount;
            set
            {
                SetProperty(ref sharesCount, value);
                OnPropertyChanged(nameof(SharesText));
            }
        }
        public string SharesText => SharesCount.ToKorM();

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
        public string VotesText => $"{VotesCount.ToKorM()} {(VotesCount > 1 ? "votes" : "vote")}";

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
            double likesCount, double commentsCount, int cartCount, double sharesCount, 
            bool isAvailable, IEnumerable<string> pictures, DateTime createdDate)
            : base(id)
        {
            Reference = reference?.ToUpper();
            Name = name?.ToFirstCharToUpper();
            Description = description;
            Category = category;
            Price = price;
            Currency = currency;
            StarsCount = starsCount;
            LikesCount = likesCount;
            CommentsCount = commentsCount;
            OrderQuantity = cartCount;
            SharesCount = sharesCount;
            VotesCount = votesCount;
            IsAvailable = isAvailable;
            Pictures = new ObservableCollection<string>(pictures);
            CreatedDate = createdDate;
            StarNumber = (int)Math.Round(StarsCount / VotesCount, 1);
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