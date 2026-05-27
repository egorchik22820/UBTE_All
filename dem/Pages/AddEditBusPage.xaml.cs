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
using dem.AppData;

namespace dem.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddEditBusPage.xaml
    /// </summary>
    public partial class AddEditBusPage : Page
    {
        private Frame _frame;
        private Buses _bus;
        public AddEditBusPage(Frame frame, Buses bus)
        {
            InitializeComponent();
            _frame = frame;
            _bus = bus;

            LoadData();
        }

        private void LoadData()
        {
            var types = AppConnect.modelDB.BusTypes.ToList();

            foreach (var type in types)
            {
                ComboType.Items.Add(type);
            }

            if (_bus.BusID != 0)
            {
                NumberBox.Text = _bus.BusNumber;
                ComboType.SelectedItem = types.FirstOrDefault(x => x.BusTypeID == _bus.BusTypeID);
            }
            
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(NumberBox.Text) && !string.IsNullOrWhiteSpace(ComboType.Text))
            {
                _bus.BusNumber = NumberBox.Text;
                _bus.BusTypeID = AppConnect.modelDB.BusTypes.FirstOrDefault(x => x.TypeName == ComboType.Text).BusTypeID;

                if (_bus.BusID == 0)
                    AppConnect.modelDB.Buses.Add(_bus);

                AppConnect.modelDB.SaveChanges();
                _frame.Navigate(new Pages.BusesOutPage(_frame));
                
            }
            else
            {
                MessageBox.Show("Заполните необходимые данные.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            _frame.GoBack();
        }
    }
}
