using IucMarket.Common;
using System;
using System.Collections.Generic;

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
        public bool IsEmailVerified { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public string RegistrationNumber { get; set; }
        public string Token { get; set; }
        public int TokenExpiresIn { get; set; }
        public IEnumerable<InteractionDto> ProductInteractions { get; set; }
        public UserDto():base()
        {

        }

        public UserDto(string personId, string fullName, string phoneCountryCode, 
            long phoneNumber, DateTime createdAt, RoleOptions role,
            bool status, bool isEmailVerified, string userId, string email, string registrationNumber,
            string token, int tokenExpiresIn,
            IEnumerable<InteractionDto> productInteractions)
        {
            PersonId = personId;
            FullName = fullName;
            PhoneCountryCode = phoneCountryCode;
            PhoneNumber = phoneNumber;
            CreatedAt = createdAt;
            Role = role;
            Status = status;
            IsEmailVerified = isEmailVerified;
            UserId = userId;
            Email = email;
            RegistrationNumber = registrationNumber;
            Token = token;
            TokenExpiresIn = tokenExpiresIn;
            ProductInteractions = productInteractions;
        }
    }
}