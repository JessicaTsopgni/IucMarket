using IucMarket.Dtos;
using IucMarket.Mobile.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace IucMarket.Mobile.Services
{
    public class ProductDataStore : IProductDataStore
    {
        List<ProductModel> items;

        IDataStore<UserModel> UserDataStore => DependencyService.Get<IDataStore<UserModel>>();

        public ProductDataStore()
        {
            items = new List<ProductModel>();
        }

        private async Task LoadItems()
        {
            try
            {
                if (App.HasNetwork)
                {
                    using (HttpClient client = new HttpClient { BaseAddress = new Uri(App.ApiAddress) })
                    {
                        var response = await client.GetAsync($"Article/Index");
                        if (response.IsSuccessStatusCode)
                        {
                            var json = await response.Content.ReadAsStringAsync();
                            var list = JsonConvert.DeserializeObject<ListDto<ProductDto>>(json);
                            var r = new Random();
                            items = list.Items.Select
                            (
                                x =>
                                {
                                    var votesCount = r.Next(0, 10000);
                                    return new ProductModel
                                    (
                                        x.Id,
                                        x.Reference,
                                        x.Name,
                                        x.Description,
                                        new CategoryModel(x.Category.Id, x.Category.Name),
                                        x.Price,
                                        x.Currency,
                                        r.Next(0, votesCount * ProductModel.StarsMax),
                                        votesCount,
                                        r.Next(0, 1000000),
                                        r.Next(0, 1000000),
                                        r.Next(0, 1000000),
                                        r.Next(0, 1000000),
                                        r.Next(0, 2) == 1 ? true : false,
                                        x.Pictures?.Select(y => y.Path).ToArray(),
                                        new UserModel
                                        (
                                            x.Owner?.UserId,
                                            x.Owner?.Email,
                                            x.Owner?.FullName,
                                            x.Owner?.CreatedAt ?? DateTime.MinValue,
                                            x.Owner?.Token,
                                            x.Owner.TokenExpiresIn
                                        ),
                                        x.CreatedAt
                                    );
                                }
                            ).ToList();
                        }
                        else
                        {
                            throw new Exception(await response.Content.ReadAsStringAsync());
                        }
                    }
                }
                else
                {
                    throw new Exception("No internet connection !");
                }
            }
            catch(TaskCanceledException)
            {
                throw new Exception("Cannot join the server!");
            }
            catch (Exception ex)
            {
                throw ex;
            }
                
        }

        public async Task<ProductModel> AddAsync(ProductModel item)
        {
            items.Add(item);

            return await Task.FromResult(item);
        }

        public async Task<ProductModel> DeleteAsync(string id)
        {
            var oldItem = items.Where((ProductModel arg) => arg.Id == id).FirstOrDefault();
            items.Remove(oldItem);

            return await Task.FromResult(oldItem);
        }

        public async Task<ProductModel> GetAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<ProductModel>> GetAsync(bool forceRefresh = false)
        {
            if (forceRefresh)
                await LoadItems();
            return await Task.FromResult(items);
        }

        public async Task<IEnumerable<ProductModel>> GetAsync(ProductModel item)
        {
            try
            {
                if (App.HasNetwork)
                {
                    using (HttpClient client = new HttpClient { BaseAddress = new Uri(App.ApiAddress) })
                    {
                        var response = await client.GetAsync($"Article/Categories/{item.Category.Id}");
                        if (response.IsSuccessStatusCode)
                        {
                            var json = await response.Content.ReadAsStringAsync();
                            var list = JsonConvert.DeserializeObject<ListDto<ProductDto>>(json);
                            var r = new Random();
                            return list.Items.Where(x => x.Id != item.Id).Select
                            (
                                x =>
                                {
                                    var votesCount = r.Next(0, 10000);
                                    return new ProductModel
                                    (
                                        x.Id,
                                        x.Reference,
                                        x.Name,
                                        x.Description,
                                        new CategoryModel(x.Category.Id, x.Category.Name),
                                        x.Price,
                                        x.Currency,
                                        r.Next(0, votesCount * ProductModel.StarsMax),
                                        votesCount,
                                        r.Next(0, 1000000),
                                        r.Next(0, 1000000),
                                        r.Next(0, 1000000),
                                        r.Next(0, 1000000),
                                        r.Next(0, 2) == 1 ? true : false,
                                        x.Pictures?.Select(y => y.Path).ToArray(),
                                        new UserModel
                                        (
                                            x.Owner?.UserId,
                                            x.Owner?.Email,
                                            x.Owner?.FullName,
                                            x.Owner?.CreatedAt ?? DateTime.MinValue,
                                            x.Owner?.Token,
                                            x.Owner.TokenExpiresIn
                                        ),
                                        x.CreatedAt
                                    );
                                }
                            ).ToArray();
                        }
                        else
                        {
                            throw new Exception(await response.Content.ReadAsStringAsync());
                        }
                    }
                }
                else
                {
                    throw new Exception("No internet connection !");
                }
            }
            catch (TaskCanceledException)
            {
                throw new Exception("Cannot join the server!");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<ProductModel> GetByUniqIdAsync(string uniqId)
        {
            return await Task.FromResult
            (
                items.FirstOrDefault
                (
                    x =>
                    x.Reference == uniqId
                )
            );
        }

        public async Task<ProductModel> Rate(ProductModel product, UserModel owner, int value)
        {
            var index = items.IndexOf(product);
            var o = await UserDataStore.GetAsync(owner.Id);
            if (o != null && index >= 0)
            {
                var t = items[index];
                t.VotesCount++;
                t.StarsCount += value;
                owner.ProductsRated.Add(t);
                await AddAsync(t);
                await UserDataStore.AddAsync(owner);

                return await Task.FromResult(t);
            }
            return await Task.FromResult(product);
        }

        public Task<ProductModel> Abuse(ProductModel product, UserModel owner, string message)
        {
            throw new NotImplementedException();
        }
    }
}