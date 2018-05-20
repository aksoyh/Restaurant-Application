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
        private OrderingViewModel _oVM;
        private FoodOrders obj;
        private FoodItems fooditemdata;
        private ViewOrderItems _vOrderItems;
        private List<ViewOrderItems> mycart;

        public NewOrderPlace()
        {
            InitializeComponent();
            DataContext = new OrderingViewModel();
            mycart = new List<ViewOrderItems>();
        }

        private void AddItem_Click(object sender, RoutedEventArgs e)
        {
            if (fooditem.SelectedValue != null || tableitem.SelectedValue != null || Quantitytxt.Text != null)
            {
                _vOrderItems = new ViewOrderItems();
                _oVM = new OrderingViewModel();
                fooditemdata = _oVM.getFoodDetail(Convert.ToInt32(fooditem.SelectedValue));
                _vOrderItems.Quantity = Convert.ToInt32(Quantitytxt.Text);
                _vOrderItems.Price = fooditemdata.fPrice * _vOrderItems.Quantity;
                _vOrderItems.FoodID = fooditemdata.FoodID;
                _vOrderItems.FoodName = fooditemdata.FoodName;
                _vOrderItems.TableID = Convert.ToInt32(tableitem.SelectedValue);
                mycart.Add(_vOrderItems);
                fooditemsgrid.ItemsSource = mycart;
                fooditemsgrid.Items.Refresh();
                status.Content = "Ürün listeye eklendi";
            }
            else
            {
                status.Foreground = Brushes.Red;
                status.Content = "Tüm alanlar zorunludur";
                status.Foreground = Brushes.Green;
            }

        }

        private void PlaceOrder_Click(object sender, RoutedEventArgs e)
        {
            if (fooditemsgrid.ItemsSource != null)
            {
                _oVM = new OrderingViewModel();
                bool confirm = _oVM.PlaceOrder(mycart);
                if (confirm)
                {
                    DataContext = new OrderingViewModel();
                    mycart.Clear();
                    fooditemsgrid.ItemsSource = mycart;
                    fooditemsgrid.Items.Refresh();
                    status.Content = "Sipariş verildi.";
                }
                else
                {
                    status.Content = "Birşeyler yanlış gitti.";
                }

            }
            else
            {
                status.Content = "Listeye herhangi bir ürün eklenmedi.";
            }

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            List<ViewOrderItems> temp = new List<ViewOrderItems>();

            int selecteditem = Convert.ToInt32(fooditemsgrid.SelectedValue);
            temp = mycart.Where(p => p.FoodID != selecteditem).ToList();
            mycart.Clear();
            foreach (var v in temp)
            {
                mycart.Add(v);
            }
            fooditemsgrid.ItemsSource = mycart;
            fooditemsgrid.Items.Refresh();
            status.Content = "Ürün listeden silindi.";

        }
    }
}
