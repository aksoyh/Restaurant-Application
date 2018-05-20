using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_Application.Model
{
    public class RestaurantDB : DbContext
    {
        public RestaurantDB() : base("DBContext")
        {
            Database.SetInitializer(new sampleData());
        }
        public DbSet<FoodItems> FoodItems { get; set; }
        public DbSet<TableList> TableList { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<FoodOrders> FoodOrders { get; set; }
    }
}
