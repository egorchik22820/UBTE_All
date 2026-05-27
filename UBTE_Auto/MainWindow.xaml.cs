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
            // Папка Programs находится рядом с исполняемым файлом WPF приложения
            string programsDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Programs"); // идет не в ту папку
            if (!Directory.Exists(programsDir))
            {
                // Если папки нет, пробуем подняться на уровень выше (на случай, если WPF.exe лежит в bin\Debug)
                programsDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Programs");
                programsDir = Path.GetFullPath(programsDir);
                if (!Directory.Exists(programsDir))
                {
                    MessageBox.Show("Папка Programs не найдена!");
                    return;
                }
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

            bool alreadyRunning = Process.GetProcesses()
                .Any(p =>
                {
                    try
                    {
                        return p.MainModule != null &&
                                p.MainModule.FileName.Equals(selected.ExecutablePath, StringComparison.OrdinalIgnoreCase);
                    }
                    catch { return false; }
                });

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
    }
}
