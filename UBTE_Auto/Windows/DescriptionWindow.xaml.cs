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
using System.Windows.Shapes;
using UBTE_Auto.AppData.DTO;

namespace UBTE_Auto.Windows
{
    /// <summary>
    /// Логика взаимодействия для DescriptionWindow.xaml
    /// </summary>
    public partial class DescriptionWindow : Window
    {
        private ProgramDTO _program;
        public DescriptionWindow(ProgramDTO program)
        {
            InitializeComponent();

            _program = program;

            LoadInfo();
        }

        private void LoadInfo()
        {
            NameBlock.Text = _program.Name;
            VersionBlock.Text = _program.Version;
            DescriptionBlock.Text = _program.Description;
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
