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
    public class ProductService : BaseService
    {
        
        public readonly string Table = "Products";
        private readonly UserService userService;
        private readonly CategoryService categoryService;

        public ProductService(UserService userService, CategoryService categoryService)
        {
            this.userService = userService;
            this.categoryService = categoryService;
        }
        public async Task<ProductDto> GetProductAsync(string id, string path)
        {
            try
            {
                Product product = await GetAsync(id);

                return GetProductDto
                (
                    id,
                    product,
                    path,
                    await categoryService.GetCategoryAsync(product.CategoryId),
                    await userService.GetUserAsync(product.UserId)
                );
            }
            catch (Exception)
            {
                return null;
            }
        }

        internal async Task<Product> GetAsync(string id)
        {
            return await FirebaseClient
                        .Child(Table)
                        .Child(id)
                        .OnceSingleAsync<Product>();
        }

        private async Task<ProductDto> GetProductByReferenceAsync(string reference, string path)
        {
            try
            {
                var products = await FirebaseClient
                       .Child(Table)
                       .OrderBy("Reference")
                       .EqualTo(reference)
                       .OnceAsync<Product>();

                var product = products?.FirstOrDefault();

                return GetProductDto
                (
                    product?.Key,
                    product?.Object,
                    path,
                    await categoryService.GetCategoryAsync(product?.Object.CategoryId),
                    await userService.GetUserAsync(product?.Object.UserId)
                );
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

        public async Task<ListDto<ProductDto>> GetProductsByCategoryAsync(string id, string path, int pageIndex, int pageSize)
        {
            var list = new ListDto<ProductDto>();
            try
            {
                list.PageIndex = pageIndex;
                list.PageSize = pageSize;

                var products = await FirebaseClient
                       .Child(Table)
                       .OrderBy("CategoryId")
                       .EqualTo(id)
                       .OnceAsync<Product>();

                foreach (var p in products)
                {
                    list.Items.Add
                    (
                        GetProductDto
                        (
                            p.Key,
                            new Product
                            (
                                p.Object.Reference,
                                p.Object.Name,
                                p.Object.Description,
                                p.Object.Price,
                                p.Object.Currency,
                                p.Object.Pictures,
                                p.Object.CategoryId,
                                p.Object.UserId,
                                p.Object.CreatedAt,
                                p.Object.Status
                            ),
                            path,
                            await categoryService.GetCategoryAsync(p.Object.CategoryId),
                            await userService.GetUserAsync(p.Object.UserId)
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
            catch (Exception)
            {
                return null;
            }
            list.Items = list.Items.OrderByDescending(x => x.CreatedAt).ToList();
            return list;
        }

        public async Task<ProductDto> AddAsync(ProductAddCommand command, string path)
        {            
            try
            {
                if(await GetProductByReferenceAsync(command.Reference, path) != null)
                    throw new DuplicateWaitObjectException($"{nameof(command.Reference)} {command.Reference} already exists !");

                var result = await FirebaseClient
                  .Child(Table)
                  .PostAsync
                  (
                     JsonConvert.SerializeObject
                     (
                         new Product
                        (
                            command.Reference,
                            command.Name,
                            command.Description,
                            command.Price,
                            command.Currency,
                            command.Pictures.Select
                            (
                                x =>
                                new FileInfo
                                (
                                    x.Key,
                                    x.Value

                                )
                            ).ToArray(),
                            command.CategoryId,
                            command.OwnerId,
                            command.CreatedAt,
                            command.Status
                        )
                     )
                  );

                return GetProductDto
                (
                    result.Key,
                    new Product
                    (
                        command.Reference,
                        command.Name,
                        command.Description,
                        command.Price,
                        command.Currency,
                        command.Pictures.Select
                        (
                            x =>
                            new FileInfo
                            (
                                x.Key,
                                x.Value

                            )
                        ).ToArray(),
                        command.CategoryId,
                        command.OwnerId,
                        command.CreatedAt,
                        command.Status
                    ),
                    path,
                    await categoryService.GetCategoryAsync(command.CategoryId),
                    await userService.GetUserAsync(command.OwnerId)
                );
            }
            catch (Firebase.Database.FirebaseException ex)
            {
                if (ex.InnerException?.InnerException?.GetType() == typeof(SocketException))
                    throw new HttpRequestException("Cannot join the server. Please check your internet connexion.");
                throw ex;
            }
            catch (DuplicateWaitObjectException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task EditAsync(string id, ProductAddCommand command, string path)
        {
            try
            {
                var oldProduct1 = await GetProductAsync(id, path);
                if (oldProduct1 == null)
                    throw new KeyNotFoundException($"{nameof(Product)} {id} not found");

                var oldProduct2 = await GetProductByReferenceAsync(command.Reference, path);
                if (oldProduct2 != null && oldProduct2.Id != id)
                    throw new DuplicateWaitObjectException($"{nameof(Product)} {command.Reference} already exists !");

                await FirebaseClient
                  .Child(Table)
                  .Child(id)
                  .PutAsync
                  (
                    JsonConvert.SerializeObject
                    (
                        new Product
                        (
                            command.Reference,
                            command.Name,
                            command.Description,
                            command.Price,
                            command.Currency,
                            command.Pictures.Select
                            (
                                x =>
                                new FileInfo
                                (
                                    x.Key,
                                    x.Value

                                )
                            ).ToArray(),
                            command.CategoryId,
                            command.OwnerId,
                            oldProduct1.CreatedAt,
                            command.Status
                        )
                    )
                );
            }
            catch (Firebase.Database.FirebaseException ex)
            {
                if (ex.InnerException?.InnerException?.GetType() == typeof(SocketException))
                    throw new HttpRequestException("Cannot join the server. Please check your internet connexion.");
            }
            catch (KeyNotFoundException ex)
            {
                throw ex;
            }
            catch (DuplicateWaitObjectException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ListDto<ProductDto>> GetProductsAsync(string path , int pageIndex = 1, int pageSize = 100)
        {
            var list = new ListDto<ProductDto>();
            list.PageIndex = pageIndex;
            list.PageSize = pageSize;
            try
            {
                var products = await FirebaseClient
                    .Child(Table)
                    .OnceAsync<Product>();

                foreach(var p in products)
                {

                    list.Items.Add
                    (
                        GetProductDto
                        (
                            p.Key,
                            new Product
                            (
                                p.Object.Reference,
                                p.Object.Name,
                                p.Object.Description,
                                p.Object.Price,
                                p.Object.Currency,
                                p.Object.Pictures,
                                p.Object.CategoryId,
                                p.Object.UserId,
                                p.Object.CreatedAt,
                                p.Object.Status
                            ), 
                            path,
                            await categoryService.GetCategoryAsync(p.Object.CategoryId),
                            await userService.GetUserAsync(p.Object.UserId)
                        )
                    );
                }
            }
            catch(Firebase.Database.FirebaseException ex)
            {
                if (ex.InnerException?.InnerException?.GetType() == typeof(SocketException))
                    throw new HttpRequestException("Cannot join the server. Please check your internet connexion.");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            list.Items = list.Items.OrderByDescending(x => x.CreatedAt).ToList();
            return list;
        }

        public async Task DeleteAsync(string uid)
        {
            try
            {
               await FirebaseClient
                    .Child(Table)
                    .Child(uid)
                    .DeleteAsync();
            }
            catch (FirebaseAdmin.FirebaseException)
            {
                throw new KeyNotFoundException($"{nameof(Product)} {uid} not found");
            }
            catch (Firebase.Database.FirebaseException ex)
            {
                if (ex.InnerException?.InnerException?.GetType() == typeof(SocketException))
                    throw new HttpRequestException("Cannot join the server. Please check your internet connexion.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

         internal static ProductDto GetProductDto
        (string key, Product product, string picturePath, CategoryDto category, UserDto user)
        {
            if (product == null) return null;
            return new ProductDto
            (
                key,
                product.Reference,
                product.Name,
                product.Description,
                product.Price,
                product.Currency,
                product.Pictures?.Select
                (
                    x => 
                    new FileInfoDto
                    (
                        picturePath, 
                        x.Name, 
                        x.ContentType
                    )
                ).ToArray(),
                category,
                user,
                product.CreatedAt,
                product.Status
            );
        }
    }

}
