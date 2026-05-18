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

namespace UBTE_Auto
{
    /// <summary>
    /// Логика взаимодействия для testWindow2.xaml
    /// </summary>
    public partial class testWindow2 : Window
    {
        private List<ProgramDTO> programs = new List<ProgramDTO>();
        public testWindow2()
        {
            InitializeComponent();

            LoadPrograms();
            listProg.ItemsSource = programs;

        }

        private void LoadPrograms()
        {
            programs.Clear();

            programs.Add(new ProgramDTO("test1", "description1", "v0.0.0.0.0"));
            programs.Add(new ProgramDTO("test1", "description1", "v0.0.0.0.0"));
            programs.Add(new ProgramDTO("test1", "description1", "v0.0.0.0.0"));
            programs.Add(new ProgramDTO("test1", "description1", "v0.0.0.0.0"));
            programs.Add(new ProgramDTO("test1", "description1", "v0.0.0.0.0"));
            programs.Add(new ProgramDTO("test2", "descriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescription", "v0.0.0.0.0"));
            programs.Add(new ProgramDTO("test3", "description1", "v0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0"));
            programs.Add(new ProgramDTO("testtesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttest", "description1", "v0.0.0.0.0"));
            programs.Add(new ProgramDTO("test1", "description1", "v0.0.0.0.0"));
            programs.Add(new ProgramDTO("test1", "description1", "v0.0.0.0.0"));
            programs.Add(new ProgramDTO("test1", "description1", "v0.0.0.0.0"));
            programs.Add(new ProgramDTO("test1", "description1", "v0.0.0.0.0"));
            programs.Add(new ProgramDTO("test1", "description1", "v0.0.0.0.0"));
            //programs.Add(new ProgramDTO("test1", "description1", "v0.0.0.0.0"));
            //programs.Add(new ProgramDTO("test1", "description1", "v0.0.0.0.0"));
            //programs.Add(new ProgramDTO("test1", "description1", "v0.0.0.0.0"));
            //programs.Add(new ProgramDTO("test1", "description1", "v0.0.0.0.0"));
            //programs.Add(new ProgramDTO("test1", "description1", "v0.0.0.0.0"));
            //programs.Add(new ProgramDTO("test1", "description1", "v0.0.0.0.0"));
            //programs.Add(new ProgramDTO("test1", "description1", "v0.0.0.0.0"));
            //programs.Add(new ProgramDTO("test1", "description1", "v0.0.0.0.0"));
            //programs.Add(new ProgramDTO("test1", "description1", "v0.0.0.0.0"));
            //programs.Add(new ProgramDTO("test1", "description1", "v0.0.0.0.0"));
            //programs.Add(new ProgramDTO("test1", "description1", "v0.0.0.0.0"));
            //programs.Add(new ProgramDTO("test1", "description1", "v0.0.0.0.0"));
            //programs.Add(new ProgramDTO("test1", "description1", "v0.0.0.0.0"));

        }

        private void documentsBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void settingsBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void instructionBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void aboutBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void listProduct_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
