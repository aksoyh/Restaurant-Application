using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_Application.Model
{
    class ViewOrderItems
    {
        public int FoodOrderId { get; set; }
        public int OrderId { get; set; }
        public int FoodId { get; set; }
        public int TableId { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public int fPrice { get; set; }
        public String FoodName { get; set; }
        public String OrderStatus { get; set; }
        public String BookingStatus { get; set; }
        public String  TableName { get; set; }
        public DateTime OrderCreatedDate { get; set; }
       

    }
}
