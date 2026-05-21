using Microsoft.Win32;
using PotrebAuto.Configuration;
using PotrebAuto.Extensions;
using PotrebAuto.Extensions.Filters;
using PotrebAuto.Models;
using PotrebAuto.Servises;
using PotrebAuto.Servises.ExcelReaderServices;
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

namespace PotrebAuto.Windows
{
    /// <summary>
    /// Р›РѕРіРёРєР° РІР·Р°РёРјРѕРґРµР№СЃС‚РІРёСЏ РґР»СЏ MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _selectedConsumersPath;
        private string _selectedSecondConsumersPath;
        private string _selectedConsumersAndSourcesPath;
        private string _selectedGiTPath;
        private string _selectedQlickPath;

        private readonly string defoultString = ConfigModel.DefoultString; // перенести в конфиг модел

        private readonly int _maxPathLength = 20;

        // Пути до шаблона и итоговой формы
        private string templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ExcelTemplates", "ConsumersTemplate.xlsx");
        private string templatePathExtra = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ExcelTemplates", "ConsumersTemplateSecond.xlsx");

        // для сохранениея в папке на рабочем столе
        private string reportsFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Потребители");
        private string newFilePath;

        public MainWindow()
        {
            InitializeComponent();

            // Создаем папку если ее нет
            if (!Directory.Exists(reportsFolder))
                Directory.CreateDirectory(reportsFolder);

            newFilePath = Path.Combine(reportsFolder, "Потребители_ДАТА.xlsx");

            Configuration.ConfigModel.LoadAllConfigurations();
        }

        private void OpenOutDirectory(string newFilePath)
        {
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

        private void OriginalFileBtn_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Excel files (*.xlsx;*.xls)|*.xlsx;*.xls",
                Title = "Выберете исходный файл",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            };

            if (openFileDialog.ShowDialog() == true)
            {
                _selectedConsumersPath = openFileDialog.FileName;
                var fileName = $"{Path.GetFileName(_selectedConsumersPath)}";
                OriginalFileText.Text = fileName.Length <= _maxPathLength ? fileName : fileName.Substring(0, _maxPathLength) + "...";
                OriginalFileText.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        private void OriginalSecondFileBtn_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Excel files (*.xlsx;*.xls)|*.xlsx;*.xls",
                Title = "Выберете дополнительный исходный файл",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            };

            if (openFileDialog.ShowDialog() == true)
            {
                _selectedSecondConsumersPath = openFileDialog.FileName;
                var fileName = $"{Path.GetFileName(_selectedSecondConsumersPath)}";
                OriginalSecondFileText.Text = fileName.Length <= _maxPathLength ? fileName : fileName.Substring(0, _maxPathLength) + "...";
                OriginalSecondFileText.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        private void ConsumersAndSourcesFileBtn_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Excel files (*.xlsx;*.xls)|*.xlsx;*.xls",
                Title = "Выберете файл Источники - потребители",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            };

            if (openFileDialog.ShowDialog() == true)
            {
                _selectedConsumersAndSourcesPath = openFileDialog.FileName;
                var fileName = $"{Path.GetFileName(_selectedConsumersAndSourcesPath)}";
                ConsumersAndSourcesFileText.Text = fileName.Length <= _maxPathLength ? fileName : fileName.Substring(0, _maxPathLength) + "...";
                ConsumersAndSourcesFileText.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        private void GiTFileBtn_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Excel files (*.xlsx;*.xls)|*.xlsx;*.xls",
                Title = "Выберете ГиТ файл",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            };

            if (openFileDialog.ShowDialog() == true)
            {
                _selectedGiTPath = openFileDialog.FileName;
                var fileName = $"{Path.GetFileName(_selectedGiTPath)}";
                GiTFileText.Text = fileName.Length <= _maxPathLength ? fileName : fileName.Substring(0, _maxPathLength) + "...";
                GiTFileText.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        private void QlickFileBtn_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Excel files (*.xlsx;*.xls)|*.xlsx;*.xls",
                Title = "Выберете файл с идентификатором объекта и кодом строения",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            };

