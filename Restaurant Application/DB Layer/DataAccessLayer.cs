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
            throw new NotImplementedException();
        }

        public void UpdateOrderDetails(ViewOrderItems selectedOrderItems)
        {
            throw new NotImplementedException();
        }

        public void UpdateTableStatus(TableList sTableList)
        {
            throw new NotImplementedException();
        }
    }
}
