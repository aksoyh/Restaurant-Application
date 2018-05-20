using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_Application.Model
{
    public class ViewOrderItems
    {
        public int FoodOrderID { get; set; }
        public int OrderID { get; set; }
        public int FoodID { get; set; }
        public int TableID { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public string FoodName { get; set; }
        public string OrderStatus { get; set; }
        public DateTime OrderCreatedDate { get; set; }
        public string BookingStatus { get; set; }
        public string TableName { get; set; }
        public int fPrice { get; set; }
    }
}
