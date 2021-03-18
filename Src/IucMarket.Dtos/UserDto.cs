using IucMarket.Common;
using System;

namespace IucMarket.Dtos
{
    public class UserDto
    {
        public string PersonId { get; set; }
        public string FullName { get; set; }
        public string PhoneCountryCode { get; set; }
        public long PhoneNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public RoleOptions Role { get; set; }
        public bool Status { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public int TokenExpiresIn { get; set; }

        public UserDto():base()
        {

        }

        public UserDto(string personId, string fullName, string phoneCountryCode, 
            long phoneNumber, DateTime createdAt, RoleOptions role,
            bool status, string userId, string email, string password, string token, int tokenExpiresIn)
        {
            PersonId = personId;
            FullName = fullName;
            PhoneCountryCode = phoneCountryCode;
            PhoneNumber = phoneNumber;
            CreatedAt = createdAt;
            Role = role;
            Status = status;
            UserId = userId;
            Email = email;
            Password = password;
            Token = token;
            TokenExpiresIn = tokenExpiresIn;
        }
    }
}