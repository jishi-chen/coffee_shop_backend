using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.Model.ViewModels
{
    public class PaginatedList
    {
        public PaginationModel PaginationModel { get; set; }

        public PaginatedList(int count, int pageIndex, int pageSize)
        {
            PaginationModel = new PaginationModel
            {
                TotalItems = count,
                PageIndex = pageIndex,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize),
                HasPreviousPage = pageIndex > 1,
                HasNextPage = pageIndex < (int)Math.Ceiling(count / (double)pageSize),
            };
        }
    }

    public class PaginationModel
    {
        public int TotalItems { get; set; }
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
    }
}
