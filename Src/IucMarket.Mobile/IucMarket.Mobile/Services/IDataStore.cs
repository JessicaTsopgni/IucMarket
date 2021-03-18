using IucMarket.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IucMarket.Mobile.Services
{
    public interface IDataStore<T>
    {
        Task<T> AddAsync(T item);
        Task<T> DeleteAsync(string id);
        Task<T> GetAsync(string id);
        Task<IEnumerable<T>> GetAsync(bool forceRefresh = false);
        Task<IEnumerable<T>> GetAsync(T item);
    }
}
