using Restaurant_Application.Page_Screens;
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

namespace Restaurant_Application
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void pageload(Page pageToLoad)
        {
            MainFrame.NavigationUIVisibility = NavigationUIVisibility.Hidden;
            MainFrame.Content = pageToLoad;
        }

        private void AddItems_Click(object sender, RoutedEventArgs e)
        {
            AddFoodItems fooditemscreen = new AddFoodItems();
            pageload(fooditemscreen);
        }

        private void PlaceNewOrder_Click(object sender, RoutedEventArgs e)
        {
            NewOrderPlace neworderplace = new NewOrderPlace();
            pageload(neworderplace);
        }

        private void UpdateOrder_Click(object sender, RoutedEventArgs e)
        {
            UpdateOrders updateorder = new UpdateOrders();
            pageload(updateorder);
        }

        private void GenerateBill_Click(object sender, RoutedEventArgs e)
        {
            GenerateBill generatebill = new GenerateBill();
            pageload(generatebill);
        }
        
    }
}
