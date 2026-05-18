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
using UBTE_Auto.AppData.DTO;

namespace UBTE_Auto
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<ProgramDTO> programs = new List<ProgramDTO>();
        public MainWindow()
        {
            InitializeComponent();

            LoadPrograms();
            listProg.ItemsSource = programs;

        }

        private void LoadPrograms()/////hjhjhj/////
        {
            programs.Clear();

            //programs.Add(new ProgramDTO("test1","description1", "v0.0.0.0.0" ));
            //programs.Add(new ProgramDTO("Баланс по ЦТП", "description1", "v0.0.0.0.0"));
            //programs.Add(new ProgramDTO("Котельные", "description1", "v0.0.0.0.0"));
            //programs.Add(new ProgramDTO("Потребители", "привет как дела, эта программа делает то да се и ваще тут тестирую как работает обрезание по словам а еще надо строки настроить", "v0.0.0.0.0"));
            //programs.Add(new ProgramDTO("Потери ГВС", "description1", "v0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0"));
            //programs.Add(new ProgramDTO("testtesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttest", "description1dfhdfhdfhdfhdfhfdhfdhddescription1dfhdfhdfhdfhdfhfdhfdhddescription1dfhdfhdfhdfhdfhfdhfdhddescription1dfhdfhdfhdfhdfhfdhfddescription1dfhdfhdfhdfhdfhfdhfddescription1dfhdfhdfhdfhdfhfdhfdhd", "v0.0.0.0.hdfhfdhdfhfdhfdhdhfd0"));
            //programs.Add(new ProgramDTO("test1", "description1", "v0.0.0.0.0"));
            //programs.Add(new ProgramDTO("1", "description1", "v0.0.0.0.0"));
            //programs.Add(new ProgramDTO("tt1", "description1", "v0.0.0.0.0"));
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
            //programs.Add(new ProgramDTO("test1", "description1", "v0.0.0.0.0"));
            //programs.Add(new ProgramDTO("test1", "description1", "v0.0.0.0.0"));



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

        private void documentsBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void listProduct_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void descriptionBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
