using IucMarket.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace IucMarket.Mobile.Services
{
    public class UserDataStore : IUserDataStore
    {
        readonly List<UserModel> items;

        public UserDataStore()
        {
            string[] names =
            {
                "Paul Alban",
                "Fatou Fati",
                "Ets Yoyo & fils",
                "Kouam Alex",
                "Boris Johnson",
                "Jessica Alba",
                "Trasnport pour tous",
                "Bitan Samuel",
                "Tonton sami",
                "Extensions"
            };
            Random r = new Random(Guid.NewGuid().GetHashCode());
            items = new List<UserModel>();
            for (int i = 0; i < names.Length; i++)
            {
                items.Add
                (
                    new UserModel
                    (
                        Guid.NewGuid().ToString(),
                        "Test@email.com",
                        names[i],
                        TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow.AddSeconds(-r.Next(0, 94608000)), TimeZoneInfo.Local)
                    )
                );
            }
        }

        public async Task<UserModel> AddAsync(UserModel item)
        {
            var index = items.IndexOf(item);
            if (index <0 )
                items.Add(item);
            else
               items[index] = item;

            return await Task.FromResult(item);
        }


        public async Task<UserModel> DeleteAsync(string id)
        {
            var oldItem = items.Where((UserModel arg) => arg.Id == id).FirstOrDefault();
            items.Remove(oldItem);

            return await Task.FromResult(oldItem);
        }

        public async Task<UserModel> GetAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<UserModel>> GetAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }

        public async Task<IEnumerable<UserModel>> GetAsync(UserModel item)
        {
            return await Task.FromResult
            (
                items.Where
                (
                    x => 
                    x.Name.Contains(item.Name) 
                ).ToArray()
            );
        }


        public async Task<UserModel> GetByUniqAsync(string uniqIdentity)
        {
            return await Task.FromResult
            (
                items.FirstOrDefault
                (
                    x => x.Id == uniqIdentity
                )
            );
        }


    }
}