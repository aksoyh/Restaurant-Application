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
        private FoodItems fooditemsdata;
        private OrderingViewModel _oVM;
        private List<ViewOrderItems> myCart;

        // FoodOrder Model eklenecek

        public NewOrderPlace()
        {
            InitializeComponent();
            myCart = new List<ViewOrderItems>();
            // ViewModel Entegrasyonu yapılacak
        }

        private void AddItem_Click(object sender, RoutedEventArgs e)
        {
            if (fooditem.SelectedValue != null || tableitem.SelectedValue != null || Quantitytxt.Text != null)
            {
                _vOrderItems = new ViewOrderItems();
                _oVM = new OrderingViewModel();
                //fooditemsdata = _oVM.getFoodDetail() // getfooddetail
                _vOrderItems.Quantity = Convert.ToInt32(Quantitytxt.Text);
                _vOrderItems.Price = fooditemsdata.fPrice * _vOrderItems.Quantity;// ücret hesaplanır
                _vOrderItems.TableID = Convert.ToInt32(tableitem.SelectedItem);
                _vOrderItems.FoodID = fooditemsdata.FoodID;
                _vOrderItems.FoodName = fooditemsdata.FoodName;
                myCart.Add(_vOrderItems);
                fooditemsgrid.ItemsSource = myCart;
                fooditemsgrid.Items.Refresh();
                status.Content = "Ürün eklendi.";
            }
            else
            {
                status.Content = "Tüm alanlar zorunlu doldurulmalıdır";
            }                         
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            List<ViewOrderItems> temp = new List<ViewOrderItems>();
            int selectedItem = Convert.ToInt32(fooditemsgrid.SelectedValue);
            temp = myCart.Where(p => p.FoodID != selectedItem).ToList();
            // temp'den seçilen item kaldırılabilir
            myCart.Clear();
            foreach(var v in temp)
            {
                myCart.Add(v);
            }
            fooditemsgrid.ItemsSource = myCart;
            fooditemsgrid.Items.Refresh();
            status.Content = "Ürünler listeden silindi";

        }

        private void PlaceOrder_Click(object sender, RoutedEventArgs e)
        {
            _oVM = new OrderingViewModel();
            bool confirm = _oVM.PlaceOrder(myCart);
            if (confirm)
            {
                DataContext = new OrderingViewModel();
                myCart.Clear();
                fooditemsgrid.ItemsSource = myCart;
                fooditemsgrid.Items.Refresh();
                status.Content = "Sipariş Alındı.";
            }
            else
            {
                status.Content = "Yanlış giden bir şeyler var";
            }
        }
    }
}
