using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using Restaurant_Application.Model;
using System.Data;

namespace Restaurant_Application.DB_Layer
{
    class DataAccessLayer
    {
        public static String ConnectionString;
        SqlConnection conn;
        private readonly RestaurantDB _rDBContext;
        enum OrderStatus { InProgress, OrderDelivered, Closed, Cancelled};
        enum bookingStatus { Booked, Reserved, Available};

        public DataAccessLayer()
        {
            _rDBContext = new RestaurantDB();
        }
        public ICollection<FoodItems> GetFoodItems()
        {
            return _rDBContext.FoodItems.OrderBy(p => p.FoodID).ToArray();
        }
        public List<TableList> getTableList()
        {
            return _rDBContext.TableList.ToList();
        }
        public int InsertNewFoodItems(FoodItems newFoodItem)
        {
            ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
            conn = new SqlConnection(ConnectionString);
            conn.Open();
            string query = "Insert into FoodItem (FoodName, Description, fPrice) values (@FoodName, @Description, @Price) select SCOPE_IDENTITY()";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@FoodName", newFoodItem.FoodName);
            cmd.Parameters.AddWithValue("@Description", newFoodItem.Description);
            cmd.Parameters.AddWithValue("@Price", newFoodItem.fPrice);
            int foodid = Convert.ToInt32(cmd.ExecuteScalar());

            return foodid;
        }

        // Update ve Delete kısımlarının hataları çok olduğu için açıklaması sonra yapılacak.
        public void UpdateFoodDetails(FoodItems fooditem)
        {
            var entity = _rDBContext.FoodItems.Find(fooditem.FoodID);
            if(entity == null)
            {
                throw new NotImplementedException("Handle appropriately for your API design");
            }
            _rDBContext.Entry(fooditem).State = System.Data.Entity.EntityState.Modified;
            _rDBContext.SaveChanges();
        }
        public void UpdateFoodDetails(ViewOrderItems fooditem)
        {
            // Complate it later. Be careful, we must to use just English comments!!! This is for Mert!!!
        }
        public void DeleteFoodDetails(FoodItems fooditem)
        {
            _rDBContext.FoodItems.Remove(fooditem);
            _rDBContext.SaveChanges();
        }
        public List<TableList> getTableListToPlaceHolder()
        {
            return _rDBContext.TableList.ToList().Where(p => p.BookingStatus != "Booked").ToList();
        }
        public List<FoodOrders> getOrderItems(Orders orderObj) // Explain it later in deeply detailed.
        {
            Orders order = new Orders();
            var data = from o in _rDBContext.Orders
                       join t in _rDBContext.TableList on o.TableID equals t.TableID
                       where t.TableID == orderObj.TableID && t.BookingStatus == "Booked"
                       select new { o.OrderID };
            foreach(var a in data)
            {
                order.OrderID = a.OrderID;
            }
            return _rDBContext.FoodOrders.ToList().Where(p => p.OrderID == order.OrderID).ToList();
        }
        public int InsertOrder(Orders ordObj, FoodOrders food)
        {
            return 1; // We can return warning report
        }
        public FoodItems getFoodDetails(int FoodID)
        {
            return _rDBContext.FoodItems.Where(p => p.FoodID == FoodID).FirstOrDefault();
        }
        public void DeleteOrderItems(ViewOrderItems obj)
        {
            _rDBContext.SaveChanges();
        }
        public bool PlaceOrder(List<ViewOrderItems> obj)
        {
            int FoodOrderID = 0;
            int totalprice = obj.Sum(p => p.Price);
            ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
            conn = new SqlConnection(ConnectionString);
            conn.Open();
            string query = "Insert to the Orders......"; // Complete it later, this line
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@CreateDate", DateTime.Now);
            cmd.Parameters.AddWithValue("@TotalPrice", totalprice);
            cmd.Parameters.AddWithValue("@OrderStatus", OrderStatus.InProgress.ToString());
            cmd.Parameters.AddWithValue("@TableID", obj[0].TableId);

            int orderID = Convert.ToInt32(cmd.ExecuteScalar());

            if(orderID > 0)
            {
                foreach(var item in obj)
                {
                    string query1 = "Insert....."; // Complate this line later.
                    SqlCommand cmd1 = new SqlCommand(query1, conn);
                    cmd1.Parameters.AddWithValue("@OrderID", orderID);
                    cmd1.Parameters.AddWithValue("@FoodID", item.FoodId);
                    cmd1.Parameters.AddWithValue("@Quantity", item.Quantity);
                    cmd1.Parameters.AddWithValue("@Price", item.Price);
                    FoodOrderID = Convert.ToInt32(cmd1.ExecuteScalar());
                }
            }

            string updateTableStatus = "Update TableList set BookingStatus = @BookingStatus where TableID = @TableID";
            SqlCommand cmd2 = new SqlCommand(updateTableStatus, conn);
            cmd2.Parameters.AddWithValue("@BookingStatus", bookingStatus.Booked.ToString());
            cmd2.Parameters.AddWithValue("@TableID", obj[0].TableId);

            cmd2.ExecuteNonQuery();
            conn.Close();

            if (FoodOrderID > 0)
            {
                return true;
            }
            else
                return false;
        }
    }
}
