using Microsoft.EntityFrameworkCore;
using shopapp.entity;

namespace shopapp.data.Concrete.EfCore
{
    public class ShopContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductCategory>()
                .HasKey(e=>new{e.CategoryId,e.ProductId});
            modelBuilder.Entity<ProductCategory>()
                .HasOne(e=>e.Category)
                .WithMany(e=>e.ProductCategories)
                .HasForeignKey(e=>e.CategoryId);
            modelBuilder.Entity<ProductCategory>()
                .HasOne(e=>e.Product)
                .WithMany(e=>e.ProductCategories)
                .HasForeignKey(e=>e.ProductId);
                
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders{get;set;}
        public DbSet<OrderItem> OrderItems{get;set;}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=ShopApp;integrated security=true;");
        }
    }
}