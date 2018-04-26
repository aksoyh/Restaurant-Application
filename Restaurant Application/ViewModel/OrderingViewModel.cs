using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restaurant_Application.Model;
using Restaurant_Application.DB_Layer;
using Restaurant_Application.ActionEvents;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Restaurant_Application.ViewModel
{
    class OrderingViewModel : ObservableObject
    {
        internal bool PlaceOrder(List<ViewOrderItems> myCart)
        {
            throw new NotImplementedException();
        }

        private string status;
        private int _totalPrice;
        private int _Tip;
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

        public ICollection<FoodItems> Foodlist
        {
            get;
            private set; // Explain later
        }

        private List<TableList> _table;
        public List<TableList> TableList
        {
            get { return _table; }
            set { _table = value; }
        }
        private TableList _sTable;
        public TableList STableList
        {
            get { return _sTable; }
            set { _sTable = value; NotifyPropertyChanged(); }
        }
        private List<TableList> _allTables;
        public List<TableList> AllTableList
        {
            get { return _allTables; }
            set { _allTables = value; }
        }
        private List<FoodItems> _food;
        public List<FoodItems> FoodItems
        {
            get { return _food; }
            set { _food = value; NotifyPropertyChanged(); } 
        }
        private FoodItems _sFoodItem;
        public FoodItems sFoodItem
        {
            get { return _sFoodItem; }
            set { _sFoodItem = value; NotifyPropertyChanged(); }
        }
        private ViewOrderItems selectedOrderItem;
        public ViewOrderItems SelectedViewOrderItem
        {
            get { return selectedOrderItem; }
            set { selectedOrderItem = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("CanModify");
                NotifyPropertyChanged("CannotModify");
            }
        }
        private TableList selectedTable;
        private DataAccessLayer _dbLayerObj;

        public TableList SelectedTable
        {
            get { return selectedTable; }
            set { selectedTable = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("CanModify");
                NotifyPropertyChanged("CannotModify");
            }
        }
        public OrderingViewModel():this(new DataAccessLayer())
        {
            Message = "";
            // We can add later, get customer list
            // We can add also later, food order items
        }

        public OrderingViewModel(DataAccessLayer _dbLayerObj)
        {
            getAvailableTableList();
            getAllTableList();
            getFoodList();
            foodOrderItems = new ObservableCollection<ViewOrderItems>();
            // we can also do this for food items
            this._dbLayerObj = _dbLayerObj;
        }

        private void getFoodList()
        {
            _dbLayerObj = new DataAccessLayer();
            Foodlist = _dbLayerObj.GetFoodItems(); // DB işlemleri
        }

        private void getAllTableList()
        {
            AllTableList = new List<TableList>();
            _dbLayerObj = new DataAccessLayer();
            AllTableList = _dbLayerObj.getTableListToPlaceHolder();
        }

        private void getAvailableTableList()
        {
            TableList = new List<TableList>();
            _dbLayerObj = new DataAccessLayer();
            TableList = _dbLayerObj.GetAllTableList(); // DB işlemleri
        }

        private FoodItems getFoodDetail(int foodid)
        {
            return _dbLayerObj.getFoodDetails(foodid);
        }
        private ObservableCollection<ViewOrderItems> _foodOrderItems;
        public ObservableCollection<ViewOrderItems> foodOrderItems
        {
            get { return _foodOrderItems; }
            set
            {
                _foodOrderItems = value;
                NotifyPropertyChanged();
            }
        }
        public bool PlaceHolder(List<ViewOrderItems> Obj)
        {
            _dbLayerObj = new DataAccessLayer();
            return _dbLayerObj.PlaceHolder(Obj);
        }
        public void getFoodOrderItems()
        {
            foodOrderItems.Clear();
            selectedOrderItem = null;
            _dbLayerObj = new DataAccessLayer();
            foodOrderItems = _dbLayerObj.getFoodOrderDetails(STableList);
            // if we can decide this method not void, we can must return foodOrderItems from DataAccessLayer.
        }
        private void UpdateFoodItem()
        {
            selectedOrderItem.Price = selectedOrderItem.Quantity * selectedOrderItem.fPrice;
            _dbLayerObj.UpdateOrderDetails(selectedOrderItem);
            getFoodOrderItems();
            Message = "Sipariş başarı ile güncellendi!";
        }
        private void GenerateBill()
        {
            foodOrderItems = _dbLayerObj.getFoodOrderDetails(STableList);
            Tip = (foodOrderItems.Sum(p => p.Price) * 10) / 100;
            TotalPrice = foodOrderItems.Sum(p => p.Price) + Tip; // We can modify this line with discount.
            _dbLayerObj.UpdateTableStatus(STableList);
            Message = STableList.TableName + " nolu masa kullanılabilir.";
        }
        public ICommand UpdateCommand
        {
            get { return new ActionCommand(p => UpdateFoodItem()); }
        }
        public ICommand GenerateFoodBill
        {
            get { return new ActionCommand(p => GenerateBill()); }
        }
    }
}
