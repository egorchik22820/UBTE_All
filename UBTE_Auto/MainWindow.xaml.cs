using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using UBTE_Auto.AppData.Configuration;
using Path = System.IO.Path;
using UBTE_Auto.Windows;

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

        private void LoadPrograms()
        {
            // Ищем папку Programs в нескольких местах: рядом с exe (вариант поставки)
            // и на 1–3 уровня выше (запуск из bin\Debug при разработке).
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string[] candidates =
            {
                Path.Combine(baseDir, "Programs"),
                Path.GetFullPath(Path.Combine(baseDir, @"..\Programs")),
                Path.GetFullPath(Path.Combine(baseDir, @"..\..\Programs")),
                Path.GetFullPath(Path.Combine(baseDir, @"..\..\..\Programs")),
            };
            string programsDir = candidates.FirstOrDefault(Directory.Exists);
            if (programsDir == null)
            {
                MessageBox.Show(
                    "Папка Programs не найдена. Поиск выполнялся здесь:\n\n" +
                    string.Join("\n", candidates) +
                    "\n\nПоместите папку Programs рядом с UBTE_Auto.exe.",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var jsonFiles = Directory.GetFiles(programsDir, "program.json", SearchOption.AllDirectories);
            foreach (var jsonPath in jsonFiles)
            {
                try
                {
                    string json = File.ReadAllText(jsonPath);
                    var config = JsonConvert.DeserializeObject<ProgramConfig>(json);
                    string exePath = Path.Combine(Path.GetDirectoryName(jsonPath), config.ExecutablePath);

                    // Если версия не указана в JSON, пытаемся взять из .exe
                    string version = config.Version;
                    if (string.IsNullOrEmpty(version) && File.Exists(exePath))
                    {
                        var versionInfo = FileVersionInfo.GetVersionInfo(exePath);
                        version = versionInfo.ProductVersion ?? versionInfo.FileVersion;
                    }

                    programs.Add(new ProgramDTO
                    {
                        Name = config.Name,
                        Description = config.Description,
                        Version = version ?? "0.0",
                        ExecutablePath = exePath,
                        WorkingDirectory = config.WorkingDirectory ?? Path.GetDirectoryName(exePath),
                        Arguments = config.Arguments ?? ""
                    });
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Ошибка загрузки {jsonPath}: {ex.Message}");
                }
            }
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
            var selected = listProg.SelectedItem as ProgramDTO;
            if (selected == null) return;

            // Имя процесса = имя exe без расширения. GetProcessesByName НЕ обращается
            // к MainModule, поэтому: нет потока Win32Exception от системных процессов,
            // нет задержки при запуске и нет проблем на ПК с ограниченными правами.
            string exeName = Path.GetFileNameWithoutExtension(selected.ExecutablePath);
            bool alreadyRunning = Process.GetProcessesByName(exeName).Length > 0;

            if (alreadyRunning)
            {
                MessageBox.Show($"Программа '{selected.Name}' уже запущена.", "Информация",
                                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = selected.ExecutablePath,
                    WorkingDirectory = selected.WorkingDirectory,
                    Arguments = selected.Arguments,
                    UseShellExecute = true
                };
                Process.Start(startInfo);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось запустить программу:\n{ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void descriptionBtn_Click(object sender, RoutedEventArgs e)
        {
            var selected = listProg.SelectedItem as ProgramDTO;

            if (selected == null)
            {
                MessageBox.Show($"Выберите программу", "Предупреждение",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var descriptionWindow = Application.Current.Windows
                .OfType<DescriptionWindow>()
                .FirstOrDefault();

            if (descriptionWindow == null)
            {
                descriptionWindow = new DescriptionWindow(selected);
                descriptionWindow.Show();
            }
            else
            {
                descriptionWindow.Activate();
            }
        }

        private void searchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (listProg == null || listProg.ItemsSource == null) return;

            var view = CollectionViewSource.GetDefaultView(listProg.ItemsSource);
            if (view == null) return;

            string query = searchBox.Text?.Trim();
            if (string.IsNullOrEmpty(query))
            {
                // пустой запрос — показываем все программы
                view.Filter = null;
                return;
            }

            // фильтр по названию ИЛИ описанию, без учёта регистра
            view.Filter = item =>
            {
                var p = item as ProgramDTO;
                if (p == null) return false;
                return (p.Name != null && p.Name.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0)
                    || (p.Description != null && p.Description.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0);
            };
        }
    }
}
