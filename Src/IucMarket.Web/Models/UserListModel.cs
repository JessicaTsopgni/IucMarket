using IucMarket.Common;
using System;

namespace IucMarket.Web.Models
{
    public class UserListModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Fullname { get; set; }
        public RoleOptions Role { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Status { get; set; }
        public UserListModel()
        {

        }

        public UserListModel(string id, string email, string fullname, DateTime createdDate,
            RoleOptions role, bool status)
        {
            Id = id;
            Email = email;
            Fullname = fullname;
            CreatedDate = createdDate;
            Role = role;
            Status = status ? "Enabled" : "Disabled";
        }
    }
}
