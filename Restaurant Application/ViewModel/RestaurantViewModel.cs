using Restaurant_Application.DB_Layer;
using Restaurant_Application.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_Application.ViewModel
{
    class RestaurantViewModel // Add Action Events later
    {
        private string status;
        public string Message
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
                //NotifyPropertyChanged(); // Action Event
            }
        }
        public ICollection<FoodItems> FoodItems
        {
            get;
            private set;
        }
        //private List<TableList> TableList { get; set; } // Model

        private FoodItems selectedFoodItem;

        private DataAccessLayer _dbLayerObj; // Database

        public RestaurantViewModel() : this (new DataAccessLayer())
        {
            GetCustomerList();
            Message = "";
        }

        private void GetCustomerList()
        {
            FoodItems.Clear();
            selectedFoodItem = null;

            foreach (var fooditem in _dbLayerObj.GetFoodItems())
            {
                FoodItems.Add(fooditem);
            }
        }
        public RestaurantViewModel(DataAccessLayer _dbLayerObj)
        {
            FoodItems = new ObservableCollection<FoodItems>();
            this._dbLayerObj = _dbLayerObj;
        }
    }
}
