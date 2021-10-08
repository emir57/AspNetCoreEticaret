using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace shopapp.entity
{
    public class Product
    {
        public int ProductId { get; set; }
        
        public string Name { get; set; }
        public string Url { get; set; }
        public decimal? Price { get; set; }  
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public bool IsApproved { get; set; }
        public bool IsHome { get; set; }
        

        public List<ProductCategory> ProductCategories { get; set; }

    }
}