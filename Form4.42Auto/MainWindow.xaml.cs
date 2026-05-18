using Form4._42Auto.Models;
using Form4._42Auto.Services;
using Microsoft.Win32;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
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

namespace Form4._42Auto
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _selectedExcelPath_OFTE; // Переменная для хранения пути ОФ ТЭ
        private string _selectedExcelPath_OldForm; // Переменная для хранения пути старой формы

        public MainWindow()
        {
            InitializeComponent();
        }

        private void CreateReportFromTemplate(string templatePath, string newFilePath, Dictionary<string, object> valuesToWrite)
        {
            // Проверяем существование шаблона
            if (!File.Exists(templatePath))
            {
                throw new FileNotFoundException($"Шаблон не найден: {templatePath}");
            }

            // Копируем шаблон
            File.Copy(templatePath, newFilePath, true);

            // Записываем данные
            using (var package = new ExcelPackage(new FileInfo(newFilePath)))
            {
                var worksheet = package.Workbook.Worksheets["wsParent"];

                if (worksheet == null)
                {
                    throw new Exception("Лист 'wsParent' не найден в файле");
                }

                foreach (var cell in valuesToWrite)
                {
                    worksheet.Cells[cell.Key].Value = cell.Value;
                }

                package.Save();

            }
        }

        public void PasteToOren(string templatePath, string newFilePath, List<List<ExportDataTable>> orenburgData, List<List<decimal>> orenDataOld)
        {
            var valuesToWrite = new Dictionary<string, object>();
            
            // данные из предыдущей формы
            for (int i = 0; i < 10; i++)
            {
                int rowNumber = 85 + i;

                if (rowNumber == 94)
                    break;
                if (rowNumber == 87 || rowNumber == 88)
                    continue;

                if (i < orenDataOld.Count && orenDataOld[i].Count >= 2)
                {
                    valuesToWrite[$"G{rowNumber}"] = orenDataOld[i][0];
                    valuesToWrite[$"H{rowNumber}"] = orenDataOld[i][1];
                }
            }

            #region 1. Пром

            valuesToWrite["I85"] = (Filter.PromOrgFilter_GCal(orenburgData[0]).Sum(x => x.Quantity_GCal)) * 1000;
            valuesToWrite["J85"] = (Filter.PromOrgFilter(orenburgData[0]).Sum(x => x.Quantity_m3)) * 1000;
            valuesToWrite["K85"] = Filter.PromOrgFilter(orenburgData[0]).Sum(x => x.Amount_rub);

            valuesToWrite["L85"] = Filter.PromOrgFilter(orenburgData[0]).Sum(x => x.PaidInPeriod_rub)
                                - Filter.PromOrgFilter(orenburgData[0]).Sum(x => x.CorrectionDZ)
                                - Filter.PromOrgFilter(orenburgData[0]).Sum(x => x.TransferDZ)
                                + Filter.PromOrgFilter(orenburgData[0]).Sum(x => x.OffsKZ)
                                + ((orenDataOld[0][1] - orenDataOld[0][0]) - Filter.PromOrgFilter(orenburgData[0]).Sum(x => x.SaldoDZ));

            valuesToWrite["M85"] = Filter.PromOrgFilter(orenburgData[0]).Sum(x => x.ValueOfDZ);

            valuesToWrite["P85"] = Filter.PromOrgFilter(orenburgData[0]).Sum(x => x.KZ_InEndPeriod);
            valuesToWrite["Q85"] = Filter.PromOrgFilter(orenburgData[0]).Sum(x => x.TotalDZ);
            valuesToWrite["R85"] = Filter.PromOrgFilter(orenburgData[0]).Sum(x => x.CurrentDZ);

            #endregion

            #region 2. МинОбр

            valuesToWrite["I86"] = (Filter.MinOboronOrgFilter(orenburgData[0]).Sum(x => x.Quantity_GCal)) * 1000;
            valuesToWrite["J86"] = (Filter.MinOboronOrgFilter(orenburgData[0]).Sum(x => x.Quantity_m3)) * 1000;
            valuesToWrite["K86"] = Filter.MinOboronOrgFilter(orenburgData[0]).Sum(x => x.Amount_rub);

            valuesToWrite["L86"] = Filter.MinOboronOrgFilter(orenburgData[0]).Sum(x => x.PaidInPeriod_rub)
                                - Filter.MinOboronOrgFilter(orenburgData[0]).Sum(x => x.CorrectionDZ)
                                - Filter.MinOboronOrgFilter(orenburgData[0]).Sum(x => x.TransferDZ)
                                + Filter.MinOboronOrgFilter(orenburgData[0]).Sum(x => x.OffsKZ)
                                + ((orenDataOld[1][1] - orenDataOld[1][0]) - Filter.MinOboronOrgFilter(orenburgData[0]).Sum(x => x.SaldoDZ));

            valuesToWrite["M86"] = Filter.MinOboronOrgFilter(orenburgData[0]).Sum(x => x.ValueOfDZ);

            valuesToWrite["P86"] = Filter.MinOboronOrgFilter(orenburgData[0]).Sum(x => x.KZ_InEndPeriod);
            valuesToWrite["Q86"] = Filter.MinOboronOrgFilter(orenburgData[0]).Sum(x => x.TotalDZ);
            valuesToWrite["R86"] = Filter.MinOboronOrgFilter(orenburgData[0]).Sum(x => x.CurrentDZ);

            #endregion

            #region 3.1 ФедБюдж

            valuesToWrite["I89"] = (Filter.FromFedBujetFilter(orenburgData[0]).Sum(x => x.Quantity_GCal)) * 1000;
            valuesToWrite["J89"] = (Filter.FromFedBujetFilter(orenburgData[0]).Sum(x => x.Quantity_m3)) * 1000;
            valuesToWrite["K89"] = Filter.FromFedBujetFilter(orenburgData[0]).Sum(x => x.Amount_rub);

            valuesToWrite["L89"] = Filter.FromFedBujetFilter(orenburgData[0]).Sum(x => x.PaidInPeriod_rub)
                                - Filter.FromFedBujetFilter(orenburgData[0]).Sum(x => x.CorrectionDZ)
                                - Filter.FromFedBujetFilter(orenburgData[0]).Sum(x => x.TransferDZ)
                                + Filter.FromFedBujetFilter(orenburgData[0]).Sum(x => x.OffsKZ)
                                + ((orenDataOld[4][1] - orenDataOld[4][0]) - Filter.FromFedBujetFilter(orenburgData[0]).Sum(x => x.SaldoDZ));

            valuesToWrite["M89"] = Filter.FromFedBujetFilter(orenburgData[0]).Sum(x => x.ValueOfDZ);

            valuesToWrite["P89"] = Filter.FromFedBujetFilter(orenburgData[0]).Sum(x => x.KZ_InEndPeriod);
            valuesToWrite["Q89"] = Filter.FromFedBujetFilter(orenburgData[0]).Sum(x => x.TotalDZ);
            valuesToWrite["R89"] = Filter.FromFedBujetFilter(orenburgData[0]).Sum(x => x.CurrentDZ);

            #endregion

            #region 3.2 Бюдж суб федерации

            valuesToWrite["I90"] = (Filter.FromSubFedBujetFilter(orenburgData[0]).Sum(x => x.Quantity_GCal)) * 1000;
            valuesToWrite["J90"] = (Filter.FromSubFedBujetFilter(orenburgData[0]).Sum(x => x.Quantity_m3)) * 1000;
            valuesToWrite["K90"] = Filter.FromSubFedBujetFilter(orenburgData[0]).Sum(x => x.Amount_rub);

            valuesToWrite["L90"] = Filter.FromSubFedBujetFilter(orenburgData[0]).Sum(x => x.PaidInPeriod_rub)
                                - Filter.FromSubFedBujetFilter(orenburgData[0]).Sum(x => x.CorrectionDZ)
                                - Filter.FromSubFedBujetFilter(orenburgData[0]).Sum(x => x.TransferDZ)
                                + Filter.FromSubFedBujetFilter(orenburgData[0]).Sum(x => x.OffsKZ)
                                + ((orenDataOld[5][1] - orenDataOld[5][0]) - Filter.FromSubFedBujetFilter(orenburgData[0]).Sum(x => x.SaldoDZ));

            valuesToWrite["M90"] = Filter.FromSubFedBujetFilter(orenburgData[0]).Sum(x => x.ValueOfDZ);

            valuesToWrite["P90"] = Filter.FromSubFedBujetFilter(orenburgData[0]).Sum(x => x.KZ_InEndPeriod);
            valuesToWrite["Q90"] = Filter.FromSubFedBujetFilter(orenburgData[0]).Sum(x => x.TotalDZ);
            valuesToWrite["R90"] = Filter.FromSubFedBujetFilter(orenburgData[0]).Sum(x => x.CurrentDZ);

            #endregion

            #region 4. Населен

            valuesToWrite["I91"] = (Filter.NaselenFilter(orenburgData[0]).Sum(x => x.Quantity_GCal)) * 1000;
            valuesToWrite["J91"] = (Filter.NaselenFilter(orenburgData[0]).Sum(x => x.Quantity_m3)) * 1000;
            valuesToWrite["K91"] = Filter.NaselenFilter(orenburgData[0]).Sum(x => x.Amount_rub);

            valuesToWrite["L91"] = Filter.NaselenFilter(orenburgData[0]).Sum(x => x.PaidInPeriod_rub)
                                - Filter.NaselenFilter(orenburgData[0]).Sum(x => x.CorrectionDZ)
                                - Filter.NaselenFilter(orenburgData[0]).Sum(x => x.TransferDZ)
                                + Filter.NaselenFilter(orenburgData[0]).Sum(x => x.OffsKZ)
                                + ((orenDataOld[6][1] - orenDataOld[6][0]) - Filter.NaselenFilter(orenburgData[0]).Sum(x => x.SaldoDZ));

            valuesToWrite["M91"] = Filter.NaselenFilter(orenburgData[0]).Sum(x => x.ValueOfDZ);

            valuesToWrite["P91"] = Filter.NaselenFilter(orenburgData[0]).Sum(x => x.KZ_InEndPeriod);
            valuesToWrite["Q91"] = Filter.NaselenFilter(orenburgData[0]).Sum(x => x.TotalDZ);
            valuesToWrite["R91"] = Filter.NaselenFilter(orenburgData[0]).Sum(x => x.CurrentDZ);

            #endregion

            #region 5. Тепло орг

            valuesToWrite["I92"] = (Filter.TeploOrgFilter(orenburgData[0]).Sum(x => x.Quantity_GCal)) * 1000;
            valuesToWrite["J92"] = (Filter.TeploOrgFilter(orenburgData[0]).Sum(x => x.Quantity_m3)) * 1000;
            valuesToWrite["K92"] = Filter.TeploOrgFilter(orenburgData[0]).Sum(x => x.Amount_rub);

            valuesToWrite["L92"] = Filter.TeploOrgFilter(orenburgData[0]).Sum(x => x.PaidInPeriod_rub)
                                - Filter.TeploOrgFilter(orenburgData[0]).Sum(x => x.CorrectionDZ)
                                - Filter.TeploOrgFilter(orenburgData[0]).Sum(x => x.TransferDZ)
                                + Filter.TeploOrgFilter(orenburgData[0]).Sum(x => x.OffsKZ)
                                + ((orenDataOld[7][1] - orenDataOld[7][0])- Filter.TeploOrgFilter(orenburgData[0]).Sum(x => x.SaldoDZ));

            valuesToWrite["M92"] = Filter.TeploOrgFilter(orenburgData[0]).Sum(x => x.ValueOfDZ);

            valuesToWrite["P92"] = Filter.TeploOrgFilter(orenburgData[0]).Sum(x => x.KZ_InEndPeriod);
            valuesToWrite["Q92"] = Filter.TeploOrgFilter(orenburgData[0]).Sum(x => x.TotalDZ);
            valuesToWrite["R92"] = Filter.TeploOrgFilter(orenburgData[0]).Sum(x => x.CurrentDZ);

            #endregion

            #region 6. Проч

            valuesToWrite["I93"] = (Filter.ProchFilter(orenburgData[0]).Sum(x => x.Quantity_GCal)) * 1000;
            valuesToWrite["J93"] = (Filter.ProchFilter(orenburgData[0]).Sum(x => x.Quantity_m3)) * 1000;
            valuesToWrite["K93"] = Filter.ProchFilter(orenburgData[0]).Sum(x => x.Amount_rub);

            valuesToWrite["L93"] = Filter.ProchFilter(orenburgData[0]).Sum(x => x.PaidInPeriod_rub)
                                - Filter.ProchFilter(orenburgData[0]).Sum(x => x.CorrectionDZ)
                                - Filter.ProchFilter(orenburgData[0]).Sum(x => x.TransferDZ)
                                + Filter.ProchFilter(orenburgData[0]).Sum(x => x.OffsKZ)
                                + ((orenDataOld[8][1] - orenDataOld[8][0]) - Filter.ProchFilter(orenburgData[0]).Sum(x => x.SaldoDZ));

            valuesToWrite["M93"] = Filter.ProchFilter(orenburgData[0]).Sum(x => x.ValueOfDZ);

            valuesToWrite["P93"] = Filter.ProchFilter(orenburgData[0]).Sum(x => x.KZ_InEndPeriod);
            valuesToWrite["Q93"] = Filter.ProchFilter(orenburgData[0]).Sum(x => x.TotalDZ);
            valuesToWrite["R93"] = Filter.ProchFilter(orenburgData[0]).Sum(x => x.CurrentDZ);

            #endregion

            // Создаем итоговую форму
            CreateReportFromTemplate(templatePath, newFilePath, valuesToWrite);
        }

        private void SelectOldFileBtn_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Excel files (*.xlsx;*.xls)|*.xlsx;*.xls",
                Title = "Выберите файл предыдущей формы",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            };

            if (openFileDialog.ShowDialog() == true)
            {
                _selectedExcelPath_OldForm = openFileDialog.FileName;
                selectedFileText.Text = $"Выбран файл: {Path.GetFileName(_selectedExcelPath_OldForm)}";
                selectedFileText.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        private void SelectFileBtn_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Excel files (*.xlsx;*.xls)|*.xlsx;*.xls",
                Title = "Выберите файл ОФ ТЭ",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            };

            if (openFileDialog.ShowDialog() == true)
            {
                _selectedExcelPath_OFTE = openFileDialog.FileName;
                selectedFileText.Text = $"Выбран файл: {Path.GetFileName(_selectedExcelPath_OFTE)}";
                selectedFileText.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        private void start_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_selectedExcelPath_OFTE) || string.IsNullOrEmpty(_selectedExcelPath_OldForm))
            {
                MessageBox.Show("Сначала выберите файл необходимые файлы!", "Внимание",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!File.Exists(_selectedExcelPath_OFTE) || !File.Exists(_selectedExcelPath_OldForm))
            {
                MessageBox.Show("Файл не найден!", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string excelFilePath_OFTE = _selectedExcelPath_OFTE;
            string excelFilePath_OldForm = _selectedExcelPath_OldForm;

            // Читает файлы и сохраняет в списки
            var excelReader = new ExcelReaderService();
            List<ExportDataTable> dataList = excelReader.ReadExcelFile(excelFilePath_OFTE);

            var oldFileReader = new OldFileReader();
            List<List<decimal>> oldDataList = oldFileReader.ReadExcelFile(excelFilePath_OldForm);

            //отладка
            Console.WriteLine(dataList.Count);
            Console.WriteLine(oldDataList.Count);

            // Путь до шаблона
            string templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Ex", "Шаблон 4.42 _10521.xlsx");

            List<List<ExportDataTable>> orenList = new List<List<ExportDataTable>>();
            orenList.Add(dataList);

            // Автоматическое сохранение в папку на рабочем столе
            string reportsFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Form_4.42_Reports");

            // Создаем папку если ее нет
            if (!Directory.Exists(reportsFolder))
            {
                Directory.CreateDirectory(reportsFolder);
            }

            // Формируем пути к файлам
            string newFilePathOren = Path.Combine(reportsFolder, $"Шаблон 4.42 _10521.xlsx");

            // Создаем отчеты
            PasteToOren(templatePath, newFilePathOren, orenList, oldDataList);

            selectedFileText.Text += "...ГОТОВО";

            // Открываем папку с файлом
            try
            {
                System.Diagnostics.Process.Start("explorer.exe", $"/select,\"{newFilePathOren}\"");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось открыть папку: {ex.Message}",
                              "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

    }
}
