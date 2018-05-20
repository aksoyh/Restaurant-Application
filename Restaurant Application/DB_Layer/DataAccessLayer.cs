using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using Restaurant_Application.Model;
using System.Data;
using System.Collections.ObjectModel;

namespace Restaurant_Application.DB_Layer
{
    public class DataAccessLayer
    {
        public static String ConnectionString;
        SqlConnection conn;
        private readonly RestaurantDB _rDBContext;
        enum Orderstatus { InProgress, OrderDelivered, Closed, Cancelled };
        enum bookingstatus { Booked, Reserved, Available };

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

        public int InsertNewFoodItem(FoodItems newfoodItem)
        {
            ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
            conn = new SqlConnection(ConnectionString);
            conn.Open();
            string query = "Insert into FoodItems (FoodName,Description,fPrice) values (@FoodName,@Description,@Price) select SCOPE_IDENTITY()";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@FoodName", newfoodItem.FoodName);
            cmd.Parameters.AddWithValue("@Description", newfoodItem.Description);
            cmd.Parameters.AddWithValue("@Price", newfoodItem.fPrice);

            int foodid = Convert.ToInt32(cmd.ExecuteScalar());

            return foodid;
        }

        public void UpdateFoodDetails(FoodItems fooditem)
        {
            var entity = _rDBContext.FoodItems.Find(fooditem.FoodID);

            if (entity == null)
            {
                throw new NotImplementedException("Handle appropriately for your API design.");
            }
            
            _rDBContext.Entry(fooditem).State = System.Data.Entity.EntityState.Modified;
            _rDBContext.SaveChanges();
        }

        public void UpdateOrderDetails(ViewOrderItems fooditem)
        {
            ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
            conn = new SqlConnection(ConnectionString);
            conn.Open();
            string query = "Update FoodOrders set Quantity = @Quantity, Price = @Price where FoodOrderID = @FoodOrderID";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@FoodOrderID", fooditem.FoodOrderID);
            cmd.Parameters.AddWithValue("@Quantity", fooditem.Quantity);
            cmd.Parameters.AddWithValue("@Price", fooditem.Price);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public void DeleteFoodDetails(FoodItems foodItem)
        {
            _rDBContext.FoodItems.Remove(foodItem);
            _rDBContext.SaveChanges();
        }

        public List<TableList> getTableListToPlaceOrder()
        {
            return _rDBContext.TableList.ToList().Where(p => p.BookingStatus != "Booked").ToList();
        }

        public List<FoodOrders> getOrderItems(Orders orderObj)
        {
            Orders order = new Orders();
            var data = from o in _rDBContext.Orders
                       join t in _rDBContext.TableList on o.TableID equals t.TableID
                       where t.TableID == orderObj.TableID && t.BookingStatus == "Booked"
                       select new { o.OrderID };
            foreach (var a in data)
            {
                order.OrderID = a.OrderID;
            }

            return _rDBContext.FoodOrders.ToList().Where(p => p.OrderID == order.OrderID).ToList();
        }

        public int InsertOrder(Orders ordObj, FoodItems food)
        {
            return 1;
        }

        public FoodItems getFoodDetails(int FoodID)
        {
            return _rDBContext.FoodItems.Where(p => p.FoodID == FoodID).FirstOrDefault();
        }

        public void DeleteOrderItem(ViewOrderItems obj)
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
            string query = "Insert into Orders (CreatedDate,TotalPrice,OrderStatus,TableID) values (@CreatedDate,@TotalPrice,@OrderStatus,@TableID) select SCOPE_IDENTITY()";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
            cmd.Parameters.AddWithValue("@TotalPrice", totalprice);
            cmd.Parameters.AddWithValue("@OrderStatus", Orderstatus.InProgress.ToString());
            cmd.Parameters.AddWithValue("@TableID", obj[0].TableID);
            //cmd.ExecuteScalar();

            int orderID = Convert.ToInt32(cmd.ExecuteScalar());

            if (orderID > 0)
            {
                foreach (var item in obj)
                {
                    string query1 = "Insert into FoodOrders (OrderID,FoodID,Quantity,Price) values (@OrderID,@FoodID,@Quantity,@Price) select SCOPE_IDENTITY()";
                    SqlCommand cmd1 = new SqlCommand(query1, conn);
                    cmd1.Parameters.AddWithValue("@OrderID", orderID);
                    cmd1.Parameters.AddWithValue("@FoodID", item.FoodID);
                    cmd1.Parameters.AddWithValue("@Quantity", item.Quantity);
                    cmd1.Parameters.AddWithValue("@Price", item.Price);
                    FoodOrderID = Convert.ToInt32(cmd1.ExecuteScalar());
                }

            }

            string updateTablestatus = "Update TableLists set BookingStatus = @BookingStatus where TableID = @TableID";
            SqlCommand cmd2 = new SqlCommand(updateTablestatus, conn);
            cmd2.Parameters.AddWithValue("@BookingStatus", bookingstatus.Booked.ToString());
            cmd2.Parameters.AddWithValue("@TableID", obj[0].TableID);

            cmd2.ExecuteNonQuery();
            conn.Close();

            if (FoodOrderID > 0)
                return true;
            else
                return false;
        }

