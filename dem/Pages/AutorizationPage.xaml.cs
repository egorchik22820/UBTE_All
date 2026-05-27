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
    /// Логика взаимодействия для AutorizationPage.xaml
    /// </summary>
    public partial class AutorizationPage : Page
    {
        private Frame _frame;
        public AutorizationPage(Frame frame)
        {
            InitializeComponent();
            _frame = frame;

        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            var login = LoginBox.Text;
            var password = PasswordBox.Password;

            AppConnect.curUser = AppConnect.modelDB.Users.FirstOrDefault(x => x.Username == login && x.PasswordHash == password);

            if (AppConnect.curUser != null)
                _frame.Navigate(new Pages.BusesOutPage(_frame));
            else
            {
                MessageBox.Show("Пользователь незарегестрирован.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
        }

        private void AsGuestTxtBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {;
            _frame.Navigate(new Pages.BusesOutPage(_frame));
        }
    }
}
