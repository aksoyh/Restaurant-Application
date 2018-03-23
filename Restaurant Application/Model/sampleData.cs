using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_Application.Model
{
    class sampleData : DropCreateDatabaseAlways<RestaurantDB>
    {
        enum bookingstatus { Available, Reserved, Booked }
        protected override void Seed(RestaurantDB context)
        {
            try
            {
                List<FoodItems> fooditems = new List<FoodItems>()
                {
                    new FoodItems { FoodName = "Adana Kebap", Description = "Acılı Adana Usülü Kebap", fPrice = 25 },
                    new FoodItems { FoodName = "Urfa Kebap", Description = "Acısız Urfa Usülü Kebap", fPrice = 25 },
                    new FoodItems { FoodName = "Mantı", Description = "Kayseri Usülü Mantı", fPrice = 18 },
                    new FoodItems { FoodName = "Analı Kızlı", Description = "Maraş Usülü Analı-Kızlı Çorba", fPrice = 12 },
                    new FoodItems { FoodName = "Yayık Ayran", Description = "Geleneksel Yayık Ayran", fPrice = 5 }
                };
                foreach(FoodItems f in fooditems)
                {
                    context.FoodItems.Add(f);
                }
                List<TableList> tablelist = new List<TableList>()
                {
                    new TableList { TableName = "Masa 1", BookingStatus = bookingstatus.Available.ToString() },
                    new TableList { TableName = "Masa 2", BookingStatus = bookingstatus.Available.ToString() },
                    new TableList { TableName = "Masa 3", BookingStatus = bookingstatus.Available.ToString() },
                    new TableList { TableName = "Masa 4", BookingStatus = bookingstatus.Available.ToString() },
                    new TableList { TableName = "Masa 5", BookingStatus = bookingstatus.Available.ToString() },
                    new TableList { TableName = "Masa 6", BookingStatus = bookingstatus.Available.ToString() },
                    new TableList { TableName = "Masa 7", BookingStatus = bookingstatus.Available.ToString() },
                    new TableList { TableName = "Masa 8", BookingStatus = bookingstatus.Available.ToString() },
                    new TableList { TableName = "Masa 9", BookingStatus = bookingstatus.Available.ToString() },
                    new TableList { TableName = "Masa 10", BookingStatus = bookingstatus.Available.ToString() }
                };
                foreach (TableList t in tablelist)
                {
                    context.TableList.Add(t);
                }
                context.SaveChanges();
                base.Seed(context);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
