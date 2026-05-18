using Microsoft.Win32;
using OfficeOpenXml;
using PoteryGVS.Extensions;
using PoteryGVS.Extensions.FilterExtensions;
using PoteryGVS.Models;
using PoteryGVS.Services;
using PoteryGVS.Services.ExcelReaderServices;
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
using Path = System.IO.Path;

namespace PoteryGVS.Windows
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _selectedNakladPath;
        private string _selectedMPPath;
        private string _selectedODNPath;
        private string _selectedOldFilePath;

        public MainWindow()
        {
            InitializeComponent();
            Configuration.ConfigModel.LoadAllConfigurations();
        }

        private void selectNakladFileBtn_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Excel files (*.xlsx;*.xls)|*.xlsx;*.xls",
                Title = "Выберите файл накладной",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            };

            if (openFileDialog.ShowDialog() == true)
            {
                _selectedNakladPath = openFileDialog.FileName;
                var fileName = $"{Path.GetFileName(_selectedNakladPath)}";
                selectedNakladFileText.Text = fileName.Length <= 20 ? fileName : fileName.Substring(0, 20) + "...";
                selectedNakladFileText.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        private void selectMPFileBtn_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Excel files (*.xlsx;*.xls)|*.xlsx;*.xls",
                Title = "Выберите файл МП",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            };

            if (openFileDialog.ShowDialog() == true)
            {
                _selectedMPPath = openFileDialog.FileName;
                var fileName = $"{Path.GetFileName(_selectedMPPath)}";
                selectedMPFileText.Text = fileName.Length <= 20 ? fileName : fileName.Substring(0, 20) + "...";
                selectedMPFileText.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        private void selectODNFileBtn_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Excel files (*.xlsx;*.xls)|*.xlsx;*.xls",
                Title = "Выберите файл ОДН",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            };

            if (openFileDialog.ShowDialog() == true)
            {
                _selectedODNPath = openFileDialog.FileName;
                var fileName = $"{Path.GetFileName(_selectedODNPath)}";
                selectedODNFileText.Text = fileName.Length <= 20 ? fileName : fileName.Substring(0, 20) + "...";
                selectedODNFileText.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        private void selectOldFileBtn_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Excel files (*.xlsx;*.xls)|*.xlsx;*.xls",
                Title = "Выберите файл Потерь ГВС за предыдущий период",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            };

            if (openFileDialog.ShowDialog() == true)
            {
                _selectedOldFilePath = openFileDialog.FileName;
                var fileName = $"{Path.GetFileName(_selectedOldFilePath)}";
                selectedOldFileText.Text = fileName.Length <= 20 ? fileName : fileName.Substring(0, 20) + "...";
                selectedOldFileText.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        private void start_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_selectedNakladPath) || string.IsNullOrEmpty(_selectedMPPath)
                                                          || string.IsNullOrEmpty(_selectedODNPath)
                                                          || string.IsNullOrEmpty(_selectedOldFilePath))
            {
                MessageBox.Show("Сначала выберите необходимые файлы!", "Внимание",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!File.Exists(_selectedNakladPath) || !File.Exists(_selectedMPPath)
                                                  || !File.Exists(_selectedODNPath)
                                                  || !File.Exists(_selectedOldFilePath))
            {
                MessageBox.Show("Файл(ы) не найден(ы)!", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Путь до шаблона и итоговой формы
            string templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ExcelTemplates", "TemplatePoteryGVS.xlsx");
            // Автоматическое сохранение в папку на рабочем столе
            string reportsFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Потери ГВС");

            // Создаем папку если ее нет
            if (!Directory.Exists(reportsFolder))
                Directory.CreateDirectory(reportsFolder);

            string newFilePath = Path.Combine(reportsFolder, "Потери ГВС_ДАТА.xlsx");

            try
            {
                // читаем файлы и сохраняем в списки

                // файл Потерь ГВС за предыдущий период
                List<GVSDataObject> oldGVSData_With_ODPU = OldDataFileReaderService.ReadExcelSheet_With_ODPU(_selectedOldFilePath);
                List<GVSDataObject> oldGVSData_WithOut_ODPU = OldDataFileReaderService.ReadExcelSheet_WithOut_ODPU(_selectedOldFilePath);
                List<GVSDataObject> oldGVSData_With_ITP = OldDataFileReaderService.ReadExcelSheet_With_ITP(_selectedOldFilePath);

                List<GVSDataObject> oldDataUnion = new List<GVSDataObject>().GetUnion_OldData(oldGVSData_With_ODPU, oldGVSData_WithOut_ODPU,
                                                                                                                        oldGVSData_With_ITP);

                // файл ОДН
                List<ODNDataObject> ODNData = ODNFileReaderService.ReadExcelFile(_selectedODNPath);

                // накладная для МКД с ОДПУ
                List<NakladDataObject> nakladData = NakladFileReaderService.ReadExcelFile(_selectedNakladPath);
                List<NakladDataObject> nakladData_With_ODPU = nakladData.GetNeedObjects_With_ODPU();

                // файл МП
                List<MPDataObject> MPData = MPFileReaderService.ReadExcelFile(_selectedMPPath)
                                                                .GetNeedObjects();

                List<MPDataObject> MPData_ITP = MPFileReaderService.ReadExcelFile(_selectedMPPath)
                                                                .GetNeedObjects_ITP();

                // объединяем объекты для МКД с ОДПУ
                List<GVSDataObject> GVSData_With_ODPU_Raw = new List<GVSDataObject>().GetUnionData_With_ODPU(nakladData_With_ODPU, MPData,
                                                                                                                    ODNData, oldDataUnion);
                List<GVSDataObject> GVSData_With_ODPU = GVSData_With_ODPU_Raw.DeleteEmptyNull_m3();

                // накладная для МКД без ОДПУ
                List<NakladDataObject> nakladData_WithOut_ODPU_Raw = nakladData.GetNeedObjects_WithOut_ODPU(GVSData_With_ODPU);
                List<NakladDataObject> nakladData_WithOut_ODPU = nakladData.GetNeedObjects_WithOut_ODPU(GVSData_With_ODPU)
                                                                            .DeleteEmptyNull_m3();

                // объединяем объекты для МКД без ОДПУ
                List<GVSDataObject> GVSData_WithOut_ODPU_Raw = new List<GVSDataObject>().GetUnionData_WithOut_ODPU(nakladData_WithOut_ODPU_Raw,
                                                                                                               GVSData_With_ODPU, oldDataUnion);

                List<GVSDataObject> GVSData_WithOut_ODPU = new List<GVSDataObject>().GetUnionData_WithOut_ODPU(nakladData_WithOut_ODPU,
                                                                                                               GVSData_With_ODPU, oldDataUnion);

                //
                List<GVSDataObject> GVSData_With_ITP = new List<GVSDataObject>().GetUnionData_With_ITP(GVSData_With_ODPU_Raw,
                                                                                                            GVSData_WithOut_ODPU_Raw,
                                                                                                            oldDataUnion, MPData_ITP);

                // вставка в эксель
                ExcelInsertService.ExcelDataInsert(templatePath, newFilePath,
                                                    GVSData_With_ODPU, GVSData_WithOut_ODPU, GVSData_With_ITP);

                ReadyText.Text = "ГОТОВО";
                ReadyText.Margin = new Thickness(10);

                // Открываем папку с файлом
                try
                {
                    System.Diagnostics.Process.Start("explorer.exe", $"/select,\"{newFilePath}\"");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Не удалось открыть папку: {ex.Message}",
                                  "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обработке данных: {ex.Message}\n\nПроверьте:\n- Корректность выбранных файлов\n- Настройки столбцов",
                              "Ошибка обработки", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void settingsBtn_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.Show();
        }

        private void instructionBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Путь к PDF файлу в выходной директории
                string pdfPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Instruction", "Инструкция Потери ГВС.pdf");

                if (File.Exists(pdfPath))
                {
                    Process.Start(pdfPath);
                }
                else
                {
                    MessageBox.Show("PDF файл не найден!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при открытии PDF: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
