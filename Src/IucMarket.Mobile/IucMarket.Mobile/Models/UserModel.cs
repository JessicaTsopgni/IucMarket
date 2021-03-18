using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace IucMarket.Mobile.Models
{
    public class UserModel:BaseModel
    {
        private string email;
        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

        private string name;
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        private ObservableCollection<ProductModel> products;
        public ObservableCollection<ProductModel> Products
        {
            get => products;
            set => SetProperty(ref products, value);
        }

        private ObservableCollection<ProductModel> productsRated;
        public ObservableCollection<ProductModel> ProductsRated
        {
            get => productsRated;
            set => SetProperty(ref productsRated, value);
        }

        private DateTime registrationDate;
        public DateTime RegistrationDate
        {
            get => registrationDate;
            set => SetProperty(ref registrationDate, value);
        }

        public string Token { get; set; }
        public int TokenExpiresIn { get; set; }

        public UserModel(string id):base(id)
        {
            productsRated = new ObservableCollection<ProductModel>();
        }

        public UserModel(string id, string email, string name, DateTime registrationDate)
            : this(id)
        {
            Email = email;
            Name = name;
            RegistrationDate = registrationDate;
        }


        public UserModel(string id, string email, string name, DateTime registrationDate,
            IEnumerable<ProductModel> products,
            IEnumerable<ProductModel> productsRated) :
            this(id, email, name, registrationDate)
        {
            Products = new ObservableCollection<ProductModel>(products);
            ProductsRated = new ObservableCollection<ProductModel>(productsRated);
        }


        public UserModel(string id, string email, string name, DateTime registrationDate, 
            string token, int tokenExpiresIn)
            : this(id, email, name, registrationDate)
        {
            Token = token;
            TokenExpiresIn = tokenExpiresIn;
        }
    }
}
