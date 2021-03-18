using System;
using System.Collections.Generic;

namespace IucMarket.Dtos
{
    public class ListDto<T>
    {
        public List<T> Items { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string PageToken { get; set; }

        public ListDto()
        {
            Items = new List<T>();
            PageSize = 50;
        }

        public ListDto(List<T> items, int pageIndex, int pageSize)
        {
            Items = items;
            PageIndex = pageIndex;
            PageSize = pageSize;
        }

        public ListDto(List<T> items, string pageToken, int pageSize)
            :this(items, 0, pageSize)
        {
            PageToken = pageToken;
        }
    }
}