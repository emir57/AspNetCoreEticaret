using System;
using System.Collections.Generic;
using shopapp.entity;

namespace shopapp.webui.ViewModels
{
    public class PageInfo
    {
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }
        public string CurrentCategory { get; set; }

        public int TotalPages()
        {
            double totalPage = TotalItems/ItemsPerPage;
            
            return (int)Math.Ceiling(totalPage);
        }
    
    }
    public class ProductListViewModel
    {
        public PageInfo PageInfo { get; set; }
        public List<Product> Products { get; set; }
        public int CurrentPage { get; set; }
        public string CurrentCategory { get; set; }
        public int TotalPage { get; set; }
    }
    
}