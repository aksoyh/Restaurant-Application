using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_Application.Model
{
    public class FoodOrders
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FoodOrderID { get; set; }
        public int OrderID { get; set; }
        [ForeignKey("OrderID")]
        public virtual Orders Orders { get; set; }
        public int FoodID { get; set; }
        [ForeignKey("FoodID")]
        public virtual FoodItems fooditems { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
    }
}
