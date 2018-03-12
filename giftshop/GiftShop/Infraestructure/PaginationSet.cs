using System;
using System.Collections.Generic;
using System.Linq;

namespace GiftShop.Infraestructure
{
    public class PaginationSet<T>
    {
        public int Page { get; set; }
        public int Count
        {
            get
            {
                return (null != Items) ? Items.Count() : 0;
            }
        }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public IEnumerable<T> Items { get; set; }
    }

    public static class ListExtention
    {
        public static PaginationSet<T> ToPagedList<T>(this IEnumerable<T> items, int currentPage, int totalRecords, int currentPageSize)
        {
            PaginationSet<T> pagedSet = new PaginationSet<T>()
            {
                Page = currentPage,
                TotalCount = totalRecords,
                TotalPages = (int)Math.Ceiling((decimal)totalRecords / currentPageSize),
                Items = items.Skip(currentPage * currentPageSize).Take(currentPageSize)
            };

            return pagedSet;
        }
    }
}