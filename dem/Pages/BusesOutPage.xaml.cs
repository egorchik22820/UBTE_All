using dem.AppData;
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

namespace dem.Pages
{
    /// <summary>
    /// Логика взаимодействия для BusesOutPage.xaml
    /// </summary>
    public partial class BusesOutPage : Page
    {
        private Frame _frame;

        private List<Buses> _listBuses;

        public BusesOutPage(Frame frame)
        {
            InitializeComponent();
            _frame = frame;

            _listBuses = new List<Buses>();

            LoadData();

            listBuses.ItemsSource = _listBuses;
        }

        private void LoadData()
        {
            var buses = AppConnect.modelDB.Buses.ToList();

            foreach (var bus in buses)
            {
                _listBuses.Add(bus);
            }
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            _frame.Navigate(new AddEditBusPage(_frame, new Buses()));
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RoutesBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void listBuses_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selected = listBuses.SelectedItem as Buses;
            _frame.Navigate(new AddEditBusPage(_frame, selected));
        }
    }
}
