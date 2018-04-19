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
        internal ObservableCollection<ViewOrderItems> getFoodOrderDetails(TableList sTableList)
        {
            throw new NotImplementedException();
        }

        public void UpdateOrderDetails(ViewOrderItems fooditem)
        {
            /*
            ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
            conn = new SqlConnection(ConnectionString);
            conn.Open();
            string ahmet = "Ahmet'in query'leri olsun burası da onun menkansız mekanları olsun ama mekan değil buralar. Ama çöplük değil, tamam mı? :D Zaaaaaaafdasgfasdgasgasdg ASdgfasdglşaslş";
            SqlConnection cmd = new SqlConnection(ahmet, conn);
            cmd.Parameter.Add
            */
        }

        internal void UpdateTableStatus(TableList sTableList)
        {
            throw new NotImplementedException();
        }

        internal ICollection<FoodItems> GetFoodItems()
        {
            throw new NotImplementedException();
        }

        internal List<TableList> getTableListToPlaceHolder()
        {
            return _rDBContext.TableList.ToList().Where(p => p.BookingStatus != "Booked").ToList();
        }

        internal List<TableList> getTableList()
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
            cmd.Parameters.AddWithValue("@TableID", obj[0].TableID);

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
    }
}
