using Restaurant_Application.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_Application.DB_Layer
{
    class DataAccessLayer
    {
        internal IEnumerable<object> GetFoodItems()
        {
            throw new NotImplementedException();
        }
        public ICollection<FoodItems> GetFoodItems()
        {
            return _rDBContext.FoodItems.OrderBy(p => p.FoodID).ToArray();
        }
    }
}
