using Data;
using Data.Entities;
using Data.Entities.Shop;
using Data.Entities.User;
using Data.Mapping;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repo
{
    public class ApplicationDBContext : IdentityDbContext<AppUser>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }
        #region for scaffolding and creating view from controller 
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server=.; Database=AsianG_DB;Trusted_Connection=True;");
        //}
        #endregion
        public DbSet<AppUser> AppUser { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Category> Category { get; set; }
        //public DbSet<Order> Product { get; set; }
        //public DbSet<Order> Order { get; set; }
        //public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<Customer> Customer { get; set; }
        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            base.OnModelCreating(modelbuilder);
            modelbuilder.MapUser();
            modelbuilder.MapProduct();
            modelbuilder.MapCategory();
            //modelbuilder.MapCustomer();
            //modelbuilder.MapOrder();
            //modelbuilder.MapOrderDetails();
        }
        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                entry.Entity.ModifiedDate = DateTime.Now;
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedDate = DateTime.Now;
                }
            }
            return base.SaveChanges();
        }
    }

}
