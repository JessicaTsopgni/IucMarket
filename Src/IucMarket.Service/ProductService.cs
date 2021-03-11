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
        /// <summary>
        /// Log in with firebase authentication
        /// </summary>
        /// <param name="command">An instance of login command object</param>
        /// <exception cref="System.UnauthorizedAccessException">Thrown when username or password is invalid</exception>
        /// <returns>A Service.Product</returns>
        public async Task<Product> GetProductAsync(string key)
        {
            try
            {
                var product = await FirebaseClient
                       .Child(Table)
                       .Child(key)
                       .OnceSingleAsync<Product>();
                return product;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private async Task<Product> GetProductByReferenceAsync(string reference)
        {
            try
            {
                var products = await FirebaseClient
                       .Child(Table)
                       .OrderBy("Reference")
                       .EqualTo(reference)
                       .OnceAsync<Product>();

                var product = products?.FirstOrDefault()?.Object;
                product.Key = products?.FirstOrDefault()?.Key;
                return product;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<Product> AddAsync(Product product)
        {            
            try
            {
                if(await GetProductByReferenceAsync(product.Reference) != null)
                    throw new DuplicateWaitObjectException($"{nameof(product.Reference)} already exists !");

                var result = await FirebaseClient
                  .Child(Table)
                  .PostAsync(JsonConvert.SerializeObject(product));

                product.Key = result.Key;

                return product;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task EditAsync(string uid, Product product)
        {
            try
            {
                var oldProduct = await GetProductByReferenceAsync(product.Reference);

                if (oldProduct == null)
                    throw new KeyNotFoundException($"{nameof(Product)} {product.Reference} not found");
                if (oldProduct != null && oldProduct.Key != uid)
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

        public async Task<ProductList> GetProductsAsync(int pageIndex = 1, int pageSize = 100)
        {
            ProductList productList = new ProductList();
            productList.PageIndex = pageIndex;
            productList.PageSize = pageSize;
            try
            {
                var products = await FirebaseClient
                    .Child(Table)
                    .OnceAsync<Product>();

                UserService userService = new UserService();
                foreach(var p in products)
                {
                    productList.Products.Add
                    (
                        new Product
                        (
                            p.Key,
                            p.Object.Reference,
                            p.Object.Name,
                            p.Object.Description,
                            p.Object.Price,
                            p.Object.Currency,
                            p.Object.PictureNames,
                            p.Object.UserKey,
                            await userService.GetUserAsync(p.Object.UserKey),
                            p.Object.CreatedAt,
                            p.Object.Status
                        )
                    );
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return productList;
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
