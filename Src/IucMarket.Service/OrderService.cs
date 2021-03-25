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
    public class OrderService : BaseService
    {
        
        public readonly string Table = "Orders";
        private static ProductService _productService;
        private static UserService _userService;
        private static CategoryService _categoryService;

        public OrderService(ProductService productService, UserService userService, CategoryService categoryService)
        {
            _productService = productService;
            _userService = userService;
            _categoryService = categoryService;
        }

        internal async Task<Order> GetAsync(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return null;

                var order = await FirebaseClient
                        .Child(Table)
                        .Child(id)
                        .OnceSingleAsync<Order>();

                return order;

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
        public async Task<OrderDto> GetOrderAsync(string id, string productPicturePath)
        {
            try
            {
                return await GetOrder(id, await GetAsync(id), productPicturePath);
            }
            catch (Exception)
            {
                return null;
            }
        }

        internal static async Task<OrderDto> GetOrder(string id, Order order, string productPicturePath)
        {
            if(order != null)
                return new OrderDto
                (
                    id,
                    order.Number,
                    order.State,
                    order.StateReason,
                    order.Details.Select
                    (
                        x =>
                        new OrderDetailDto
                        (
                            ProductService.GetProductDto
                            (
                                x.ProductId, 
                                x.Product,
                                productPicturePath,
                                _categoryService.GetCategoryAsync(x.Product.CategoryId).Result,
                                null
                            ),
                            x.Quantity
                        )
                    ).ToArray(),
                    await _userService.GetUserAsync(order.Customer.UserId),
                    order.CreatedAt,
                    order.DeliveryPlace,
                    order.DeliveryPredicateAt,
                    order.DeliveryAt
                );
            return null;
        }

        public async Task<OrderDto> GetOrderByNumberAsync(string name, string productPicturePath)
        {
            try
            {
                var orders = await FirebaseClient
                       .Child(Table)
                       .OrderBy("Number")
                       .EqualTo(name)
                       .OnceAsync<Order>();

                var order = orders?.FirstOrDefault()?.Object;

                return await GetOrder(orders?.FirstOrDefault()?.Key, order, productPicturePath);
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

        private async Task<string> GenerateNumber()
        {
            await Task.Delay(1000);
            DateTime _now = DateTime.Now;
            string _dd = _now.ToString("dd"); //
            string _mm = _now.ToString("MM");
            string _yy = _now.ToString("yyyy");
            string _hh = _now.Hour.ToString();
            string _min = _now.Minute.ToString();
            string _ss = _now.Second.ToString();

            string _uniqueId = _yy + _mm + _dd + _hh + _min + _ss;
            return _uniqueId;
        }

        public async Task<OrderDto> AddAsync(OrderAddCommand command, string productPicturePath)
        {            
            try
            {
                string number = string.Empty;
                do
                {
                    number = await GenerateNumber();                    
                }
                while (await GetOrderByNumberAsync(number, productPicturePath) != null);

                var order = new Order
                (
                    number,
                    command.Carts.Select
                    (
                        x => new OrderDetail
                        (
                            x.Key,
                            _productService.GetAsync(x.Key).Result,
                            x.Value
                        )
                    ),
                    (await _userService.GetAsync(command.CustomerId)).Value,
                    command.DeliveryPlace,
                    command.CreatedAt,
                    Common.StateOptions.In_process,
                    null,
                    null,
                    null
                );

                var result = await FirebaseClient
                  .Child(Table)
                  .PostAsync(JsonConvert.SerializeObject(order));

                return await GetOrder(result.Key, order, productPicturePath);
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


        public async Task EditAsync(string id, OrderEditCommand command, string productPicturePath)
        {
            try
            {
                var oldOrder = await GetAsync(id);
                if (oldOrder == null)
                    throw new KeyNotFoundException($"{nameof(Order)} {id} not found");

                var order = new Order
                (
                      oldOrder.Number,
                      oldOrder.Details,
                      oldOrder.Customer,
                      command.DeliveryPlace,
                      oldOrder.CreatedAt,
                      command.State,
                      command.StateReason,
                      command.DeliveryPredicateAt,
                      command.DeliveryAt
                  );
                await FirebaseClient
                  .Child(Table)
                  .Child(id)
                  .PutAsync(JsonConvert.SerializeObject(order));
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

        public async Task<ListDto<OrderDto>> GetOrdersAsync(string productPicturePath, int pageIndex = 1, int pageSize = 100)
        {
            var list  = new ListDto<OrderDto>();
            list.PageIndex = pageIndex;
            list.PageSize = pageSize;
            try
            {
                var products = await FirebaseClient
                    .Child(Table)
                    .OnceAsync<Order>();

                foreach(var p in products)
                {
                    list.Items.Add
                    (
                        await GetOrder
                        (
                            p.Key,
                            p.Object,
                            productPicturePath
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
            list.Items = list.Items.OrderBy(x => x.CreatedAt).ToList();
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
                throw new KeyNotFoundException($"{nameof(Order)} {id} not found");
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