        public ObservableCollection<ViewOrderItems> getFoodOrderDetails(TableList selectedtable)
        {
            DataTable dt = new DataTable();
            ObservableCollection<ViewOrderItems> orderItems = new ObservableCollection<ViewOrderItems>();
            ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
            conn = new SqlConnection(ConnectionString);
            conn.Open();
            string query = @"select * from TableLists t inner join Orders o on t.TableID = o.TableID inner join FoodOrders fo on
                            fo.OrderID = o.OrderID inner join FoodItems ft on ft.FoodID = fo.FoodID where t.BookingStatus = @BookingStatus and t.TableID = @TableID and o.OrderStatus = @OrderStatus";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@BookingStatus", bookingstatus.Booked.ToString());
            cmd.Parameters.AddWithValue("@TableID", selectedtable.TableID);
            cmd.Parameters.AddWithValue("@OrderStatus", Orderstatus.InProgress.ToString());
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            for (int i = 0; dt.Rows.Count > i; i++)
            {
                ViewOrderItems orderItem = new ViewOrderItems();
                orderItem.fPrice = Convert.ToInt32(dt.Rows[i]["fPrice"]);
                orderItem.FoodOrderID = Convert.ToInt32(dt.Rows[i]["FoodOrderID"]);
                orderItem.OrderID = Convert.ToInt32(dt.Rows[i]["OrderID"]);
                orderItem.FoodID = Convert.ToInt32(dt.Rows[i]["FoodID"]);
                orderItem.TableID = Convert.ToInt32(dt.Rows[i]["FoodID"]);
                orderItem.FoodName = dt.Rows[i]["FoodName"].ToString();
                orderItem.TableName = dt.Rows[i]["TableName"].ToString();
                orderItem.OrderCreatedDate = Convert.ToDateTime(dt.Rows[i]["CreatedDate"]);
                orderItem.OrderStatus = dt.Rows[i]["OrderStatus"].ToString();
                orderItem.Quantity = Convert.ToInt32(dt.Rows[i]["Quantity"]);
                orderItem.Price = Convert.ToInt32(dt.Rows[i]["Price"]);
                orderItem.BookingStatus = dt.Rows[i]["BookingStatus"].ToString();
                orderItems.Add(orderItem);
            }

            return orderItems;
        }

        public void UpdateTableStatus(TableList t)
        {
            ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
            conn = new SqlConnection(ConnectionString);
            conn.Open();
            string updateTablestatus = "Update TableLists set BookingStatus = @BookingStatus where TableID = @TableID";
            SqlCommand cmd2 = new SqlCommand(updateTablestatus, conn);
            cmd2.Parameters.AddWithValue("@BookingStatus", bookingstatus.Available.ToString());
            cmd2.Parameters.AddWithValue("@TableID", t.TableID);

            cmd2.ExecuteNonQuery();

            string updateOrderStatus = "Update Orders set OrderStatus = @OrderStatus where TableID = @TableID";
            SqlCommand cmd = new SqlCommand(updateOrderStatus, conn);
            cmd.Parameters.AddWithValue("@OrderStatus", Orderstatus.Closed.ToString());
            cmd.Parameters.AddWithValue("@TableID", t.TableID);

            cmd.ExecuteNonQuery();

            conn.Close();
        }
    }
}
