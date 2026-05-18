using Form46Auto.DTO;
using Form46Auto.Services;
using Microsoft.Win32;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
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
using Microsoft.Win32;
using Path = System.IO.Path;

namespace Form46Auto
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private string _selectedExcelPath; // Переменная для хранения пути

        public MainWindow()
        {
            InitializeComponent();
        }

        private void SelectFileBtn_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Excel files (*.xlsx;*.xls)|*.xlsx;*.xls|All files (*.*)|*.*",
                Title = "Выберите файл накладной",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            };

            if (openFileDialog.ShowDialog() == true)
            {
                _selectedExcelPath = openFileDialog.FileName;
                selectedFileText.Text = $"Выбран файл: {Path.GetFileName(_selectedExcelPath)}";
                selectedFileText.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        // Создает копию шаблона и заполняет значениями из словоря
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
                var worksheet = package.Workbook.Worksheets["46 (2)"];

                if (worksheet == null)
                {
                    throw new Exception("Лист '46 (2)' не найден в файле");
                }

                foreach (var cell in valuesToWrite)
                {
                    worksheet.Cells[cell.Key].Value = cell.Value;
                }

                package.Save();
                selectedFileText.Text += "...ГОТОВО";
            }
        }

        // Использование для таблиц
        public void FillAllTables(string templatePath, string newFilePath, List<List<ExportDataTable>> orenburgData,
                                 List<List<ExportDataTable>> mednogorskData, List<List<ExportDataTable>> orskData, List<List<ExportDataTable>> kargalaData)
        {
            var valuesToWrite = new Dictionary<string, object>();

            // Таблица 1: Оренбург -- Прочие
            valuesToWrite["C7"] = orenburgData[0].Sum(x => x.Quantity);
            valuesToWrite["D7"] = orenburgData[0].Sum(x => x.QuantityByMeter);
            valuesToWrite["G7"] = orenburgData[0].Sum(x => x.AmountWithoutVAT);
            valuesToWrite["H7"] = orenburgData[0].Sum(x => x.AmountByMeter);

            //Таблица 1: Оренбург -- Населен ГВС
            valuesToWrite["C10"] = orenburgData[1].Sum(x => x.Quantity);
            valuesToWrite["D10"] = orenburgData[1].Sum(x => x.QuantityByMeter);
            valuesToWrite["G10"] = orenburgData[8].Sum(x => x.AmountWithoutVAT);
            valuesToWrite["H10"] = orenburgData[8].Sum(x => x.AmountByMeter);

            //Таблица 1: Оренбург -- Населен ОТОПЛ
            valuesToWrite["C9"] = orenburgData[2].Sum(x => x.Quantity);
            valuesToWrite["G9"] = orenburgData[2].Sum(x => x.AmountWithoutVAT);
            if (orenburgData[2].Sum(x => x.Quantity) > 0)
            {
                valuesToWrite["D9"] = orenburgData[2].Sum(x => x.QuantityByMeter);
                valuesToWrite["H9"] = orenburgData[2].Sum(x => x.AmountByMeter);
            }
                

            //Таблица 1: Оренбург -- Бюдж
            valuesToWrite["C11"] = orenburgData[6].Sum(x => x.Quantity);
            valuesToWrite["D11"] = orenburgData[6].Sum(x => x.QuantityByMeter);
            valuesToWrite["G11"] = orenburgData[6].Sum(x => x.AmountWithoutVAT);// мб по ПУ
            valuesToWrite["H11"] = orenburgData[6].Sum(x => x.AmountByMeter);

            //Таблица 1: Оренбург -- Пар проч.
            valuesToWrite["C13"] = orenburgData[3].Sum(x => x.Quantity);
            valuesToWrite["E13"] = orenburgData[3].Sum(x => x.Quantity);
            valuesToWrite["G13"] = orenburgData[3].Sum(x => x.AmountWithoutVAT);// мб по ПУ
            valuesToWrite["I13"] = orenburgData[3].Sum(x => x.AmountWithoutVAT);

            //Таблица 1: Оренбург -- Пар проч.
            valuesToWrite["C15"] = orenburgData[4].Sum(x => x.Quantity);
            valuesToWrite["D15"] = orenburgData[4].Sum(x => x.QuantityByMeter);
            valuesToWrite["G15"] = orenburgData[4].Sum(x => x.AmountWithoutVAT);// мб по ПУ
            valuesToWrite["H15"] = orenburgData[4].Sum(x => x.AmountByMeter);

            //Таблица 1: Оренбург -- Пар проч сеть.
            valuesToWrite["C17"] = orenburgData[7].Sum(x => x.Quantity);
            valuesToWrite["D17"] = orenburgData[7].Sum(x => x.QuantityByMeter);
            valuesToWrite["G17"] = orenburgData[7].Sum(x => x.AmountWithoutVAT);// мб по ПУ
            valuesToWrite["H17"] = orenburgData[7].Sum(x => x.AmountByMeter);

            //Таблица 1: Оренбург -- Пар Бюдж. сеть.
            valuesToWrite["C18"] = orenburgData[5].Sum(x => x.Quantity);
            valuesToWrite["D18"] = orenburgData[5].Sum(x => x.QuantityByMeter);
            valuesToWrite["G18"] = orenburgData[5].Sum(x => x.AmountWithoutVAT);// мб по ПУ
            valuesToWrite["H18"] = orenburgData[5].Sum(x => x.AmountByMeter);




            // Таблица 2: Медногорск -- Прочие
            valuesToWrite["C26"] = mednogorskData[0].Sum(x => x.Quantity);
            valuesToWrite["D26"] = mednogorskData[0].Sum(x => x.QuantityByMeter);
            valuesToWrite["G26"] = mednogorskData[0].Sum(x => x.AmountWithoutVAT);
            valuesToWrite["H26"] = mednogorskData[0].Sum(x => x.AmountByMeter);

            // Таблица 2: Медногорск -- Населен ГВС
            valuesToWrite["C29"] = mednogorskData[1].Sum(x => x.Quantity);
            valuesToWrite["D29"] = mednogorskData[1].Sum(x => x.QuantityByMeter);
            valuesToWrite["G29"] = mednogorskData[1].Sum(x => x.AmountWithoutVAT);
            valuesToWrite["H29"] = mednogorskData[1].Sum(x => x.AmountByMeter);

            // Таблица 2: Медногорск -- Населен ОТОПЛ
            valuesToWrite["C28"] = mednogorskData[2].Sum(x => x.Quantity);
            valuesToWrite["G28"] = mednogorskData[2].Sum(x => x.AmountWithoutVAT);
            if (mednogorskData[2].Sum(x => x.Quantity) > 0)
            {
                valuesToWrite["D28"] = mednogorskData[2].Sum(x => x.QuantityByMeter);
                valuesToWrite["H28"] = mednogorskData[2].Sum(x => x.AmountByMeter);
            }
            

            // Таблица 2: Медногорск -- Бюдж
            valuesToWrite["C30"] = mednogorskData[3].Sum(x => x.Quantity);
            valuesToWrite["D30"] = mednogorskData[3].Sum(x => x.QuantityByMeter);
            valuesToWrite["G30"] = mednogorskData[3].Sum(x => x.AmountWithoutVAT);
            valuesToWrite["H30"] = mednogorskData[3].Sum(x => x.AmountByMeter);




            // Таблица 3: Орск -- Проч коллекторы
            valuesToWrite["C40"] = orskData[6].Sum(x => x.Quantity);
            valuesToWrite["D40"] = orskData[6].Sum(x => x.QuantityByMeter);
            valuesToWrite["G40"] = orskData[6].Sum(x => x.AmountWithoutVAT);
            valuesToWrite["H40"] = orskData[6].Sum(x => x.AmountByMeter);

            // Таблица 3: Орск -- коллекторы др
            valuesToWrite["C41"] = orskData[7].Sum(x => x.Quantity);
            valuesToWrite["D41"] = orskData[7].Sum(x => x.Quantity);
            valuesToWrite["G41"] = orskData[7].Sum(x => x.AmountWithoutVAT);
            valuesToWrite["H41"] = orskData[7].Sum(x => x.AmountWithoutVAT);

            // Таблица 3: Орск -- Проч сеть
            valuesToWrite["C43"] = orskData[0].Sum(x => x.Quantity);
            valuesToWrite["D43"] = orskData[0].Sum(x => x.QuantityByMeter);
            valuesToWrite["G43"] = orskData[0].Sum(x => x.AmountWithoutVAT);
            valuesToWrite["H43"] = orskData[0].Sum(x => x.AmountByMeter);

            // Таблица 3: Орск -- Населен ГВС
            valuesToWrite["C46"] = orskData[1].Sum(x => x.Quantity);
            valuesToWrite["D46"] = orskData[1].Sum(x => x.QuantityByMeter);
            valuesToWrite["G46"] = orskData[1].Sum(x => x.AmountWithoutVAT);
            valuesToWrite["H46"] = orskData[1].Sum(x => x.AmountByMeter);

            // Таблица 3: Орск -- Населен ОТОПЛ
            valuesToWrite["C45"] = orskData[2].Sum(x => x.Quantity);
            valuesToWrite["G45"] = orskData[2].Sum(x => x.AmountWithoutVAT);
            if (orskData[2].Sum(x => x.Quantity) > 0)
            {
                valuesToWrite["D45"] = orskData[2].Sum(x => x.QuantityByMeter);
                valuesToWrite["H45"] = orskData[2].Sum(x => x.AmountByMeter);
            }
            

            // Таблица 3: Орск -- Населен Бюдж
            valuesToWrite["C47"] = orskData[4].Sum(x => x.Quantity);
            valuesToWrite["D47"] = orskData[4].Sum(x => x.QuantityByMeter);
            valuesToWrite["G47"] = orskData[4].Sum(x => x.AmountWithoutVAT);
            valuesToWrite["H47"] = orskData[4].Sum(x => x.AmountByMeter);

            // Таблица 3: Орск -- Населен Компенс
            valuesToWrite["C49"] = orskData[5].Sum(x => x.Quantity);
            valuesToWrite["E49"] = orskData[5].Sum(x => x.Quantity);
            valuesToWrite["G49"] = orskData[5].Sum(x => x.AmountWithoutVAT);
            valuesToWrite["I49"] = orskData[5].Sum(x => x.AmountWithoutVAT);

            // Таблица 3: Орск -- Пар Проч.
            valuesToWrite["C51"] = orskData[3].Sum(x => x.Quantity);
            valuesToWrite["D51"] = orskData[3].Sum(x => x.QuantityByMeter);
            valuesToWrite["G51"] = orskData[3].Sum(x => x.AmountWithoutVAT);
            valuesToWrite["H51"] = orskData[3].Sum(x => x.AmountByMeter);




            // Таблица 4: Каргала -- Пар с коллекторов
            valuesToWrite["C66"] = kargalaData[0].Sum(x => x.QuantityByMeter);
            valuesToWrite["D66"] = kargalaData[0].Sum(x => x.QuantityByMeter);
            valuesToWrite["G66"] = kargalaData[3].Sum(x => x.AmountWithoutVAT);
            valuesToWrite["H66"] = kargalaData[3].Sum(x => x.AmountWithoutVAT);

            // Таблица 4: Каргала -- Пар Проч
            valuesToWrite["C59"] = kargalaData[2].Sum(x => x.Quantity);
            valuesToWrite["D59"] = kargalaData[2].Sum(x => x.Quantity);
            valuesToWrite["G59"] = kargalaData[2].Sum(x => x.AmountWithoutVAT);
            valuesToWrite["H59"] = kargalaData[2].Sum(x => x.AmountWithoutVAT);

            // Таблица 4: Каргала -- Пар с коллекторов
            valuesToWrite["C56"] = kargalaData[1].Sum(x => x.Quantity);
            valuesToWrite["D56"] = kargalaData[1].Sum(x => x.Quantity);
            valuesToWrite["G56"] = kargalaData[1].Sum(x => x.AmountWithoutVAT);
            valuesToWrite["H56"] = kargalaData[1].Sum(x => x.AmountWithoutVAT);



            // Создаем итоговую форму
            CreateReportFromTemplate(templatePath, newFilePath, valuesToWrite);
        }


        // Запуск
        private void start_Click(object sender, RoutedEventArgs e)
        {
            bool isWinterPeriod = IsWinter.IsChecked ?? false;
            decimal year = 0;

            if (Year.SelectedItem != null)
                year = Convert.ToDecimal(Year.SelectedItem);

            if (string.IsNullOrEmpty(_selectedExcelPath))
            {
                MessageBox.Show("Сначала выберите файл накладной!", "Внимание",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!File.Exists(_selectedExcelPath))
            {
                MessageBox.Show($"Файл не найден:\n{_selectedExcelPath}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string excelFilePath = _selectedExcelPath;

            // Читает накладную и сохраняет в список
            var excelReader = new ExcelReaderService();
            List<ExportDataTable> dataList = excelReader.ReadExcelFile(excelFilePath);


            // фильтрация
            #region OrenFilter

            // Фильтрация -- Орен.Проч(пересчитать)//////////////////////////////////
            var filteredDataOrenProch = dataList.Where(x =>
                x.Department == "Оренбургский филиал ПАО \"Т Плюс\" г. Оренбург" &&
                (x.RecalculationYear == year || x.RecalculationYear == 0) &&
                x.HeatSource != "КТЭЦ" &&
                x.NomenclatureUnit != "м3" &&
                (x.ContractClassifier.StartsWith("3") ||
                 x.ContractClassifier.StartsWith("6") ||
                 x.ContractClassifier.StartsWith("8")) &&
                x.Nomenclature != "612 ТЭ в горячей воде для компенс. потерь" &&
                x.Nomenclature != "Хоз. нужды (тепловая энергия)" &&
                x.LoadType != "Технология" &&
                !string.IsNullOrWhiteSpace(x.LoadType) &&
                !string.IsNullOrEmpty(x.Department))
                .ToList();

            
            
                // Фильтрация -- Орен.На гвс Гкал (пересчитать)//////////////////////////////
            var filteredDataOrenNaselenGVS_GCal = dataList.Where(x =>
                x.Department == "Оренбургский филиал ПАО \"Т Плюс\" г. Оренбург" &&
                (x.RecalculationYear == year || x.RecalculationYear == 0) &&
                x.HeatSource != "КТЭЦ" &&
                x.NomenclatureUnit != "м3" &&
                (x.ContractClassifier.StartsWith("1") ||
                x.ContractClassifier.StartsWith("2")) &&
                x.Nomenclature != "612 ТЭ в горячей воде для компенс. потерь" &&
                x.Nomenclature != "Хоз. нужды (тепловая энергия)" &&
                (x.LoadType == "Горячее водоснабжение" ||
                x.LoadType == "Подогрев ХВ"))/// 
                .ToList();

            // Фильтрация -- Орен.На гвс Деньги(пересчитать)//////////////////////////////
            var filteredDataOrenNaselenGVS_Money = dataList.Where(x =>
                x.Department == "Оренбургский филиал ПАО \"Т Плюс\" г. Оренбург" &&
                (x.RecalculationYear == year || x.RecalculationYear == 0) &&
                x.HeatSource != "КТЭЦ" &&
                x.NomenclatureUnit != "м3" &&
                (x.ContractClassifier.StartsWith("1") ||
                x.ContractClassifier.StartsWith("2")) &&
                (x.Nomenclature != "612 ТЭ в горячей воде для компенс. потерь" &&
                x.Nomenclature != "Хоз. нужды (тепловая энергия)") &&
                (x.LoadType == "Горячее водоснабжение" ||
                x.LoadType == "Подогрев ХВ" ||
                x.LoadType == ""))/// 
                .ToList();



            // Фильтрация -- Орен.Бюдж(пересчитать)
            var filteredDataOrenBujet = dataList.Where(x =>
                (x.RecalculationYear == year || x.RecalculationYear == 0) &&
                x.Department == "Оренбургский филиал ПАО \"Т Плюс\" г. Оренбург" &&
                x.HeatSource != "КТЭЦ" &&
                x.NomenclatureUnit != "м3" &&
                x.ContractClassifier.StartsWith("4") &&
                !x.Nomenclature.Contains("пар") &&
                x.Department != "")
                .ToList();

            // Фильтрация -- Орен.Компенс(пересчитать)
            var filteredDataOrenKompens = dataList.Where(x =>
                (x.RecalculationYear == year || x.RecalculationYear == 0) &&
                x.Department == "Оренбургский филиал ПАО \"Т Плюс\" г. Оренбург" &&
                x.HeatSource != "КТЭЦ" &&
                x.NomenclatureUnit != "м3" &&
                 x.Nomenclature == "612 ТЭ в горячей воде для компенс. потерь" &&
                 x.Department != "")
                .ToList();

            
            // Фильтрация -- Орен.На ОТОПЛ (пересчитать)////////////////////////////
            var filteredDataOrenNaselenOTOPL = dataList.Where(x =>
                x.Department == "Оренбургский филиал ПАО \"Т Плюс\" г. Оренбург" &&
                (x.RecalculationYear == year || x.RecalculationYear == 0) &&
                x.HeatSource != "КТЭЦ" &&
                x.NomenclatureUnit != "м3" &&
                (x.ContractClassifier.StartsWith("1") ||
                x.ContractClassifier.StartsWith("2")) &&
                x.LoadType == "Отопление" &&
                x.Nomenclature != "612 ТЭ в горячей воде для компенс. потерь")
                .ToList();
            
            // Фильтрация -- Орен.На ОТОПЛ (пересчитать)////////////////////////////
            var filteredDataOrenNaselenOTOPL_Winter = dataList.Where(x =>
                x.Department == "Оренбургский филиал ПАО \"Т Плюс\" г. Оренбург" &&
                (x.RecalculationYear == year || x.RecalculationYear == 0) &&
                x.HeatSource != "КТЭЦ" &&
                x.NomenclatureUnit != "м3" &&
                (x.ContractClassifier.StartsWith("1") ||
                x.ContractClassifier.StartsWith("2")) &&
                (x.LoadType == "Отопление" ||
                x.LoadType == "Потери теплоэнергии") &&
                x.Nomenclature != "612 ТЭ в горячей воде для компенс. потерь")
                .ToList();
            
            

            // Фильтрация -- Орен.Пар проч коллекторы(пересчитать)/////////////////////////////
            var filteredDataOrenParProch = dataList.Where(x =>
                x.Department == "Оренбургский филиал ПАО \"Т Плюс\" г. Оренбург" &&
                (x.RecalculationYear == 0 || x.RecalculationYear == year) &&
                x.HeatSource != "КТЭЦ" &&
                x.NomenclatureUnit != "м3" &&
                x.Nomenclature.Contains("пар") &&
                x.HeatSource == "СТЭЦ" &&
                x.ContractClassifier.StartsWith("3"))
                .ToList();

            // Фильтрация -- Орен.Пар Бюдж. сеть.(пересчитать)///////////////////////
            var filteredDataOrenParBujetOrg = dataList.Where(x =>
                x.Department == "Оренбургский филиал ПАО \"Т Плюс\" г. Оренбург" &&
                (x.RecalculationYear == 0 || x.RecalculationYear == year) &&
                x.HeatSource != "КТЭЦ" &&
                x.NomenclatureUnit != "м3" &&
                x.ContractClassifier.StartsWith("4") &&
                x.Nomenclature.Contains("пар"))
                .ToList();

            // Фильтрация -- Орен.Пар проч(пересчитать)/////////////////////////////
            var filteredDataOrenParProchSetevoy = dataList.Where(x =>
                x.Department == "Оренбургский филиал ПАО \"Т Плюс\" г. Оренбург" &&
                (x.RecalculationYear == 0 || x.RecalculationYear == year) &&
                x.HeatSource != "КТЭЦ" &&
                x.NomenclatureUnit != "м3" &&
                x.Nomenclature == "Т/Э  в паре давление от 2.5 до 7 (кг/см2)" &&
                (x.ContractClassifier.StartsWith("3") ||
                x.ContractClassifier.StartsWith("6") ||
                x.ContractClassifier.StartsWith("8")) &&
                x.HeatSource != "СТЭЦ")
                .ToList();

            #endregion

            #region MednogFilter

            // фильтрация -- Медн.Проч(пересчитать)
            var filteredDataMednogProch = dataList.Where(x =>
                x.Department == "Оренбургский филиал ПАО \"Т Плюс\" г. Медногорск" &&
                (x.RecalculationYear == 0 || x.RecalculationYear == year) &&
                x.NomenclatureUnit != "м3" &&
                x.Nomenclature != "Хоз. нужды (тепловая энергия)" &&
                (x.ContractClassifier.StartsWith("3") ||
                x.ContractClassifier.StartsWith("6") ||
                x.ContractClassifier.StartsWith("8")))
                .ToList();

            // фильтрация -- Медн.На ГВС(пересчитать)/////////////////////////////////
            var filteredDataMednogNaselenGVS = dataList.Where(x =>
                x.Department == "Оренбургский филиал ПАО \"Т Плюс\" г. Медногорск" &&
                (x.RecalculationYear == 0 || x.RecalculationYear == year) &&
                x.NomenclatureUnit != "м3" &&
                (x.ContractClassifier.StartsWith("1") ||
                x.ContractClassifier.StartsWith("2")) &&
                x.Nomenclature != "Хоз. нужды (тепловая энергия)" &&
                (x.LoadType == "Подогрев ХВ" ||
                x.LoadType == "Горячее водоснабжение"))
                .ToList();

            
            // фильтрация -- Медн.На ОТОПЛ(пересчитать)////////////////////////////////
            var filteredDataMednogNaselenOTOPL_Winter = dataList.Where(x =>
                x.Department == "Оренбургский филиал ПАО \"Т Плюс\" г. Медногорск" &&
                (x.RecalculationYear == 0 || x.RecalculationYear == year) &&
                x.NomenclatureUnit != "м3" &&
                (x.ContractClassifier.StartsWith("1") ||
                x.ContractClassifier.StartsWith("2")) &&
                x.Nomenclature != "Хоз. нужды (тепловая энергия)" &&
                (x.LoadType == "Отопление" ||
                x.LoadType == "Потери теплоэнергии"))
                .ToList();
            // фильтрация -- Медн.На ОТОПЛ(пересчитать)////////////////////////////////
            var filteredDataMednogNaselenOTOPL = dataList.Where(x =>
                x.Department == "Оренбургский филиал ПАО \"Т Плюс\" г. Медногорск" &&
                (x.RecalculationYear == 0 || x.RecalculationYear == year) &&
                x.NomenclatureUnit != "м3" &&
                (x.ContractClassifier.StartsWith("1") ||
                x.ContractClassifier.StartsWith("2")) &&
                x.Nomenclature != "Хоз. нужды (тепловая энергия)" &&
                x.LoadType == "Отопление")
                .ToList();
            
            

            // Фильтрация -- Медн.Бюдж(пересчитать)
            var filteredDataMednogBujet = dataList.Where(x =>
                (x.RecalculationYear == year || x.RecalculationYear == 0) &&
                x.Department == "Оренбургский филиал ПАО \"Т Плюс\" г. Медногорск" &&
                x.NomenclatureUnit != "м3" &&
                x.ContractClassifier.StartsWith("4") &&
                 x.Department != "")///////////////////////
                .ToList();

            #endregion

            #region OrskFilter

            // фильтрация -- Орск.Проч Коллекторы(пересчитать)/////////////////////////////////////////
            var filteredDataOrskProchCollect = dataList.Where(x =>
                x.Department == "Оренбургский филиал ПАО \"Т Плюс\" г. Орск" &&
                (x.RecalculationYear == 0 || x.RecalculationYear == year) &&
                x.NomenclatureUnit != "м3" &&
                (x.Tariff == "Потребители ОрТЭЦ на коллекторах (СЦ)" ||////////////////
                x.Tariff == "Потребители ОрТЭЦ на коллекторах ЦЗ П 1.1") &&////////////////
                (x.ContractClassifier.StartsWith("3") ||
                x.ContractClassifier.StartsWith("6") ||
                x.ContractClassifier.StartsWith("8")))
                .ToList();

            // фильтрация -- Орск.Другие орг. Коллекторы(пересчитать)////////////////////////////////////
            var filteredDataOrskOtherCollect = dataList.Where(x =>
                x.Department == "Оренбургский филиал ПАО \"Т Плюс\" г. Орск" &&
                (x.RecalculationYear == 0 || x.RecalculationYear == year) &&
                x.NomenclatureUnit != "м3" &&
                (x.Tariff == "Потребители ОрТЭЦ на коллекторах (СЦ)" ||//////////////////
                x.Tariff == "Потребители ОрТЭЦ на коллекторах ЦЗ П 1.1") &&//////////////////
                x.ContractClassifier.StartsWith("5"))
                .ToList();

            // фильтрация -- Орск.Проч(пересчитать)
            var filteredDataOrskProch = dataList.Where(x =>
                x.Department == "Оренбургский филиал ПАО \"Т Плюс\" г. Орск" &&
                (x.RecalculationYear == 0 || x.RecalculationYear == year) &&
                x.NomenclatureUnit != "м3" &&
                (x.ContractClassifier.StartsWith("3") ||
                 x.ContractClassifier.StartsWith("6") ||
                 x.ContractClassifier.StartsWith("8")) &&
                !x.Nomenclature.Contains("пар") &&
                !x.Tariff.Contains("коллектор") &&
                !x.Tariff.Contains("кол-р"))
                .ToList();

            // фильтрация -- Орск.На ГВС(пересчитать)
            var filteredDataOrskNaselenGVS = dataList.Where(x =>
                x.Department == "Оренбургский филиал ПАО \"Т Плюс\" г. Орск" &&
                (x.RecalculationYear == 0 || x.RecalculationYear == year) &&
                x.NomenclatureUnit != "м3" &&
                x.Nomenclature != "Хоз. нужды (тепловая энергия)" &&
                (x.ContractClassifier.StartsWith("1") ||
                x.ContractClassifier.StartsWith("2")) &&
                (x.LoadType == "Подогрев ХВ" ||
                x.LoadType == "Горячее водоснабжение"))
                .ToList();


            
            // фильтрация -- Орск.На ОТОПЛ(пересчитать)
            var filteredDataOrskNaselenOTOPL_Winter = dataList.Where(x =>
                x.Department == "Оренбургский филиал ПАО \"Т Плюс\" г. Орск" &&
                (x.RecalculationYear == 0 || x.RecalculationYear == year) &&
                x.NomenclatureUnit != "м3" &&
                x.Nomenclature != "Хоз. нужды (тепловая энергия)" &&
                (x.ContractClassifier.StartsWith("1") ||
                x.ContractClassifier.StartsWith("2")) &&
                (x.LoadType == "Отопление" ||
                x.LoadType == "Потери теплоэнергии"))
                .ToList();
            // фильтрация -- Орск.На ОТОПЛ(пересчитать)
            var filteredDataOrskNaselenOTOPL = dataList.Where(x =>
                x.Department == "Оренбургский филиал ПАО \"Т Плюс\" г. Орск" &&
                (x.RecalculationYear == 0 || x.RecalculationYear == year) &&
                x.NomenclatureUnit != "м3" &&
                x.Nomenclature != "Хоз. нужды (тепловая энергия)" &&
                (x.ContractClassifier.StartsWith("1") ||
                x.ContractClassifier.StartsWith("2")) &&
                x.LoadType == "Отопление")
                .ToList();
            
            

            // Фильтрация -- Орск.Бюдж(пересчитать)
            var filteredDataOrskBujet = dataList.Where(x =>
                (x.RecalculationYear == year || x.RecalculationYear == 0) &&
                x.Department == "Оренбургский филиал ПАО \"Т Плюс\" г. Орск" &&
                x.NomenclatureUnit != "м3" &&
                x.ContractClassifier.StartsWith("4") &&
                 x.Department != "")
                .ToList();

            // Фильтрация -- Орск.Компенсац(пересчитать)///////////////////////////////////////////
            var filteredDataOrskCompens = dataList.Where(x =>
                x.Department == "Оренбургский филиал ПАО \"Т Плюс\" г. Орск" &&
                (x.RecalculationYear == 0 || x.RecalculationYear == year) &&
                x.NomenclatureUnit != "м3" &&
                (x.Tariff.Contains("потер")))////////////////
                .ToList();

            // Фильтрация -- Орск.Пар проч(пересчитать)
            var filteredDataOrskParProch = dataList.Where(x =>
                x.Department == "Оренбургский филиал ПАО \"Т Плюс\" г. Орск" &&
                (x.RecalculationYear == 0 || x.RecalculationYear == year) &&
                x.NomenclatureUnit != "м3" &&
                x.ContractClassifier.StartsWith("3") &&
                x.Nomenclature.Contains("пар"))
                .ToList();

            #endregion

            #region KargalaFilter

            // Фильтрация -- Каргала.Отпуск ТЭ коллекторы(пересчитать)
            var filteredDataKargalaOtpuskCollect = dataList.Where(x =>
                x.Department == "Оренбургский филиал ПАО \"Т Плюс\" г. Оренбург" &&
                (x.RecalculationYear == 0 || x.RecalculationYear == year) &&
                x.HeatSource == "КТЭЦ" &&
                x.NomenclatureUnit == "Гкал" &&
                x.Nomenclature != "Услуги по передаче" &&
                x.Tariff.Contains("Компенсация потерь ТСО от КТЭЦ"))//////////////////////////////////////////
                .ToList();

            // Фильтрация -- Каргала.Отпуск ТЭ проч(пересчитать)
            var filteredDataKargalaProch = dataList.Where(x =>
                x.Department == "Оренбургский филиал ПАО \"Т Плюс\" г. Оренбург" &&
                (x.RecalculationYear == 0 || x.RecalculationYear == year) &&
                x.HeatSource == "КТЭЦ" &&
                x.NomenclatureUnit == "Гкал" &&
                x.Nomenclature != "Услуги по передаче" &&
                x.Tariff.Contains("сети ГПЗ"))//////////////////////////////////////////
                .ToList();

            // Фильтрация -- Каргала.Пар с коллекторов Гкал(пересчитать)
            var filteredDataKargalaOtpuskParCollectGCal = dataList.Where(x =>
                x.Department == "Оренбургский филиал ПАО \"Т Плюс\" г. Оренбург" &&
                (x.RecalculationYear == 0 || x.RecalculationYear == year) &&
                x.HeatSource == "КТЭЦ" &&
                x.NomenclatureUnit == "Гкал" &&
                (x.Nomenclature.Contains("пар") ||
                x.Nomenclature.Contains("конденсат")))//////////////////////////////////////////
                .ToList();

            // Фильтрация -- Каргала.Пар с коллекторов Деньги(пересчитать)
            var filteredDataKargalaOtpuskParCollectMoney = dataList.Where(x =>
                x.Department == "Оренбургский филиал ПАО \"Т Плюс\" г. Оренбург" &&
                (x.RecalculationYear == 0 || x.RecalculationYear == year) &&
                x.HeatSource == "КТЭЦ" &&
                (x.NomenclatureUnit == "Гкал" ||
                x.NomenclatureUnit == "Гкал/ч") &&
                (x.Nomenclature.Contains("пар") ||
                x.Nomenclature.Contains("конденсат") ||
                x.Nomenclature.Contains("ощность")))//////////////////////////////////////////
                .ToList();

            #endregion


            // Путь до шаблона и итоговой формы
            string templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Ex", "Form46Sample(2).xlsx");

            // Диалог сохранения файла
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*",
                Title = "Сохранить отчет Форма 46",
                FileName = $"form46_{DateTime.Now:yyyy-MM-dd}.xlsx",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                DefaultExt = ".xlsx"
            };


            // отфильтрованные данные распределены по городам
            #region OrenList

            List<List<ExportDataTable>> orenList = new List<List<ExportDataTable>>();
            orenList.Add(filteredDataOrenProch);
            orenList.Add(filteredDataOrenNaselenGVS_GCal);
            if (isWinterPeriod)
                orenList.Add(filteredDataOrenNaselenOTOPL_Winter);
            else
                orenList.Add(filteredDataOrenNaselenOTOPL);
            orenList.Add(filteredDataOrenKompens);
            orenList.Add(filteredDataOrenParProch);
            orenList.Add(filteredDataOrenParBujetOrg);
            orenList.Add(filteredDataOrenBujet);
            orenList.Add(filteredDataOrenParProchSetevoy);
            orenList.Add(filteredDataOrenNaselenGVS_Money);

            #endregion

            #region MednoList

            List<List<ExportDataTable>> MednoList = new List<List<ExportDataTable>>();
            MednoList.Add(filteredDataMednogProch);
            MednoList.Add(filteredDataMednogNaselenGVS);
            if (isWinterPeriod)
                MednoList.Add(filteredDataMednogNaselenOTOPL_Winter);
            else
                MednoList.Add(filteredDataMednogNaselenOTOPL);
            MednoList.Add(filteredDataMednogBujet);

            #endregion

            #region OrskList

            List<List<ExportDataTable>> orskList = new List<List<ExportDataTable>>();
            orskList.Add(filteredDataOrskProch);
            orskList.Add(filteredDataOrskNaselenGVS);
            if (isWinterPeriod)
                orskList.Add(filteredDataOrskNaselenOTOPL_Winter);
            else
                orskList.Add(filteredDataOrskNaselenOTOPL);
            orskList.Add(filteredDataOrskParProch);
            orskList.Add(filteredDataOrskBujet);
            orskList.Add(filteredDataOrskCompens);
            orskList.Add(filteredDataOrskProchCollect);
            orskList.Add(filteredDataOrskOtherCollect);

            #endregion

            #region KargalaList

            List<List<ExportDataTable>> kargalaList = new List<List<ExportDataTable>>();
            kargalaList.Add(filteredDataKargalaOtpuskParCollectGCal);
            kargalaList.Add(filteredDataKargalaOtpuskCollect);
            kargalaList.Add(filteredDataKargalaProch);
            kargalaList.Add(filteredDataKargalaOtpuskParCollectMoney);

            #endregion



            // отладка
            /*
            // Вывод результатов в консоль для отладки
            Console.WriteLine($"Найдено записей: {filteredDataOrenNaselenGVS.Count}");
            Console.WriteLine("==========================================");
            for (int i = 0; i < filteredDataOrenNaselenGVS.Count; i++)
            {
                Console.WriteLine(filteredDataOrenNaselenGVS[i].ToString());
            }

            // для отладки
            #region CalcOren

            // Вычисление итоговых значений Орен.Проч(всего, по ПУ)
            CalculateSum(filteredDataOrenProch);

            // Вычисление итоговых значений Орен.На гвс(всего, по ПУ)
            CalculateSum(filteredDataOrenNaselenGVS);
            // Вычисление итоговых значений Орен.ОТОПЛ(всего, по ПУ)
            if (isWinterPeriod)
                CalculateSum(filteredDataOrenNaselenOTOPL_Winter);
            else
                CalculateSum(filteredDataOrenNaselenOTOPL);

            // Вычисление итоговых значений Орен.Бюдж>(всего, по ПУ)
            CalculateSum(filteredDataOrenBujet);

            // Вычисление итоговых значений Орен.Компенс(всего, по ПУ)
            CalculateSum(filteredDataOrenKompens);

            // Вычисление итоговых значений Орен.Пар проч(всего, по ПУ)
            CalculateSum(filteredDataOrenParProch);

            // Вычисление итоговых значений Орен.Пар Бюдж. орг.(всего, по ПУ)
            CalculateSum(filteredDataOrenParBujetOrg);
            // Вычисление итоговых значений Орен.Пар Бюдж. сеть.(всего, по ПУ)
            CalculateSum(filteredDataOrenParProchSetevoy);

            #endregion

            #region CalcMednog

            // Вычисление итоговых значений Медн.Проч(всего, по ПУ)
            CalculateSum(filteredDataMednogProch);

            // Вычисление итоговых значений Медн.На ГВС(всего, по ПУ)
            CalculateSum(filteredDataMednogNaselenGVS);
            // Вычисление итоговых значений Медн.На ОТОПЛ(всего, по ПУ)
            if (isWinterPeriod)
                CalculateSum(filteredDataMednogNaselenOTOPL_Winter);
            else
                CalculateSum(filteredDataMednogNaselenOTOPL);

            // Вычисление итоговых значений Медн.Бюдж(всего, по ПУ)
            CalculateSum(filteredDataMednogBujet);

            #endregion

            #region CalcOrsk

            // Вычисление итоговых значений Орск.Проч коллект.(всего, по ПУ)
            CalculateSum(filteredDataOrskProchCollect);

            // Вычисление итоговых значений Орск.другие коллект.(всего, по ПУ)
            CalculateSum(filteredDataOrskOtherCollect);

            // Вычисление итоговых значений Орск.Проч(всего, по ПУ)
            CalculateSum(filteredDataOrskProch);

            // Вычисление итоговых значений Орск.ГВС(всего, по ПУ)
            CalculateSum(filteredDataOrskNaselenGVS);
            // Вычисление итоговых значений Орск.ОТОПЛ(всего, по ПУ)
            if (isWinterPeriod)
                CalculateSum(filteredDataOrskNaselenOTOPL_Winter);
            else
                CalculateSum(filteredDataOrskNaselenOTOPL);

            // Вычисление итоговых значений Орск. Бюдж(всего, по ПУ)
            CalculateSum(filteredDataOrskBujet);

            // Вычисление итоговых значений Орск. Компенс(всего, по ПУ)
            CalculateSum(filteredDataOrskCompens);

            // Вычисление итоговых значений Орск.Пар проч(всего, по ПУ)
            CalculateSum(filteredDataOrskParProch);

            #endregion

            #region CalcKargala

            // Вычисление итоговых значений Каргала.Пар с коллекторов(всего, по ПУ)
            CalculateSum(filteredDataKargalaOtpuskCollect);

            CalculateSum(filteredDataKargalaOtpuskParCollect);

            CalculateSum(filteredDataKargalaProch);

            #endregion

            // для отладки
            decimal sum = 0;
            decimal sumByMeter = 0;
            decimal money = 0;
            decimal moneyByMeter = 0;
            for (int i = 0; i < filteredDataOrenNaselenGVS.Count; i++)
            {

                sum += filteredDataOrenNaselenGVS[i].Quantity;
                sumByMeter += filteredDataOrenNaselenGVS[i].QuantityByMeter;
                money += filteredDataOrenNaselenGVS[i].AmountWithoutVAT;
                moneyByMeter += filteredDataOrenNaselenGVS[i].AmountByMeter;
            }
            Console.WriteLine($"{sum}\t{sumByMeter}\n");

            */



            // Вставка. Показываем диалог и ждем выбора пользователя
            if (saveFileDialog.ShowDialog() == true)
            {
                string newFilePath = saveFileDialog.FileName;

                // Создаем отчет
                FillAllTables(templatePath, newFilePath, orenList, MednoList, orskList, kargalaList);


                // Открываем файл
                try
                {
                    System.Diagnostics.Process.Start(newFilePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Не удалось открыть файл: {ex.Message}",
                                  "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Сохранение отменено пользователем",
                              "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        //// для отладки
        //public void CalculateSum(List<ExportDataTable> filteredList)
        //{
        //    decimal sum = 0;
        //    decimal sumByMeter = 0;
        //    decimal money = 0;
        //    decimal moneyByMeter = 0;

        //    for (int i = 0; i < filteredList.Count; i++)
        //    {

        //        sum += filteredList[i].Quantity;
        //        sumByMeter += filteredList[i].QuantityByMeter;
        //        money += filteredList[i].AmountWithoutVAT;
        //        moneyByMeter += filteredList[i].AmountByMeter;
        //    }
        //}

    }
}


