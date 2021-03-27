using IucMarket.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IucMarket.Mobile.Services
{
    public interface IUserDataStore:IDataStore<UserModel>
    {
        //Task<UserModel> CheckCode(string phoneCountryCode, long phoneNumber, bool isWhatsApp, string code);
        //Task ResendCode(string fullPhoneNumber);
        Task<UserModel> LoginAsync(string email, string password);
        Task<bool> SendEmailAsync(string token);
    }
}
