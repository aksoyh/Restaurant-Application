using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_Application.Model
{
    class ViewOrderItems
    {
        public int TableID { get; set; }
        public string TableName{ get; set; }
        public int FoodID { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public string FoodName { get; set; }
    }
}
