using IucMarket.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IucMarket.Mobile.Services
{
    public interface IProductDataStore:IDataStore<ProductModel>
    {
        Task<ProductModel> Rate(ProductModel product, UserModel user, int value);
        Task<ProductModel> Abuse(ProductModel product, UserModel user, string message);
    }
}
