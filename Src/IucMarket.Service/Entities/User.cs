using IucMarket.Common;
using System;

namespace IucMarket.Service.Entities
{
    internal class User :Person
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public int ExpiresIn { get; set; }

        public User():base()
        {

        }

        public User(string userId, string fullName, string phoneCountryCode, long phoneNumber, DateTime createdAt, 
            RoleOptions role, bool isEmailVerified, bool status, string email, string registrationNumber, string password) :
           base(userId, fullName, registrationNumber, phoneCountryCode, phoneNumber, createdAt, role, isEmailVerified, status)
        {
            Email = email;
            Password = password;
        }

        public User(string userId, string fullName, string phoneCountryCode, long phoneNumber,
            DateTime createdAt, RoleOptions role, bool isEmailVerified, bool status, string email,
            string registrationNumber, string password, string token, int expiresIn):
            this(userId, fullName, phoneCountryCode, phoneNumber, createdAt, role, 
               isEmailVerified, status, email, registrationNumber,  password)
        {
            Token = token;
            ExpiresIn = expiresIn;
        }
    }
}