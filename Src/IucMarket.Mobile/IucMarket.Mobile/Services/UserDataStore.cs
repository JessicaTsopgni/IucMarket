using IucMarket.Common;
using IucMarket.Dtos;
using IucMarket.Mobile.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace IucMarket.Mobile.Services
{
    public class UserDataStore : IUserDataStore
    {
        readonly List<UserModel> items;

        public UserDataStore()
        {
            
        }

        public async Task<UserModel> AddAsync(UserModel item)
        {
            try
            {
                if (App.HasNetwork)
                {
                    using (HttpClient client = new HttpClient { BaseAddress = new Uri(App.ApiAddress) })
                    {
                        HttpResponseMessage response;
                        if (string.IsNullOrEmpty(item.Id))
                        {
                            response = await client.PostAsync
                            (
                                $"Account",
                                new StringContent
                                (
                                    JsonConvert.SerializeObject
                                    ( 
                                        new RegisterCommand
                                        (
                                            item.Email,
                                            item.RegistrationNumber,
                                            item.Password,
                                            item.Name,
                                            item.PhoneCountryCode,
                                            item.PhoneNumber,
                                            true,
                                            RoleOptions.Other,
                                            false
                                        )
                                    ),
                                    Encoding.UTF8,
                                    "application/json"
                                )
                            );
                        }
                        else
                        {
                            var oldUser = await GetAsync(item.Id);
                            if (oldUser != null)
                            {
                                response = await client.PutAsync
                                (
                                    $"Account/{item.Id}",
                                    new StringContent
                                    (
                                        JsonConvert.SerializeObject
                                        (
                                           new RegisterCommand
                                           (
                                                item.Email,
                                                item.RegistrationNumber,
                                                item.Password,
                                                item.Name,
                                                item.PhoneCountryCode,
                                                item.PhoneNumber,
                                                !oldUser.Email.Equals(item.Email, StringComparison.OrdinalIgnoreCase),
                                                oldUser.Role,
                                                oldUser.Status
                                            )
                                        ),
                                        Encoding.UTF8,
                                        "application/json"
                                    )
                                );
                            }
                            else
                            {
                                throw new HttpRequestException($"User {item.Email} not found !");
                            }
                        }
                        if (response.IsSuccessStatusCode)
                        {
                            var json = await response.Content.ReadAsStringAsync();
                            var user = JsonConvert.DeserializeObject<UserDto>(json);
                            return new UserModel
                            (
                                user.UserId,
                                user.Email,
                                user.RegistrationNumber,
                                user.PhoneCountryCode,
                                user.PhoneNumber,
                                user.FullName,
                                user.CreatedAt,
                                new ObservableCollection<InteractionModel>
                                ( 
                                    user.ProductInteractions?.Select
                                    (
                                        x =>
                                        new InteractionModel
                                        (
                                            x.UserId,
                                            x.ProductId,
                                            x.InteractionType,
                                            x.Count,
                                            x.Content,
                                            x.CreatedAt
                                        )
                                    ).ToList() ?? new List<InteractionModel>()
                                ),
                                user.Token,
                                user.TokenExpiresIn,
                                user.IsEmailVerified,
                                user.Role,
                                user.Status
                            );
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

        public async Task<bool> SendEmailAsync(string token)
        {
            try
            {
                if (App.HasNetwork)
                {
                    using (HttpClient client = new HttpClient { BaseAddress = new Uri(App.ApiAddress) })
                    {
                        HttpResponseMessage response = await client.PostAsync
                        (
                                $"Account/Send",
                                new StringContent
                                (
                                    JsonConvert.SerializeObject
                                    (
                                        new {Token = token}
                                    ),
                                    Encoding.UTF8,
                                    "application/json"
                                )
                            );
                        
                        if (response.IsSuccessStatusCode)
                        {
                            var json = await response.Content.ReadAsStringAsync();
                            return true;
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
            return false;

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


        public async Task<UserModel> LoginAsync(string email, string password)
        {
            try
            {
                if (App.HasNetwork)
                {
                    using (HttpClient client = new HttpClient { BaseAddress = new Uri(App.ApiAddress) })
                    {
                        var response = await client.GetAsync($"Account/Login?email={email}&password={password}");
                        if (response.IsSuccessStatusCode)
                        {
                            var json = await response.Content.ReadAsStringAsync();
                            var user = JsonConvert.DeserializeObject<UserDto>(json);
                            var userModel = new UserModel
                            (
                                user.UserId,
                                user.Email,
                                user.RegistrationNumber,
                                user.PhoneCountryCode,
                                user.PhoneNumber,
                                user.FullName,
                                user.CreatedAt,
                                new ObservableCollection<InteractionModel>
                                (
                                    user.ProductInteractions?.Select
                                    (
                                        x =>
                                        new InteractionModel
                                        (
                                            x.UserId,
                                            x.ProductId,
                                            x.InteractionType,
                                            x.Count,
                                            x.Content,
                                            x.CreatedAt
                                        )
                                    ).ToList() ?? new List<InteractionModel>()
                                ),
                                user.Token,
                                user.TokenExpiresIn,
                                user.IsEmailVerified,
                                user.Role,
                                user.Status
                            );
                            return userModel;
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

        public async Task<IEnumerable<UserModel>> GetAsync(UserModel item)
        {
            await Task.FromException<NotImplementedException>(new NotImplementedException());
            return null;
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