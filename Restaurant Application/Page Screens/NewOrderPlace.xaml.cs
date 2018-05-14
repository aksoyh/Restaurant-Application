using Restaurant_Application.Model;
using Restaurant_Application.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace Restaurant_Application.Page_Screens
{
    /// <summary>
    /// Interaction logic for NewOrderPlace.xaml
    /// </summary>
    public partial class NewOrderPlace : Page
    {
        private ViewOrderItems _vOrderItems;
        private FoodOrders obj;
        private FoodItems fooditemsdata;
        private OrderingViewModel _oVM;
        private List<ViewOrderItems> myCart;
        //Listeleme tanımlanacak
        public NewOrderPlace()
        {
            InitializeComponent();
            DataContext = new OrderingViewModel();
            myCart = new List<ViewOrderItems>();
        }

        private void AddItem_Click(object sender, RoutedEventArgs e)
        {
            if (Quantitytxt.Text != "" || fooditem.SelectedValue!=null || tableitem.SelectedValue!=null)
            {
                
            _vOrderItems = new ViewOrderItems();
            _oVM = new OrderingViewModel();

            //fooditemsdata = _oVM.getFoodItems(); //get food detail

            _vOrderItems.FoodId = fooditemsdata.FoodID;
            _vOrderItems.FoodName = fooditemsdata.FoodName;
            _vOrderItems.Quantity = Convert.ToInt32(Quantitytxt.Text);
            _vOrderItems.TableId = Convert.ToInt32 (tableitem.SelectedItem);
            _vOrderItems.Price = fooditemsdata.fPrice * _vOrderItems.Quantity;

            myCart.Add(_vOrderItems);
            fooditemsgrid.ItemsSource = myCart;
            fooditemsgrid.Items.Refresh(); //veri ekledikçe data grid güncelleme işlemi
            status.Content = "Ürün Eklendi";

            }
            else
            {
                status.Foreground = Brushes.Red;
                status.Content = "Tüm alanlar zorunludur.";
                status.Foreground = Brushes.Green;
            }

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            List<ViewOrderItems> temp = new List<ViewOrderItems>();
            int selectedItem = Convert.ToInt32(fooditemsgrid.SelectedValue);
            temp = myCart.Where(p => p.FoodId != selectedItem).ToList();
            myCart.Clear();
            foreach(var v in temp)
            {
                myCart.Add(v);
            }
            fooditemsgrid.ItemsSource = myCart;
            fooditemsgrid.Items.Refresh();
            status.Content = "Ürün Listeden Silindi";
        }

        private void PlaceOrder_Click(object sender, RoutedEventArgs e)
        {
            if(fooditemsgrid.ItemsSource != null)
            {
                _oVM = new OrderingViewModel();
                bool confirm = _oVM.PlaceOrder(myCart);
                if (confirm)
                {
                    DataContext = new OrderingViewModel();
                    myCart.Clear();
                    fooditemsgrid.ItemsSource = myCart;
                    fooditemsgrid.Items.Refresh();
                }
                else
                {
                    status.Foreground = Brushes.Red;
                    status.Content = "Yanlış giden bir şeyler var.";
                }
            }
            else
            {
                status.Content = "Herhangi bir ürün eklenemedi.";
            }
        }
    }
}