            if (openFileDialog.ShowDialog() == true)
            {
                _selectedQlickPath = openFileDialog.FileName;
                var fileName = $"{Path.GetFileName(_selectedQlickPath)}";
                QlickFileText.Text = fileName.Length <= _maxPathLength ? fileName : fileName.Substring(0, _maxPathLength) + "...";
                QlickFileText.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        private void StartProgramm()
        {
            var consumers = ConsumersFileReaderService.ReadExcelFile(_selectedConsumersPath)
                                                                            .GetFiltered();

            var sources = SourcesAndConsumersFileReaderService.ReadExcelFile(_selectedConsumersAndSourcesPath)
                                                                                                .GetFilteredDict();

            var result = consumers.GetUnionData(sources);


            // вставка в эксель
            ExcelInsertService.ExcelDataInsert(templatePath, newFilePath, result);
        }

        private void StartProgrammExtra()
        {
            var GiTData = GiTFileReaderService.ReadExcelFile(_selectedGiTPath)
                                                                    .GetFilteredDict();

            var consumers = ConsumersFileReaderService.ReadExcelFile(_selectedConsumersPath)
                                                                        .GetFiltered();

            ConsumersDataObject.DateListTemp = ConsumersDataObject.DateList;


            var consumersSecond = ConsumersFileReaderService.ReadExcelFileExtra(_selectedSecondConsumersPath)
                                                                        .GetFilteredDict();

            var sources = SourcesAndConsumersFileReaderService.ReadExcelFile(_selectedConsumersAndSourcesPath)
                                                                                            .GetFilteredDict();

            var qlickData = QlickReaderService.ReadExcelFile(_selectedQlickPath)
                                                                .GetFilteredDict();



            
            var result = consumers.GetUnionDataExtra(consumersSecond, sources, GiTData, qlickData);



            ExcelInsertService.ExcelDataInsertExtra(templatePathExtra, newFilePath,
                                                result, consumersSecond, qlickData, sources);
        }

        private bool IsExtra()
        {
            if (!string.IsNullOrEmpty(_selectedSecondConsumersPath) || !string.IsNullOrEmpty(_selectedGiTPath) || !string.IsNullOrEmpty(_selectedQlickPath))
            {
                return true;
            }
            return false;
        }

        private bool IsValidToStart()
        {
            if (!string.IsNullOrEmpty(_selectedConsumersPath) && !string.IsNullOrEmpty(_selectedConsumersAndSourcesPath))
            {
                if (File.Exists(_selectedConsumersPath) && File.Exists(_selectedConsumersAndSourcesPath))
                {
                    return true;
                }
                else
                {
                    MessageBox.Show("Файл(ы) не найден(ы)!", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);

                    return false;
                }
            }
            else
            {
                MessageBox.Show("Сначала выберите необходимые файлы!", "Внимание",
                              MessageBoxButton.OK, MessageBoxImage.Warning);

                return false;
            }
        }

        private bool IsValidToStartExtra()
        {
            if (!string.IsNullOrEmpty(_selectedSecondConsumersPath) && !string.IsNullOrEmpty(_selectedGiTPath) && !string.IsNullOrEmpty(_selectedQlickPath))
            {
                if (File.Exists(_selectedSecondConsumersPath) && File.Exists(_selectedGiTPath) && File.Exists(_selectedQlickPath))
                {
                    return true;
                }
                else
                {
                    MessageBox.Show("Файл(ы) не найден(ы)!", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);

                    return false;
                }
            }
            else
            {
                MessageBox.Show("Сначала выберите необходимые файлы!", "Внимание",
                              MessageBoxButton.OK, MessageBoxImage.Warning);

                return false;
            }

        }

        private void start_Click(object sender, RoutedEventArgs e)
        {

            try
            {

                switch (IsExtra())
                {
                    case true:

                        if (!IsValidToStartExtra())
                            return;

                        StartProgrammExtra();
                        break;


                    case false:

                        if (!IsValidToStart())
                            return;

                        StartProgramm();
                        break;


                    default:
                        return;
                }



                ReadyText.Text = "ГОТОВО";
                ReadyText.Margin = new Thickness(7);


                // Открываем папку с файлом
                OpenOutDirectory(newFilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обработке данных: {ex.Message}\n\nПроверьте:\n- Корректность выбранных файлов\n- Настройки столбцов",
                              "Ошибка обработки", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void settingsBtn_Click(object sender, RoutedEventArgs e)
        {
            var settingsWindow = Application.Current.Windows
                .OfType<SettingsWindow>()
                .FirstOrDefault();

            if (settingsWindow == null)
            {
                settingsWindow = new SettingsWindow();
                settingsWindow.Show();
            }
            else
            {
                settingsWindow.Activate();
            }
        }

        private void instructionBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Путь к PDF файлу в выходной директории
                string pdfPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Instructions", "Инструкция Потребители.pdf");

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

        private void clearBtn_Click(object sender, RoutedEventArgs e)
        {
            ReadyText.Text = string.Empty;
            ReadyText.Margin = new Thickness(0);

            _selectedConsumersPath = string.Empty;
            _selectedSecondConsumersPath = string.Empty;
            _selectedConsumersAndSourcesPath = string.Empty;
            _selectedGiTPath = string.Empty;
            _selectedQlickPath = string.Empty;

            OriginalFileText.Text = defoultString;
            OriginalSecondFileText.Text = defoultString;
            ConsumersAndSourcesFileText.Text = defoultString;
            GiTFileText.Text = defoultString;
            QlickFileText.Text = defoultString;
        }
    }
}
