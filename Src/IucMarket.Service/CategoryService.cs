using Firebase.Database;
using Firebase.Database.Query;
using IucMarket.Service.Entities;
using IucMarket.Dtos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Sockets;

namespace IucMarket.Service
{
    public class CategoryService : BaseService
    {
        
        public readonly string Table = "Categories";

        public CategoryService()
        {
        }

        internal async Task<Category> GetAsync(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return null;

                var category = await FirebaseClient
                        .Child(Table)
                        .Child(id)
                        .OnceSingleAsync<Category>();

                return category;

            }
            catch (Firebase.Database.FirebaseException ex)
            {
                if (ex.InnerException?.InnerException?.GetType() == typeof(SocketException))
                    throw new HttpRequestException("Cannot join the server. Please check your internet connexion.");
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<CategoryDto> GetCategoryAsync(string id)
        {
            try
            {
                return GetCategoryDto(id, await GetAsync(id));
            }
            catch (Exception)
            {
                return null;
            }
        }

        internal static CategoryDto GetCategoryDto(string id, Category category)
        {
            if(category != null)
                return new CategoryDto
                (
                    id,
                    category.Name
                );
            return null;
        }

        public async Task<CategoryDto> GetCategoryByNameAsync(string name)
        {
            try
            {
                var products = await FirebaseClient
                       .Child(Table)
                       .OrderBy("Name")
                       .EqualTo(name)
                       .OnceAsync<Category>();

                var category = products?.FirstOrDefault()?.Object;

                return GetCategoryDto(products?.FirstOrDefault()?.Key, category);
            }
            catch (Firebase.Database.FirebaseException ex)
            {
                if (ex.InnerException?.InnerException?.GetType() == typeof(SocketException))
                    throw new HttpRequestException("Cannot join the server. Please check your internet connexion.");
                throw ex;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<CategoryDto> AddAsync(CategoryAddCommand command)
        {            
            try
            {
                if(await GetCategoryByNameAsync(command.Name) != null)
                    throw new DuplicateWaitObjectException($"{nameof(command.Name)} already exists !");
                var category = new Category(command.Name);
                var result = await FirebaseClient
                  .Child(Table)
                  .PostAsync(JsonConvert.SerializeObject(category));

                return GetCategoryDto(result.Key, category);
            }
            catch (Firebase.Database.FirebaseException ex)
            {
                if (ex.InnerException?.InnerException?.GetType() == typeof(SocketException))
                    throw new HttpRequestException("Cannot join the server. Please check your internet connexion.");
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task EditAsync(string id, CategoryAddCommand command)
        {
            try
            {
                var oldProduct1 = await GetCategoryAsync(id);
                if (oldProduct1 == null)
                    throw new KeyNotFoundException($"{nameof(Category)} {id} not found");

                var oldProduct2 = await GetCategoryByNameAsync(command.Name);
                if (oldProduct2 != null && oldProduct2.Id != id)
                    throw new DuplicateWaitObjectException($"{nameof(Category)} {command.Name} already exists !");

                await FirebaseClient
                  .Child(Table)
                  .Child(id)
                  .PutAsync(JsonConvert.SerializeObject(new Category(command.Name)));
            }
            catch (Firebase.Database.FirebaseException ex)
            {
                if (ex.InnerException?.InnerException?.GetType() == typeof(SocketException))
                    throw new HttpRequestException("Cannot join the server. Please check your internet connexion.");
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ListDto<CategoryDto>> GetCategoriesAsync(int pageIndex = 1, int pageSize = 100)
        {
            var list  = new ListDto<CategoryDto>();
            list.PageIndex = pageIndex;
            list.PageSize = pageSize;
            try
            {
                var products = await FirebaseClient
                    .Child(Table)
                    .OnceAsync<Category>();

                foreach(var p in products)
                {
                    list.Items.Add
                    (
                        GetCategoryDto
                        (
                            p.Key,
                            new Category
                            (
                                p.Object.Name
                            )
                        )
                    );
                }
            }
            catch (Firebase.Database.FirebaseException ex)
            {
                if (ex.InnerException?.InnerException?.GetType() == typeof(SocketException))
                    throw new HttpRequestException("Cannot join the server. Please check your internet connexion.");
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            list.Items = list.Items.OrderBy(x => x.Name).ToList();
            return list;
        }

        public async Task DeleteAsync(string id)
        {
            try
            {
               await FirebaseClient
                    .Child(Table)
                    .Child(id)
                    .DeleteAsync();
            }
            catch (FirebaseAdmin.FirebaseException)
            {
                throw new KeyNotFoundException($"{nameof(CategoryDto)} {id} not found");
            }
            catch (Firebase.Database.FirebaseException ex)
            {
                if (ex.InnerException?.InnerException?.GetType() == typeof(SocketException))
                    throw new HttpRequestException("Cannot join the server. Please check your internet connexion.");
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
