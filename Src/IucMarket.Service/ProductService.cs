using Firebase.Database;
using Firebase.Database.Query;
using IucMarket.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IucMarket.Service
{
    public class ProductService : BaseService
    {
        
        public readonly string Table = "Products";
        private readonly UserService userService;

        public ProductService(UserService userService)
        {
            this.userService = userService;
        }
        /// <summary>
        /// Log in with firebase authentication
        /// </summary>
        /// <param name="command">An instance of login command object</param>
        /// <exception cref="System.UnauthorizedAccessException">Thrown when username or password is invalid</exception>
        /// <returns>A Service.Product</returns>
        public async Task<Product> GetProductAsync(string key, string path)
        {
            try
            {
                var product = await FirebaseClient
                       .Child(Table)
                       .Child(key)
                       .OnceSingleAsync<Product>();

                await SetProduct(product, key, path);
                return product;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private async Task<Product> GetProductByReferenceAsync(string reference, string path)
        {
            try
            {
                var products = await FirebaseClient
                       .Child(Table)
                       .OrderBy("Reference")
                       .EqualTo(reference)
                       .OnceAsync<Product>();

                var product = products?.FirstOrDefault()?.Object;
                await SetProduct(product, products?.FirstOrDefault()?.Key, path);
                return product;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private async Task SetProduct(Product product, string key, string path)
        {
            if (product != null)
            {
                product.Key = key;
                product.Pictures = product.Pictures.Select(x => new FileInfo(string.Format(path, x.Name, x.ContentType), x.ContentType)).ToArray();
                product.Owner = await GetUserAsync(product.UserKey);
            }
        }

        public async Task<Product> AddAsync(Product product, string path)
        {            
            try
            {
                if(await GetProductByReferenceAsync(product.Reference, path) != null)
                    throw new DuplicateWaitObjectException($"{nameof(product.Reference)} already exists !");

                var result = await FirebaseClient
                  .Child(Table)
                  .PostAsync(JsonConvert.SerializeObject(product));

                await SetProduct(product, result?.Key, path);

                return product;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task EditAsync(string uid, Product product, string path)
        {
            try
            {
                var oldProduct1 = await GetProductAsync(uid, path);
                if (oldProduct1 == null)
                    throw new KeyNotFoundException($"{nameof(Product)} {uid} not found");

                var oldProduct2 = await GetProductByReferenceAsync(uid, path);
                if (oldProduct2 != null && oldProduct2.Key != uid)
                    throw new DuplicateWaitObjectException($"{nameof(Product)} {product.Reference} already exists !");
    
                    await FirebaseClient
                      .Child(Table)
                      .Child(product.Key)
                      .PutAsync(JsonConvert.SerializeObject(product));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ProductList> GetProductsAsync(string path , int pageIndex = 1, int pageSize = 100)
        {
            ProductList productList = new ProductList();
            productList.PageIndex = pageIndex;
            productList.PageSize = pageSize;
            try
            {
                var products = await FirebaseClient
                    .Child(Table)
                    .OnceAsync<Product>();

                foreach(var p in products)
                {
                    var product = new Product
                    (
                        p.Key,
                        p.Object.Reference,
                        p.Object.Name,
                        p.Object.Description,
                        p.Object.Price,
                        p.Object.Currency,
                        p.Object.Pictures,
                        p.Object.UserKey,
                        p.Object.Owner,
                        p.Object.CreatedAt,
                        p.Object.Status
                    );
                    await SetProduct(product, p.Key, path);
                    productList.Products.Add(product);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return productList;
        }

        private async Task<User> GetUserAsync(string userId)
        {
            return await userService.GetUserAsync(userId);
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
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
