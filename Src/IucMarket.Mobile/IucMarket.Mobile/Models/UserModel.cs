using IucMarket.Common;
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

        private string registrationNumber;
        public string RegistrationNumber
        {
            get => registrationNumber;
            set => SetProperty(ref registrationNumber, value);
        }

        private string phoneCountryCode;
        public string PhoneCountryCode
        {
            get => phoneCountryCode;
            set => SetProperty(ref phoneCountryCode, value);
        }

        private long phoneNumber;
        public long PhoneNumber
        {
            get => phoneNumber;
            set
            {
                SetProperty(ref phoneNumber, value);
                OnPropertyChanged(nameof(FullPhoneNumber));
            }
        }
        public string FullPhoneNumber => $"{PhoneCountryCode} {PhoneNumber}";

        private string name;
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        public string Password { get; private set; }
        public bool IsEmailVerified { get; private set; }
        public RoleOptions Role { get; private set; }
        public bool Status { get; private set; }

        private ObservableCollection<InteractionModel> productInteractions;
        public ObservableCollection<InteractionModel> ProductInteractions
        {
            get => productInteractions;
            set => SetProperty(ref productInteractions, value);
        }

        private DateTime registrationDate;
        public DateTime RegistrationDate
        {
            get => registrationDate;
            set => SetProperty(ref registrationDate, value);
        }

        public string Token { get; set; }
        public int TokenExpiresIn { get; set; }


        public UserModel()
        {
            productInteractions = new ObservableCollection<InteractionModel>();
        }
        public UserModel(string id):base(id)
        {
            productInteractions = new ObservableCollection<InteractionModel>();
        }
        public UserModel(string email, string registrationNumber,
            string phoneCountryCode, long phoneNumber, string name, string password) : this(null)
        {
            Email = email;
            RegistrationNumber = registrationNumber;
            PhoneCountryCode = phoneCountryCode;
            PhoneNumber = phoneNumber;
            Name = name;
            Password = password;
        }

        public UserModel(string id, string email, string registrationNumber,
            string phoneCountryCode, long phoneNumber, string name,
            DateTime registrationDate, ObservableCollection<InteractionModel> productInteractions,
            string token, int tokenExpiresIn, bool isEmailverified, RoleOptions role, bool status) : this(id)
        {
            Email = email;
            RegistrationNumber = registrationNumber;
            PhoneCountryCode = phoneCountryCode;
            PhoneNumber = phoneNumber;
            RegistrationNumber = registrationNumber;
            Name = name;
            ProductInteractions = productInteractions;
            RegistrationDate = registrationDate;
            Token = token;
            TokenExpiresIn = tokenExpiresIn;
            IsEmailVerified = isEmailverified;
            Role = role;
            Status = status;
        }
    }
}
