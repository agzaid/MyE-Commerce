﻿using Data.Entities.Address;
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
            modelBuilder.Entity<AppUser>().HasMany(s=>s.UsersAddresses).WithOne(f=>f.AppUser);
        } 
        public static void MapProduct(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasKey(s => s.ID);
            modelBuilder.Entity<Product>().HasOne(s => s.Category).WithMany(b => b.Products);
        }
        public static void MapCategory(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasKey(s => s.ID);
        }
        public static void MapAddress(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>().HasKey(s => s.ID);
            modelBuilder.Entity<Address>().HasMany(s=>s.UsersAddresses).WithOne(f=>f.Address);
        }
    }
}
