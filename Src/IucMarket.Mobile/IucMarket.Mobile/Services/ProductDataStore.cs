using IucMarket.Dtos;
using IucMarket.Mobile.Models;
using Newtonsoft.Json;
using Plugin.SecureStorage;
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

                            items = list.Items.Select(x => GetProductModel(x, 0)).ToList();
                        }
                        else
                        {
                            throw new HttpRequestException(await response.Content.ReadAsStringAsync());
                        }
                    }
                }
                else
                {
                    throw new HttpRequestException("No internet connection !");
                }
            }
            catch (HttpRequestException ex)
            {
                throw ex;
            }
            catch (TaskCanceledException)
            {
                throw new HttpRequestException("Cannot join the server!");
            }
            catch (Exception ex)
            {
                throw ex;
            }
                
        }

        public static ProductModel GetProductModel(ProductDto product, int orderQuantity)
        {
            var r = new Random();
            var votesCount = r.Next(0, 10000);

            return new ProductModel
            (
                product.Id,
                product.Reference,
                product.Name,
                product.Description,
                new CategoryModel(product.Category.Id, product.Category.Name),
                product.Price,
                product.Currency,
                r.Next(0, votesCount * ProductModel.StarsMax),
                votesCount,
                r.Next(0, 1000000),
                r.Next(0, 1000000),
                orderQuantity,
                r.Next(0, 1000000),
                r.Next(0, 2) == 1 ? true : false,
                product.Pictures?.Select(y => y.Path).ToArray(),
                product.CreatedAt
            );
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
                            return list.Items.Where(x => x.Id != item.Id).Select(x => GetProductModel(x, 0)).ToList();
                        }
                        else
                        {
                            throw new HttpRequestException(await response.Content.ReadAsStringAsync());
                        }
                    }
                }
                else
                {
                    throw new HttpRequestException("No internet connection !");
                }
            }
            catch (HttpRequestException ex)
            {
                throw ex;
            }
            catch (TaskCanceledException)
            {
                throw new HttpRequestException("Cannot join the server!");
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