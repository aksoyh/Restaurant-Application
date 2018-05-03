using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Restaurant_Application.Model;
using System.Data.SqlClient;
using System.Data;

namespace Restaurant_Application.DB_Layer
{
    class DataAccessLayer
    {
        private readonly RestaurantDB _rDBContext;
        public static String ConnectionString;
        SqlConnection conn;
        enum OrderingStatus { Closed, Canceled, InProgress, OrderDelivered};
        enum bookingStatus { Booked, Reserved, Available};

        public DataAccessLayer()
        {
            _rDBContext = new RestaurantDB();
        }
        public ObservableCollection<ViewOrderItems> getFoodOrderDetails(TableList sTableList)
        {
            DataTable dt = new DataTable();
            ObservableCollection<ViewOrderItems> orderItems = new ObservableCollection<ViewOrderItems>();
            ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
            conn = new SqlConnection(ConnectionString);
            conn.Open();
            string query = "Complate it later...";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@BookingStatus", bookingStatus.Booked.ToString());
            cmd.Parameters.AddWithValue("@TableID", sTableList.TableID);
            cmd.Parameters.AddWithValue("@OrderStatus", OrderingStatus.InProgress.ToString());
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            for (int i = 0; i < dt.Rows.Count; i++)
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
        public int InsertNewFoodItems(FoodItems newfoodItem)
        {
            ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
            conn = new SqlConnection(ConnectionString);
            conn.Open();
            string query = "Insert into FoodItems (FoodName,Description,fPrice) values (@FoodName,@Description,@Price) select SCOPE_IDENTITY()";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@FoodName", newfoodItem.FoodName);
            cmd.Parameters.AddWithValue("@Description", newfoodItem.Description);
            cmd.Parameters.AddWithValue("@Price", newfoodItem.fPrice);
            //cmd.ExecuteScalar();
            int foodid = Convert.ToInt32(cmd.ExecuteScalar());
            return foodid;
        }

        public void UpdateOrderDetails(ViewOrderItems fooditem)
        {
            ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
            conn = new SqlConnection(ConnectionString);
            conn.Open();
            string query = "Update FoodOrders set Quantity = @Quantity Price = @Price where FoodOrderID = @FoodOrderID";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@FoodOrderID", fooditem.FoodOrderId);
            cmd.Parameters.AddWithValue("@Price", fooditem.FoodId);
            cmd.Parameters.AddWithValue("@Quantity", fooditem.Quantity);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public void UpdateTableStatus(TableList sTableList)
        {
            ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
            conn = new SqlConnection(ConnectionString);
            conn.Open();
            string updateTablestatus = "Update TableList set BookingStatus = @BookingStatus where TableID = @TableID";
            SqlCommand cmd2 = new SqlCommand(updateTablestatus, conn);
            cmd2.Parameters.AddWithValue("@BookingStatus", bookingStatus.Booked.ToString());
            cmd2.Parameters.AddWithValue("@TableID", sTableList.TableID);
            cmd2.ExecuteNonQuery();

            string updateOrderStatus = "Update Orders set OrderStatus = @OrderStatus where TableID = @TableID";
            SqlCommand cmd = new SqlCommand(updateOrderStatus, conn);
            cmd.Parameters.AddWithValue("@OrderStatus", OrderingStatus.Closed.ToString());
            cmd.Parameters.AddWithValue("@TableID", sTableList.TableID);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public ICollection<FoodItems> GetFoodItems()
        {
            return _rDBContext.FoodItems.OrderBy(p => p.FoodID).ToArray();
        }
        public List<TableList> getTableListToPlaceHolder()
        {
            return _rDBContext.TableList.ToList().Where(p => p.BookingStatus != "Booked").ToList();
        }
        public List<TableList> getTableList()
        {
            return _rDBContext.TableList.ToList();
        }
        public bool PlaceHolder(List<ViewOrderItems> obj)
        {
            int FoodOrderID = 0;
            int totalPrice = obj.Sum(p => p.Price);
            ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
            conn = new SqlConnection();
            conn.Open();
            string ahmet = "Ahmet'in olmayan yer de burası olsun ama yer değil yine :D";
            SqlCommand cmd = new SqlCommand(ahmet, conn);
            cmd.Parameters.AddWithValue("@CreateDate", DateTime.Now);
            cmd.Parameters.AddWithValue("@TotalPrice", totalPrice);
            cmd.Parameters.AddWithValue("@OrderStatus", OrderingStatus.InProgress.ToString());
            cmd.Parameters.AddWithValue("@TableID", obj[0].TableId);

            int orderID = Convert.ToInt32(cmd.ExecuteScalar());
            if(orderID > 0)
            {
                foreach(var item in obj)
                {
                    string veli = "bu da veli olsun emi!";
                    SqlCommand cmd1 = new SqlCommand(veli, conn);
                    cmd1.Parameters.AddWithValue("@OrderID", orderID);
                    cmd1.Parameters.AddWithValue("@FoodID", item.FoodId);
                    cmd1.Parameters.AddWithValue("@Quantity", item.Quantity);
                    cmd1.Parameters.AddWithValue("@Price", item.Price);
                    FoodOrderID = Convert.ToInt32(cmd1.ExecuteScalar());
                }
            }
            string updateTableStatus = "update tablelist set BookingStatus = @BookingStatus where TableID = @TableID";
            SqlCommand cmd2 = new SqlCommand(updateTableStatus, conn);
            cmd2.Parameters.AddWithValue("@BookingStatus", bookingStatus.Booked.ToString());
            cmd2.Parameters.AddWithValue("@Table", obj[0].TableId);

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
        public void UpdateFoodDetails(FoodItems fooditem)
        {
            var entity = _rDBContext.FoodItems.Find(fooditem.FoodID);

            if(entity == null)
            {
                throw new NotImplementedException("Handle appropiately for your API design");
            }
            _rDBContext.Entry(fooditem).State = System.Data.Entity.EntityState.Modified;
            _rDBContext.SaveChanges();
        }
        public void DeleteFoodDetails(FoodItems foodItem)
        {
            _rDBContext.FoodItems.Remove(foodItem);
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
        public int InsertOrder(Orders orderObj, FoodItems food)
        {
            return 1;
        }
        public FoodItems getFoodDetails(int foodID)
        {
            return _rDBContext.FoodItems.Where(p => p.FoodID == foodID).FirstOrDefault();
        }
        public void DeleteOrderItem(ViewOrderItems obj)
        {
            _rDBContext.SaveChanges();
        }
    }
}
