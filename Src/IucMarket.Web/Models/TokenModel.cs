﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IucMarket.Web.Models
{
    public class TokenModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public int ExpiresIn { get; set; }

        public TokenModel()
        {

        }

        public TokenModel(string name, string email, string token, int expiresIn)
        {
            Name = name;
            Email = email;
            Token = token;
            ExpiresIn = expiresIn;
        }
    }
}
