using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_Application.Model
{
    class Orders
    {
        // add key 
        public int OrderID { get; set; }
        public int TableID { get; set; }
        [ForeignKey("TableID")]
        public virtual TableList TableList { get; set; }
        public string OrderStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public double TotalPrice { get; set; }
    }
}
