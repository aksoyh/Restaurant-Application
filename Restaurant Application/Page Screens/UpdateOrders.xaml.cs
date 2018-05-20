
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
    /// Interaction logic for UpdateOrders.xaml
    /// </summary>
    public partial class UpdateOrders : Page
    {
        private OrderingViewModel _oVM;

        public UpdateOrders()
        {
            InitializeComponent();
            DataContext = new OrderingViewModel();
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _oVM = new OrderingViewModel();
            _oVM.getFoodOrderItems();
            MessageBox.Show(tablelistcombo.SelectedValue.ToString());
        }
    }
}
