using Plugin.SecureStorage;
using System;
using System.Collections.Generic;
using System.Text;

namespace IucMarket.Mobile.Services
{
    public class SecureStorage : ISecureStorage
    {
        public bool Exist(string key)
        {
            return CrossSecureStorage.Current.HasKey(key);
        }

        public string Get(string key)
        {
            return CrossSecureStorage.Current.GetValue(key);
        }

        public void Remove(string key)
        {
            CrossSecureStorage.Current.DeleteKey(key);
        }

        public void Set(string key, string value)
        {
            CrossSecureStorage.Current.SetValue(key, value);
        }
    }
}
