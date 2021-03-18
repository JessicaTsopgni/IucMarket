using System;
using System.Collections.Generic;
using System.Text;

namespace IucMarket.Dtos
{
    public class LoginCommand
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public LoginCommand()
        {

        }
        public LoginCommand(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }

}
