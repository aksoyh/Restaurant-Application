using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restaurant_Application.Model;
using System.Data.SqlClient;
using System.Collections.ObjectModel;

namespace Restaurant_Application.DB_Layer
{
    class DataAccessLayer
    {
        internal List<TableList> getTableListPlaceHolder()
        {
            throw new NotImplementedException();
        }

        internal object getTableList()
        {
            throw new NotImplementedException();
        }

        internal ICollection<FoodItems> getFoodItems()
        {
            throw new NotImplementedException();
        }

        internal bool PlaceOrder(object obj)
        {
            throw new NotImplementedException();
        }

        internal ObservableCollection<ViewOrderItems> getFoodOrderDetails(TableList sTableList)
        {
            throw new NotImplementedException();
        }

        internal void UpdateOrderDetails(ViewOrderItems selectedOrderItems)
        {
            throw new NotImplementedException();
        }

        internal void UpdateTableStatus(TableList sTableList)
        {
            throw new NotImplementedException();
        }
    }
}
