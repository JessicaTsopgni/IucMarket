﻿using System;

namespace IucMarket.Entities
{
    public class User:Person
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public int ExpiresIn { get; set; }

        public User():base()
        {

        }

        public User(string userId, string fullName, string phoneCountryCode, long phoneNumber, DateTime createdAt, 
            RoleOptions role, bool status, string email, string password) :
           base(userId, fullName, phoneCountryCode, phoneNumber, createdAt, role, status)
        {
            Email = email;
            Password = password;
        }

        public User(string userId, string fullName, string phoneCountryCode, long phoneNumber,
            DateTime createdAt, RoleOptions role, bool status, string email,
           string password, string token, int expiresIn):
            this(userId, fullName, phoneCountryCode, phoneNumber, createdAt, role, status, email, password)
        {
            Token = token;
            ExpiresIn = expiresIn;
        }
    }
}