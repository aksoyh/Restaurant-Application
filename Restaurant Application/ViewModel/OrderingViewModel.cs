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
    public class OrderingViewModel : ObservableObject
    {
        private string status;
        private int _GST;
        private int _totalprice;
        public int GST
        {
            get { return _GST; }
            set { _GST = value; NotifyPropertyChanged(); }
        }
        public int TotalPrice
        {
            get { return _totalprice; }
            set { _totalprice = value; NotifyPropertyChanged(); }
        }
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
        private List<TableList> _table;
        public List<TableList> Tablelist
        {
            get { return _table; }
            set { _table = value; }
        }
        private List<TableList> _alltable;
        public List<TableList> AllTablelist
        {
            get { return _alltable; }
            set { _alltable = value; }
        }
        private TableList _stablelist;
        public TableList STableList
        {
            get { return _stablelist; }
            set { _stablelist = value; NotifyPropertyChanged(); }
        }
        public ICollection<FoodItems> Foodlist
        {
            get;
            private set;
        }
        private FoodItems _sFoodItem;
        public FoodItems SFoodList
        {
            get { return _sFoodItem; }
            set { _sFoodItem = value; NotifyPropertyChanged(); }
        }
        private DataAccessLayer _dbLayerObj;
        private ViewOrderItems selectedOrderItem;

        public ViewOrderItems SelectedOrderItem
        {
            get
            {
                return selectedOrderItem;
            }
            set
            {
                selectedOrderItem = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("CanModify");
                NotifyPropertyChanged("CanNotModify");
            }
        }
        private TableList selectedTable;
        public TableList SelectedTable
        {
            get
            {
                return selectedTable;
            }
            set
            {
                selectedTable = value;
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
            this._dbLayerObj = _dbLayerObj;
        }

        public void getAllTableList()
        {
            AllTablelist = new List<TableList>();
            _dbLayerObj = new DataAccessLayer();
            AllTablelist = _dbLayerObj.getTableList().Where(p => p.BookingStatus == "Booked").ToList();
        }

        public void getAvailableTableList()
        {
            Tablelist = new List<TableList>();
            _dbLayerObj = new DataAccessLayer();
            Tablelist = _dbLayerObj.getTableListToPlaceOrder();
        }

        public void getFoodList()
        {
            _dbLayerObj = new DataAccessLayer();
            Foodlist = _dbLayerObj.GetFoodItems();
        }

        public FoodItems getFoodDetail(int foodid)
        {
            return _dbLayerObj.getFoodDetails(foodid);
        }

        private ObservableCollection<ViewOrderItems> _foodOrderItems;
        public ObservableCollection<ViewOrderItems> foodOrderItems
        {
            get
            {
                return _foodOrderItems;
            }
            set
            {
                _foodOrderItems = value;
                NotifyPropertyChanged();
            }
        }

        public bool PlaceOrder(List<ViewOrderItems> Obj)
        {
            _dbLayerObj = new DataAccessLayer();
            return _dbLayerObj.PlaceOrder(Obj);
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
            foodOrderItems = _dbLayerObj.getFoodOrderDetails(STableList);
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

        private void UpdateFoodItem()
        {
            if (selectedOrderItem != null || selectedOrderItem.Quantity > 0)
            {
                SelectedOrderItem.Price = SelectedOrderItem.Quantity * SelectedOrderItem.fPrice;
                _dbLayerObj.UpdateOrderDetails(SelectedOrderItem);
                getFoodOrderItems();
                Message = "Sipariş ürünü güncellendi.";

            }
        }

        private void GenerateBill()
        {
            if (STableList != null)
            {

                foodOrderItems = _dbLayerObj.getFoodOrderDetails(STableList);

                GST = (foodOrderItems.Sum(p => p.Price) * 6) / 100;
                TotalPrice = foodOrderItems.Sum(p => p.Price) + GST;
                _dbLayerObj.UpdateTableStatus(STableList);
                Message = STableList.TableName + " nolu masa müsait";
            }
        }
    }
}
