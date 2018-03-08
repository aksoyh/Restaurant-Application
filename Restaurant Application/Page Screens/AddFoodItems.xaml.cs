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
    /// Interaction logic for AddFoodItems.xaml
    /// </summary>
    public partial class AddFoodItems : Page
    {
        public AddFoodItems()
        {

            InitializeComponent();
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            foodnametxt.Clear();
            descriptiontxt.Clear();
            pricetxt.Clear();
        }

        private void AddItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (foodnametxt.Text == "" || pricetxt.Text.ToString() == "" || descriptiontxt.Text == "")
                {
                    ///status.Foreground = Brush.Red;
                    status.Content = "Tüm alanlar zorunludur";
                }
                else
                {
                    FoodItems fooditem = new FoodItems();
                    fooditem.FoodName = foodnametxt.Text;
                    fooditem.Description = descriptiontxt.Text;
                    fooditem.fPrice = Convert.ToInt32(pricetxt.Text.ToString());
                }
            }
            catch
            {
                status.Content = "Lütfen doğru değerler giriniz.";
            }                                    
        }
    }
}
