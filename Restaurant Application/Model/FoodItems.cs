using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_Application.Model
{
    public class FoodItems
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FoodID { get; set; }
        public string FoodName { get; set; }
        public string Description { get; set; }
        public int fPrice { get; set; }
    }
}
