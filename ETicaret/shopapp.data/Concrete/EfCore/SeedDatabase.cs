using System.Linq;
using Microsoft.EntityFrameworkCore;
using shopapp.entity;

namespace shopapp.data.Concrete.EfCore
{
    public static class SeedDatabase
    {
        public static void Seed()
        {
            var context = new ShopContext();
            if(context.Database.GetPendingMigrations().Count()==0)
            {
                if(context.Categories.Count()==0)
                {
                    context.Categories.AddRange(Categories);
                }
                if(context.Products.Count()==0)
                {
                    context.Products.AddRange(Products);
                    context.AddRange(ProductCategories);
                }
            }
            context.SaveChanges();
        }
        private static Category[] Categories={
            new Category(){Name="Telefon",Url="telefon"},
            new Category(){Name="Bilgisayar",Url="bilgisayar"},
            new Category(){Name="Elektronik",Url="elektronik"},
            new Category(){Name="Beyaz Eşya",Url="beyaz-esya"}
        };
        private static Product[] Products={
            new Product(){Name="Samsung S20+",Price=8500,ImageUrl="1.jpg",Description="QHD+ 120Hz Super Amoled 8GB Ram 120GB 30x Dijital Zoom",Url="samsung-S20+"},
            new Product(){Name="Iphone X",Price=6000,ImageUrl="2.jpg",Description="OLED FHD+ 60Hz",Url="iphone-x"},
            new Product(){Name="Iphone 11",Price=7100,ImageUrl="3.jpg",Description="HD+ 60Hz IPS ",Url="iphone-11"},
            new Product(){Name="Samsung S20Fe",Price=6000,ImageUrl="4.jpg",Description="FHD+ 120Hz Super Amoled 6GB Ram 128GB 30x Dijital Zoom",Url="samsung-S20Fe"},
            new Product(){Name="Casper Nirvana",Price=5000,ImageUrl="5.jpg",Description="8GB Ram intel i5",Url="casper-nirvana"}
        };
        private static ProductCategory[] ProductCategories={
            new ProductCategory(){Product=Products[0],Category=Categories[0]},
            new ProductCategory(){Product=Products[0],Category=Categories[2]},

            new ProductCategory(){Product=Products[1],Category=Categories[0]},
            new ProductCategory(){Product=Products[1],Category=Categories[2]},

            new ProductCategory(){Product=Products[2],Category=Categories[0]},
            new ProductCategory(){Product=Products[2],Category=Categories[2]},

            new ProductCategory(){Product=Products[3],Category=Categories[0]},
            new ProductCategory(){Product=Products[3],Category=Categories[2]},

            new ProductCategory(){Product=Products[4],Category=Categories[1]},
            new ProductCategory(){Product=Products[4],Category=Categories[2]},
        };
    }
}