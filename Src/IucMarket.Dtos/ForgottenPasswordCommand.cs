using System;
using System.Collections.Generic;
using System.Text;

namespace IucMarket.Dtos
{
    public class ForgottenPasswordCommand
    {
        public string Email { get; set; }

        public ForgottenPasswordCommand()
        {

        }
        public ForgottenPasswordCommand(string email)
        {
            Email = email;
        }
    }

}
