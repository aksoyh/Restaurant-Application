using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restaurant_Application.Model;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Collections.ObjectModel;

namespace Restaurant_Application.DB_Layer
{
    class DataAccessLayer
    {
        public static String ConnectionString;
        SqlConnection conn;
        private readonly RestaurantDB _rDBContext;
        enum OrderStatus { InProgress, OrderDelivered, Closed, Canceled };
        enum bookingStatus { Booked, Reserved, Available };

        public DataAccessLayer()
        {
            _rDBContext = new RestaurantDB();
        }
        public int InsertOrder(Orders ordObj, FoodItems food)
        {
            return 1;
        }
        public int InsertNewFoodItem(FoodItems newFoodItem)
        {
            ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
            conn = new SqlConnection(ConnectionString);
            conn.Open();
            string query = "Insert into FoodItems (FoodName, Description, fPrice) values (@FoodName, @Description, @Price) select SCOPE_IDENTITY()";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@FoodName", newFoodItem.FoodName);
            cmd.Parameters.AddWithValue("@Description", newFoodItem.Description);
            cmd.Parameters.AddWithValue("@Price", newFoodItem.fPrice);
            int foodid = Convert.ToInt32(cmd.ExecuteScalar());
            return foodid;
        }
        public void DeleteFoodDetails(FoodItems fooditem)
        {
            _rDBContext.FoodItems.Remove(fooditem);
            _rDBContext.SaveChanges();
        }
        public FoodItems getFoodDetails(int FoodID)
        {
            return _rDBContext.FoodItems.Where(p => p.FoodID == FoodID).FirstOrDefault();
        }
        public void DeleteOrderItem(ViewOrderItems obj)
        {
            _rDBContext.SaveChanges();
        }
        public List<FoodOrders> getOrderItems(Orders orderObj)
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
        public List<TableList> getTableListToPlaceHolder() // get available tables and list them.
        {
            return _rDBContext.TableList.ToList().Where(p => p.BookingStatus != "Booked").ToList();
        }

        public object getTableList()
        {
            return _rDBContext.TableList.ToList();
        }

        public ICollection<FoodItems> getFoodItems()
        {
            return _rDBContext.FoodItems.OrderBy(p => p.FoodID).ToArray();
        }

        public bool PlaceOrder(List<ViewOrderItems> obj)
        {
            int FoodOrderID = 0;
            int totalprice = obj.Sum(p => p.Price);
            ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
            conn = new SqlConnection(ConnectionString);
            conn.Open();
            string query = "give some info about Queries, later";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@CreateDate", DateTime.Now);
            cmd.Parameters.AddWithValue("@TotalPrice", totalprice);
            cmd.Parameters.AddWithValue("@OrderStatus", OrderStatus.InProgress.ToString());
            cmd.Parameters.AddWithValue("@TableID", obj[0].TableId);

            int orderID = Convert.ToInt32(cmd.ExecuteScalar());

            if (orderID > 0)
            {
                foreach(var item in obj)
                {
                    string query1 = "give some info about Queries, later";
                    SqlCommand cmd1 = new SqlCommand(query1, conn);
                    cmd1.Parameters.AddWithValue("@OrderID", orderID);
                    cmd1.Parameters.AddWithValue("@FoodID", item.FoodId);
                    cmd1.Parameters.AddWithValue("@Quantity", item.Quantity);
                    cmd1.Parameters.AddWithValue("@Price", item.Price);
                }
            }

            string updateTableStatus = "give some info";
            SqlCommand cmd2 = new SqlCommand(updateTableStatus, conn);
            cmd2.Parameters.AddWithValue("@BookingStatus", bookingStatus.Booked.ToString());
            cmd2.Parameters.AddWithValue("@TableID", obj[0].TableId);

            cmd2.ExecuteNonQuery();
            conn.Close();

            if(FoodOrderID > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public ObservableCollection<ViewOrderItems> getFoodOrderDetails(TableList sTableList)
        {
            DataTable dt = new DataTable();
            ObservableCollection<ViewOrderItems> orderItems = new ObservableCollection<ViewOrderItems>();
            ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
            conn = new SqlConnection(ConnectionString);
            conn.Open();
            string query = @"select * from TableLists t inner join Orders o on t.TableID = o.TableID inner join FoodOrders fo on 
                            fo.OrderID = fo.OrderID inner join FoodItems ft on ft.FoodID = fo.FoodID where t.BookingStatus = @BookingStatus and 
                            t.Table = @TableID and o.OrderStatus = @OrderStatus";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@BookingStatus", bookingStatus.Booked.ToString());
            cmd.Parameters.AddWithValue("@TableID", sTableList.TableID);
            cmd.Parameters.AddWithValue("@OrderStatus", OrderStatus.InProgress.ToString());
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            for(int i = 0; dt.Rows.Count > 0; i++)
            {
                ViewOrderItems orderItem = new ViewOrderItems();
                orderItem.fPrice = Convert.ToInt32(dt.Rows[i]["fPrice"]);
                orderItem.FoodOrderId = Convert.ToInt32(dt.Rows[i]["FoodOrderID"]);
                orderItem.OrderId = Convert.ToInt32(dt.Rows[i]["OrderID"]);
                orderItem.FoodId = Convert.ToInt32(dt.Rows[i]["FoodID"]);
                orderItem.TableId = Convert.ToInt32(dt.Rows[i]["TableID"]);
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

        public void UpdateOrderDetails(ViewOrderItems selectedOrderItems)
        {
            ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
            conn = new SqlConnection(ConnectionString);
            conn.Open();
            string query = "Update FoodOrders set Quantity = @Quantity, Price = @Price where FoodOrderID = @FoodOrderID";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@FoodOrderID", selectedOrderItems.FoodOrderId);
            cmd.Parameters.AddWithValue("@Quantity", selectedOrderItems.Quantity);
            cmd.Parameters.AddWithValue("@Price", selectedOrderItems.Price);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public void UpdateFoodDetails(FoodItems fooditem)
        {
            var entity = _rDBContext.FoodItems.Find(fooditem.FoodID);
            if(entity == null)
            {
                throw new NotImplementedException("Handle appropriately for our API design");
            }
            _rDBContext.Entry(fooditem).State = System.Entity.EntityState.Modified;
            _rDBContext.SaveChanges();
        }

        public void UpdateTableStatus(TableList sTableList)
        {
            ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
            conn = new SqlConnection(ConnectionString);
            conn.Open();
            string updateTableStatus = "Update TableList set BookingStatus = @BookingStatus where TableID = @TableID";
            SqlCommand cmd2 = new SqlCommand(updateTableStatus, conn);
            cmd2.Parameters.AddWithValue("@BookingStatus", bookingStatus.Available.ToString());
            cmd2.Parameters.AddWithValue("@TableID", sTableList.TableID);
            cmd2.ExecuteNonQuery();

            string updateOrderStatus = "Update Orders set OrderStatus = @OrderStatus where TableID = @TableID";
            SqlCommand cmd = new SqlCommand(updateOrderStatus, conn);
            cmd.Parameters.AddWithValue("@OrderStatus", OrderStatus.Closed.ToString());
            cmd.Parameters.AddWithValue("@TableID", sTableList.TableID);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}
