using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restaurant_Application.Model;

namespace Restaurant_Application.DB_Layer
{
    class DataAccessLayer
    {
        internal void getFoodDetails()
        {
            throw new NotImplementedException();
        }

        internal FoodItems getFoodDetails(int foodid)
        {
            throw new NotImplementedException();
        }

        internal bool PlaceHolder(List<ViewOrderItems> obj)
        {
            throw new NotImplementedException();
        }

        internal ObservableCollection<ViewOrderItems> getFoodOrderDetails(List<TableList> sTableList)
        {
            throw new NotImplementedException();
        }

        internal void UpdateOrderDetails(ViewOrderItems selectedOrderItem)
        {
            throw new NotImplementedException();
        }

        internal void UpdateTableStatus(List<TableList> sTableList)
        {
            throw new NotImplementedException();
        }

        internal ObservableCollection<ViewOrderItems> getFoodOrderDetails(TableList sTableList)
        {
            throw new NotImplementedException();
        }

        internal void UpdateTableStatus(TableList sTableList)
        {
            throw new NotImplementedException();
        }

        internal List<TableList> GetAllTableList()
        {
            throw new NotImplementedException();
        }
    }
}
