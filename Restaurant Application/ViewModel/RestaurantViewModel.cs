using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restaurant_Application.Model;
using Restaurant_Application.Page_Screens;
using Restaurant_Application.ActionEvents;
using Restaurant_Application.DB_Layer;
using System.Windows.Input;
using System.Collections.ObjectModel;

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
                NotifyPropertyChanged();
            }
        }
        public ICollection<FoodItems> FoodItems
        {
            get;
            private set;
        }
        private List<TableList> TableList { get; set; }

        private FoodItems selectedFoodItem;

        private DataAccessLayer _dbLayerObj;

        public RestaurantViewModel() : this(new DataAccessLayer())
        {
            GetCustomerList();
            Message = "";
        }

        public RestaurantViewModel(DataAccessLayer _dbLayerObj)
        {
            FoodItems = new ObservableCollection<FoodItems>();
            this._dbLayerObj = _dbLayerObj;
        }

        public List<TableList> getTableList()
        {
            //TableList = _dbLayerObj.getTableList();
            return TableList;
        }

        public FoodItems SelectedFoodItem
        {
            get
            {
                return selectedFoodItem;
            }
            set
            {
                selectedFoodItem = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("CanModify");
                NotifyPropertyChanged("CanNotModify");
            }
        }
        public bool CanNotModify
        {
            get
            {
                return SelectedFoodItem == null;
            }
        }
        public bool CanModify
        {
            get
            {
                return SelectedFoodItem != null;
            }
        }
    }
}
