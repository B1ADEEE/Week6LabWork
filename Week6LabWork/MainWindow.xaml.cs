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

namespace Week6LabWork
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       Entities1 db = new Entities1();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            StockLvLLBOX.ItemsSource = Enum.GetNames(typeof(StockLevel));
            var query1 = from s in db.Suppliers
                         orderby s.CompanyName
                         select new
                         {
                             SupplierName = s.CompanyName,
                             SupplierID = s.SupplierID,
                             Country = s.Country
                         };
            SuppliersLBOX.ItemsSource = query1.ToList();
            var query2 = query1
                .OrderBy(s => s.Country)
                .Select(s => s.Country);
            var Countries = query2.ToList();
            CountryLBOX.ItemsSource = Countries.Distinct();
        }
        public enum StockLevel { Low, Normal, Overstocked };

        private void StockLvLLBOX_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var query = from p in db.Products
                        where p.UnitsInStock < 50
                        orderby p.ProductName
                        select p.ProductName;
            string selected = StockLvLLBOX.SelectedItem as string;
            switch(selected)
            {
                case "Low":
                    break;
                case "Normal":
                    query = from p in db.Products
                            where p.UnitsInStock >= 50 && p.UnitsInStock <= 100
                            orderby p.ProductName
                            select p.ProductName;
                    break;
                case "Overstocked":
                    query = from p in db.Products
                            where p.UnitsInStock > 100
                            orderby p.ProductName
                            select p.ProductName;
                    break;
            }
            ProductsLBOX.ItemsSource = query.ToList();
        }

        private void SuppliersLBOX_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int supplierID = Convert.ToInt32(SuppliersLBOX.SelectedValue);
            var query = from p in db.Products
                        where p.SupplierID == supplierID
                        orderby p.ProductName
                        select p.ProductName;
            ProductsLBOX.ItemsSource = query.ToList();        
        }

        private void CountryLBOX_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string country = (string)(CountryLBOX.SelectedValue);
            var query = from p in db.Products
                        where p.Supplier.Country == country
                        orderby p.ProductName
                        select p.ProductName;
            ProductsLBOX.ItemsSource = query.ToList();
        }
    }
    
}
