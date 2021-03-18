using System;
using System.Collections.Generic;
using System.Text;

namespace IucMarket.Mobile.Services
{
    public interface ISecureStorage
    {
        void Set(string key, string value);
        string Get(string key);
        bool Exist(string key);
        void Remove(string key);
    }
}
