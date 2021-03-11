using System;
using System.Collections.Generic;

namespace IucMarket.Entities
{
    public class ProductList
    {
        public List<Product> Products { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        public ProductList()
        {
            Products = new List<Product>();
            PageSize = 50;
        }

        public ProductList(List<Product> products, int pageIndex, int pageSize)
        {
            Products = products;
            PageIndex = pageIndex;
            PageSize = pageSize;
        }
    }
}