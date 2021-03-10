using System;
using System.Collections.Generic;

namespace IucMarket.Entities
{
    public class UserList
    {
        public List<User> Users { get; set; }
        public string PageToken { get; set; }
        public int PageSize { get; set; }

        public UserList()
        {
            Users = new List<User>();
            PageSize = 50;
        }

        public UserList(List<User> users, string pageToken, int pageSize)
        {
            Users = users;
            PageToken = pageToken;
            PageSize = pageSize;
        }
    }
}