using IucMarket.Dtos;
using IucMarket.Mobile.Models;
using Newtonsoft.Json;
using Plugin.SecureStorage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace IucMarket.Mobile.Services
{
    public class OrderDataStore : IOrderDataStore
    {
        List<OrderModel> items;

        public OrderDataStore()
        {
            items = new List<OrderModel>();
        }

        private async Task LoadItems()
        {
            try
            {
                if (App.HasNetwork)
                {
                    using (HttpClient client = new HttpClient { BaseAddress = new Uri(App.ApiAddress) })
                    {
                        var response = await client.GetAsync($"Command/Index");
                        if (response.IsSuccessStatusCode)
                        {
                            var json = await response.Content.ReadAsStringAsync();
                            var list = JsonConvert.DeserializeObject<ListDto<OrderDto>>(json);

                            items = list.Items.Select(x => GetOrderModel(x)).ToList();
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

        public static OrderModel GetOrderModel(OrderDto order)
        {
            return new OrderModel
            (
                order.Id,
                order.Number,
                new ObservableCollection<ProductModel>
                (
                    order.Details.Select(y => ProductDataStore.GetProductModel(y.Product, y.Quantity)).ToArray()
                ),
                order.DeliveryPlace,
                order.CreatedAt,
                order.DeliveryPredicateAt,
                order.DeliveryAt,
                order.Customer.UserId,
                order.State,
                order.Comment
            );
        }

        public async Task<OrderModel> AddAsync(OrderModel item)
        {
            try
            {
                if (App.HasNetwork)
                {
                    using (HttpClient client = new HttpClient { BaseAddress = new Uri(App.ApiAddress) })
                    {
                        var json = JsonConvert.SerializeObject
                        (
                            new OrderAddCommand
                            (
                                item.DeliveryPlace,
                                item.Products.ToDictionary
                                (
                                    x => x.Id,
                                    y => y.OrderQuantity
                                ),
                                item.CustomerId
                            )
                        );
                        var response = await client.PostAsync
                        (
                            $"Command",
                            new StringContent
                            (
                                  json,
                                  System.Text.Encoding.UTF8,
                                  "application/json"
                            )
                        );
                        if (response.IsSuccessStatusCode)
                        {
                            json = await response.Content.ReadAsStringAsync();
                            var order = JsonConvert.DeserializeObject<OrderDto>(json);
                            return GetOrderModel(order);
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

        public async Task<OrderModel> DeleteAsync(string id)
        {
            var oldItem = items.Where((OrderModel arg) => arg.Id == id).FirstOrDefault();
            items.Remove(oldItem);

            return await Task.FromResult(oldItem);
        }

        public async Task<OrderModel> GetAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<OrderModel>> GetAsync(bool forceRefresh = false)
        {
            if (forceRefresh)
                await LoadItems();
            return await Task.FromResult(items);
        }

        public async Task<IEnumerable<OrderModel>> GetAsync(OrderModel item)
        {
            try
            {
                if (App.HasNetwork)
                {
                    using (HttpClient client = new HttpClient { BaseAddress = new Uri(App.ApiAddress) })
                    {
                        var response = await client.GetAsync($"Command/Customer/{item.CustomerId}");
                        if (response.IsSuccessStatusCode)
                        {
                            var json = await response.Content.ReadAsStringAsync();
                            var list = JsonConvert.DeserializeObject<ListDto<OrderDto>>(json);
                            return items = list.Items.Select(x => GetOrderModel(x)).ToList();
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

        public async Task<OrderModel> GetByUniqIdAsync(string uniqId)
        {
            return await Task.FromResult
            (
                items.FirstOrDefault
                (
                    x =>
                    x.Number == uniqId
                )
            );
        }


    }
}