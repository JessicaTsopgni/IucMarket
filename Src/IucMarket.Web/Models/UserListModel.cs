using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IucMarket.Web.Models
{
    public class UserListModel
    {
        public string Uid { get; set; }
        public string Email { get; set; }
        public string Fullname { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Status { get; set; }
        public UserListModel()
        {

        }

        public UserListModel(string uid, string email, string fullname, DateTime createdDate, bool status)
        {
            Uid = uid;
            Email = email;
            Fullname = fullname;
            CreatedDate = createdDate;
            Status = status ? "Enable" : "Disabled";
        }
    }
}
