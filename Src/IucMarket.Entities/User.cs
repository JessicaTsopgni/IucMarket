using System;

namespace IucMarket.Entities
{
    public class User:Person
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public int ExpiresIn { get; set; }

        public User():base()
        {

        }

        public User(string uid, string fullName, DateTime createdAt, bool status, string email) :
           base(uid, fullName, createdAt, status)
        {
            Email = email;
        }

        public User(string uid, string fullName, DateTime createdAt, bool status, string email,
            string token, int expiresIn):
            this(uid, fullName, createdAt, status, email)
        {
            Token = token;
            ExpiresIn = expiresIn;
        }
    }
}