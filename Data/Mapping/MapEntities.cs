using Data.Entities.Address;
using Data.Entities.Shop;
using Data.Entities.User;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Mapping
{
    public static class MapEntities
    {
        public static void MapUser(this ModelBuilder modelBuilder)
        {
            //to make phone number unique in column
            //modelBuilder.Entity<AppUser>().HasIndex(x => new { x.PhoneNumber }).IsUnique();
            //modelBuilder.Entity<AppUser>().HasMany(s=>s.UsersAddresses).WithOne(f=>f.AppUser);

            modelBuilder.Entity<AppUser>().HasMany(s => s.ShoppingCarts).WithOne(f => f.AppUser).HasForeignKey(s => s.AppUserId);
            //modelBuilder.Entity<AppUser>().HasMany(u => u.ShoppingCarts).with().HasForeignKey(h => h.appus);
        }
        public static void MapProduct(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasKey(s => s.ID);
            modelBuilder.Entity<Product>().HasOne(s => s.Category).WithMany(b => b.Products);
            modelBuilder.Entity<Product>().HasOne(s => s.ShoppingCartItem).WithOne(d => d.Product).HasForeignKey<ShoppingCartItem>(d => d.ProductID);
        }
        public static void MapCategory(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasKey(s => s.ID);
        }
        public static void MapAddress(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>().HasKey(s => s.ID);
            //modelBuilder.Entity<Address>().HasMany(s=>s.UsersAddresses).WithOne(f=>f.Address);
        }
        public static void MapUsersAddress(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserAddress>().HasKey(s => new { s.AddressId, s.AppUserId });

            modelBuilder.Entity<UserAddress>().HasOne(s => s.Address).WithMany(s => s.UsersAddresses).HasForeignKey(s => s.AddressId);
            modelBuilder.Entity<UserAddress>().HasOne(s => s.AppUser).WithMany(s => s.UsersAddresses).HasForeignKey(s => s.AppUserId);
        }
        public static void MapShoppingCart(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShoppingCart>().HasKey(s => s.ID);
            //modelBuilder.Entity<ShoppingCart>().HasOne(s => s.AppUser).WithMany(s => s.ShoppingCarts).HasForeignKey(s => s.AppUserId).OnDelete(DeleteBehavior.Cascade); ;
        }
        public static void MapCustomer(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().HasKey(s => s.ID);
            //modelBuilder.Entity<ShoppingCart>().HasOne(s => s.AppUser).WithMany(s => s.ShoppingCarts).HasForeignKey(s => s.AppUserId).OnDelete(DeleteBehavior.Cascade); ;
        }


    }
}
