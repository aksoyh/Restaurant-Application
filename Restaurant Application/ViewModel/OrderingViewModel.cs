using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restaurant_Application.Model;
using Restaurant_Application.ActionEvents;
using Restaurant_Application.DB_Layer;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Restaurant_Application.ViewModel
{
    class OrderingViewModel:ObservableObject
    {
        private string status;
        private int _Tip;
        private int _totalPrice;
        public int Tip
        {
            get { return _Tip; }
            set { _Tip = value; NotifyPropertyChanged(); } 
        }
        public int TotalPrice
        {
            get { return _totalPrice; }
            set { _totalPrice = value; NotifyPropertyChanged(); } 
        }
        public string Message
        {
            get { return status; }
            set { status = value; NotifyPropertyChanged(); } 
        }
        private List<TableList> _table;
        public List<TableList> TableList
        {
            get { return _table; }
            set { _table = value; }
        }
        private List<TableList> _allTable;
        public List<TableList> AllTableList
        {
            get { return _allTable; }
            set { _allTable = value; }
        }
        private TableList _sTableList;
        public TableList STableList
        {
            get { return _sTableList; }
            set { _sTableList = value; NotifyPropertyChanged(); } 
        }
        public ICollection<FoodItems> FoodList
        {
            get;
            private set; // Why we use private, explain it
        }
        private FoodItems _sfoodItems;
        public FoodItems SFoodList
        {
            get { return _sfoodItems; }
            set { _sfoodItems = value; NotifyPropertyChanged(); } 
        }
        private DataAccessLayer _dbLayerObj;
        private ViewOrderItems selectedOrderItems;
        public ViewOrderItems SelectedOrderItems
        {
            get { return selectedOrderItems; }
            set { selectedOrderItems = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("CanModify");
                NotifyPropertyChanged("CanNotModify");
            }
        }
        private TableList selectedTable;
        public TableList SelectedTable
        {
            get { return selectedTable; }
            set { selectedTable = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("CanModify");
                NotifyPropertyChanged("CanNotModify");
            }
        }
        public OrderingViewModel() : this(new DataAccessLayer())
        {
            Message = "";
        }
        public OrderingViewModel(DataAccessLayer _dbLayerObj)
        {
            foodOrderItems = new ObservableCollection<ViewOrderItems>();
            getAvailableTableList();
            getAllTableList();
            getFoodList();
        }

        private void getAvailableTableList()
        {
            TableList = new List<TableList>();
            _dbLayerObj = new DataAccessLayer();
            TableList = _dbLayerObj.getTableListToPlaceHolder();
        }

        private void getAllTableList()
        {
            TableList = new List<Model.TableList>();
            _dbLayerObj = new DataAccessLayer();
            AllTableList = _dbLayerObj.getTableList().Where(p => p.BookingStatus == "Booked").ToList();
        }

        private void getFoodList()
        {
            _dbLayerObj = new DataAccessLayer();
            FoodList = _dbLayerObj.getFoodItems();
        }

        private ObservableCollection<ViewOrderItems> _foodOrderItems;
        public ObservableCollection<ViewOrderItems> foodOrderItems
        {
            get { return _foodOrderItems; }
            set { _foodOrderItems = value; NotifyPropertyChanged(); }
        }
        internal bool PlaceOrder(List<ViewOrderItems> myCart)
        {
            _dbLayerObj = new DataAccessLayer();
            return _dbLayerObj.PlaceOrder(myCart);
        }
        public ICommand GetFoodListCommand
        {
            get { return new ActionCommand(p => getFoodOrderItems()); }
        }
        public void getFoodOrderItems()
        {
            foodOrderItems.Clear();
            SelectedOrderItems = null;
            _dbLayerObj = new DataAccessLayer();
            foodOrderItems = _dbLayerObj.getFoodOrderDetails(STableList);
        }
        public ICommand UpdateCommand
        {
            get { return new ActionCommand(p => UpdateFoodItem()); }
        }
        public void UpdateFoodItem()
        {
            if(selectedOrderItems != null || selectedOrderItems.Quantity > 0)
            {
                SelectedOrderItems.Price = SelectedOrderItems.Quantity * SelectedOrderItems.fPrice;
                _dbLayerObj.UpdateOrderDetails(SelectedOrderItems);
                getFoodOrderItems();
                Message = "Sipariş başarı ile güncellendi.";
            }
        }
        public void GenerateBill()
        {
            if(STableList != null)
            {
                foodOrderItems = _dbLayerObj.getFoodOrderDetails(STableList);
                Tip = (foodOrderItems.Sum(p => p.Price) * 10) / 100;
                TotalPrice = foodOrderItems.Sum(p => p.Price) + Tip;
                _dbLayerObj.UpdateTableStatus(STableList);
                Message = STableList.TableName + " nolu masa kullanılabilir.";
            }
        }
    }
}
