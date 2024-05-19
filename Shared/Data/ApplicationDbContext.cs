using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.MenuItem;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Shared.OrderModels;
using Shared.Ingredients;
using Shared.PaymentModels;

namespace Shared.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName() ?? throw new NullReferenceException();
                if (tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Substring(6));
                }
            }
        }
        public DbSet<Item> Items { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Image> Images { get; set; }

        public DbSet<BTable> BTables {get;set;}
        public DbSet<Order> Orders {get;set;}
        public DbSet<OrderDetail> OrderDetails {get;set;}
        public DbSet<Ingredient> Ingredients {get;set;}
        public DbSet<Ingredient_Stock> Ingredient_Stocks {get;set;}
        public DbSet<Item_Ingredient> Item_Ingredients {get;set;}
        public DbSet<Order_IngredientStock> Order_IngredientStocks {get;set;}
        public DbSet<Stock> Stocks {get;set;}
        public DbSet<Payment> Payments {get;set;}
        public DbSet<Rating> Ratings {get;set;}
    }
}
