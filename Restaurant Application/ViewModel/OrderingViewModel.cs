using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restaurant_Application.Model;
using System.Collections.ObjectModel;
using Restaurant_Application.DB_Layer;
using Restaurant_Application.ActionEvents;
using System.Windows.Input;

namespace Restaurant_Application.ViewModel
{
    class OrderingViewModel : ObservableObject
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
            private set; // Why we use private, explain it!
        }
        private FoodItems _sFoodItem;
        public FoodItems SFoodList
        {
            get { return _sFoodItem; }
            set { _sFoodItem = value; }
        }
        private DataAccessLayer _dbLayerObj;
        private ViewOrderItems selectedOrderItems;
        public ViewOrderItems SelectedOrderItem
        {
            get { return selectedOrderItems; }
            set { selectedOrderItems = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("CanModify");
                NotifyPropertyChanged("CanNotModify");
            }
        }
        private TableList selectedTableList;
        private ObservableCollection<ViewOrderItems> foodOrderItems;

        public TableList SelectedTableList
        {
            get { return selectedTableList; }
            set { selectedTableList = value;
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
            getAvailableTableList();
            getAllTableList();
            getFoodList();
            foodOrderItems = new ObservableCollection<ViewOrderItems>();
        }
        private ObservableCollection<ViewOrderItems> _foodOrderItems;
        public ObservableCollection<ViewOrderItems> FoodOrderItems
        {
            get { return _foodOrderItems; }
            set { _foodOrderItems = value;
                NotifyPropertyChanged();
            }
        }

        private void getFoodList()
        {
            _dbLayerObj = new DataAccessLayer();
            FoodList = _dbLayerObj.GetFoodItems();
        }

        private void getAllTableList() // important, this is not hole list of table, just sorting of 'Booked' tables.
        {
            AllTableList = new List<Model.TableList>();
            _dbLayerObj = new DataAccessLayer();
            AllTableList = _dbLayerObj.getTableList().Where(p => p.BookingStatus == "Booked").ToList();
        }

        private void getAvailableTableList() // this shows empty tables to give new order 
        {
            TableList = new List<Model.TableList>();
            _dbLayerObj = new DataAccessLayer();
            TableList = _dbLayerObj.getTableListToPlaceHolder();
        }

        internal bool PlaceOrder(List<ViewOrderItems> myCart) 
        {
            throw new NotImplementedException(); // Bind with DB Layer
        }
        public ICommand GetFoodListCommand
        {
            get
            {
                return new ActionCommand(p => getFoodOrderItems());
            }
        }
        public void getFoodOrderItems()
        {
            foodOrderItems.Clear();
            SelectedOrderItem = null;
            _dbLayerObj = new DataAccessLayer();
            foodOrderItems = _dbLayerObj.getFoodOrderDetails(STableList); //Bind to DB Layer
        }
        public ICommand UpdateCommand
        {
            get
            {
                return new ActionCommand(p => UpdateFoodItem());
            }
        }
        public ICommand GenerateFoodBill
        {
            get
            {
                return new ActionCommand(p => GenerateBill());
            }
        }
        public void UpdateFoodItem()
        {
            if(selectedOrderItems != null || selectedOrderItems.Quantity > 0)
            {
                SelectedOrderItem.Price = SelectedOrderItem.Quantity * SelectedOrderItem.fPrice;
                _dbLayerObj.UpdateOrderDetails(SelectedOrderItem);
                getFoodOrderItems();
                Message = "Sipariş ürünü güncellemesi başarılı.";
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
                Message = STableList.TableName + " nolu masa rezervasyon için müsait.";
            }
        }
    }
}
