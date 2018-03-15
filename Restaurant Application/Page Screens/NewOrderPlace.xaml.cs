using Restaurant_Application.Model;
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
        private FoodItems _fItems;

        public NewOrderPlace()
        {
            InitializeComponent();
        }

        private void AddItem_Click(object sender, RoutedEventArgs e)
        {
            _vOrderItems = new ViewOrderItems();
            _fItems = new FoodItems(); // Quantity çağıralacak FoodItems ile eşleştirilecek
            _fItems.

            Masa No
Adet
Yemek Adı

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PlaceOrder_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
