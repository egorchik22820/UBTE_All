using PotrebAuto.Configuration;
using PotrebAuto.Configuration.jsonModels;
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
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace PotrebAuto.Windows
{
    /// <summary>
    /// Логика взаимодействия для SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            Loaded += SettingsWindow_Loaded; // Подписываемся на событие загрузки окна
        }

        private void SettingsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Проверяем существование файлов перед загрузкой
            CheckConfigFiles();
            LoadAllConfigurations();
        }

        // Метод для проверки валидности всех TextBox'ов
        private bool AreAllTextboxesValid()
        {
            // Проверяем все TextBox'ы в окне
            foreach (var textBox in FindVisualChildren<TextBox>(this))
            {
                if (Validation.GetHasError(textBox))
                    return false;

                if (string.IsNullOrEmpty(textBox.Text) || !int.TryParse(textBox.Text, out int value) || value < 0 || value >= 200)
                    return false;
            }
            return true;
        }

        // Вспомогательный метод для поиска всех дочерних элементов определенного типа
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        private void CheckConfigFiles()
        {
            string[] configPaths = {
                                        ConfigModel._Constants_ConfigPath,
                                        ConfigModel._Consumers_ConfigPath,
                                        ConfigModel._Consumers_2_ConfigPath,
                                        ConfigModel._SAC_ConfigPath,
                                        ConfigModel._GiT_ConfigPath,
                                        ConfigModel._Qlick_ConfigPath
                                    };

            foreach (string path in configPaths)
            {
                if (!File.Exists(path))
                {
                    System.Diagnostics.Debug.WriteLine($"Файл не найден: {path}");
                    // Создаем директорию если её нет
                    string directory = Path.GetDirectoryName(path);
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Файл найден: {path}");
                }
            }
        }



        private void LoadAllConfigurations()
        {
            try
            {
                // Загружаем все конфигурации
                ConfigModel.LoadAllConfigurations();

                // Заполняем поля для ConsumersData вкладки
                NumberTextBox.Text = ConfigModel.ConsumersConf.Number.ToString();
                AddressTextBox.Text = ConfigModel.ConsumersConf.Address.ToString();
                IdTextBox.Text = ConfigModel.ConsumersConf.Id.ToString();
                PU_GcalTotalTextBox.Text = ConfigModel.ConsumersConf.PU_GcalTotal.ToString();
                PU_WithVNR_GcalTextBox.Text = ConfigModel.ConsumersConf.PU_WithVNR_Gcal.ToString();
                ZM_GcalTotalTextBox.Text = ConfigModel.ConsumersConf.ZM_GcalTotal.ToString();
                ZM_WithAverage_GcalTextBox.Text = ConfigModel.ConsumersConf.ZM_WithAverage_Gcal.ToString();
                WithBS_GcalTextBox.Text = ConfigModel.ConsumersConf.WithBS_Gcal.ToString();
                WithRealLoadBS_GcalTextBox.Text = ConfigModel.ConsumersConf.WithRealLoadBS_Gcal.ToString();
                WithRealLoadTU_GcalTextBox.Text = ConfigModel.ConsumersConf.WithRealLoadTU_Gcal.ToString();
                WithNP_GcalTextBox.Text = ConfigModel.ConsumersConf.WithNP_Gcal.ToString();
                WithCab_GcalTextBox.Text = ConfigModel.ConsumersConf.WithCab_Gcal.ToString();
                WithKSN_GcalTextBox.Text = ConfigModel.ConsumersConf.WithKSN_Gcal.ToString();
                WithRealLoad_FN_GcalTextBox.Text = ConfigModel.ConsumersConf.WithRealLoad_FN_Gcal.ToString();
                WithDocAndKSN_GcalTextBox.Text = ConfigModel.ConsumersConf.WithDocAndKSN_Gcal.ToString();
                WithDoc_GcalTextBox.Text = ConfigModel.ConsumersConf.WithDoc_Gcal.ToString();
                CorrectNotesCountTextBox.Text = ConfigModel.ConsumersConf.CorrectNotesCount.ToString();
                Q_T_M_IsNullTextBox.Text = ConfigModel. ConsumersConf.Q_T_M_IsNull.ToString();
                ActsBSTextBox.Text = ConfigModel.ConsumersConf.ActsBS.ToString();
                Max_Min_Q_MTextBox.Text = ConfigModel.ConsumersConf.Max_Min_Q_M.ToString();
                IsValid_VNRTextBox.Text = ConfigModel.ConsumersConf.IsValid_VNR.ToString();
                IsValid_M1_M2TextBox.Text = ConfigModel.ConsumersConf.IsValid_M1_M2.ToString();
                IsValid_M1_M2_2TextBox.Text = ConfigModel.ConsumersConf.IsValid_M1_M2_2.ToString();
                Q_engTextBox.Text = ConfigModel.ConsumersConf.Q_eng.ToString();
                IsValid_TTextBox.Text = ConfigModel.ConsumersConf.IsValid_T.ToString();

                // Заполняем поля для Consumers_2 вкладки
                Consumers_2AddressTextBox.Text = ConfigModel.Consumers_2Conf.Address.ToString();
                Consumers_2PU_GcalTotalTextBox.Text = ConfigModel.Consumers_2Conf.PU_GcalTotal.ToString();
                Consumers_2ZM_GcalTotalTextBox.Text = ConfigModel.Consumers_2Conf.ZM_GcalTotal.ToString();

                // Заполняем поля для GiT вкладки
                GiTCityTextBox.Text = ConfigModel.GiTConf.City.ToString();
                GiTBuildingIdTextBox.Text = ConfigModel.GiTConf.BuildingId.ToString();
                GiTBuildingTypeTextBox.Text = ConfigModel.GiTConf.BuildingType.ToString();

                // Заполняем поля для Qlick вкладки
                QlickGuidTextBox.Text = ConfigModel.QlickConf.Guid.ToString();
                QlickBuildingIdTextBox.Text = ConfigModel.QlickConf.Id.ToString();

                // Заполняем поля для SourcesAndConsumers вкладки
                TU_IdTextBox.Text = ConfigModel.SACConf.TU_Id.ToString();
                Obj_IdTextBox.Text = ConfigModel.SACConf.Obj_Id.ToString();

                // Заполняем поля для Table Settings вкладки
                ConsumersTableRowStartTextBox.Text = ConfigModel.ConstantsConf.ConsumersTableRowStart.ToString();
                ConsumersDataRowStartTextBox.Text = ConfigModel.ConstantsConf.ConsumersDataRowStart.ToString();

                Consumers_2TableRowStartTextBox.Text = ConfigModel.ConstantsConf.Consumers_2TableRowStart.ToString();
                Consumers_2DataRowStartTextBox.Text = ConfigModel.ConstantsConf.Consumers_2DataRowStart.ToString();

                SACTableRowStartTextBox.Text = ConfigModel.ConstantsConf.SACTableRowStart.ToString();
                SACDataRowStartTextBox.Text = ConfigModel.ConstantsConf.SACDataRowStart.ToString();

                GiTTableRowStartTextBox.Text = ConfigModel.ConstantsConf.GiTTableRowStart.ToString();
                GiTDataRowStartTextBox.Text = ConfigModel.ConstantsConf.GiTDataRowStart.ToString();

                QlickDataRowStartTextBox.Text = ConfigModel.ConstantsConf.QlickDataRowStart.ToString();
                QlickDataColStartTextBox.Text = ConfigModel.ConstantsConf.QlickDataColStart.ToString();

                DatesColStartTextBox.Text = ConfigModel.ConstantsConf.DatesColStart.ToString();
                DatesRowStartTextBox.Text = ConfigModel.ConstantsConf.DatesRowStart.ToString();

                DatesRowStart_2TextBox.Text = ConfigModel.ConstantsConf.Dates_2RowStart.ToString();
                DatesColStart_2TextBox.Text = ConfigModel.ConstantsConf.Dates_2ColStart.ToString();

                DaysInMonthTextBox.Text = ConfigModel.ConstantsConf.DaysInMonth.ToString();

                DaysInMonth_2TextBox.Text = ConfigModel.ConstantsConf.DaysInMonth_2.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке конфигураций: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void SourcesAndConsumersSaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!AreAllTextboxesValid())
            {
                MessageBox.Show("Пожалуйста, исправьте ошибки в полях ввода. Все числа должны быть в диапазоне от 0 до 199.", "Ошибка валидации",
                              MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                var sacConf = new SACConfig
                {
                    TU_Id = int.Parse(TU_IdTextBox.Text),
                    Obj_Id = int.Parse(Obj_IdTextBox.Text)
                };

                ConfigModel.SaveConfig(ConfigModel._SAC_ConfigPath, sacConf);
                MessageBox.Show("Настройки МП сохранены!", "Успех",
                              MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении настроек источников и потребителей: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ConsumersDataSaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!AreAllTextboxesValid())
            {
                MessageBox.Show("Пожалуйста, исправьте ошибки в полях ввода. Все числа должны быть в диапазоне от 0 до 199.", "Ошибка валидации",
                              MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                var consumersConf = new ConsumersConfig
                {
                    Number = int.Parse(NumberTextBox.Text),
                    Address = int.Parse(AddressTextBox.Text),
                    Id = int.Parse(IdTextBox.Text),
                    PU_GcalTotal = int.Parse(PU_GcalTotalTextBox.Text),
                    PU_WithVNR_Gcal = int.Parse(PU_WithVNR_GcalTextBox.Text),
                    ZM_GcalTotal = int.Parse(ZM_GcalTotalTextBox.Text),
                    ZM_WithAverage_Gcal = int.Parse(ZM_WithAverage_GcalTextBox.Text),
                    WithBS_Gcal = int.Parse(WithBS_GcalTextBox.Text),
                    WithRealLoadBS_Gcal = int.Parse(WithRealLoadBS_GcalTextBox.Text),
                    WithRealLoadTU_Gcal = int.Parse(WithRealLoadTU_GcalTextBox.Text),
                    WithNP_Gcal = int.Parse(WithNP_GcalTextBox.Text),
                    WithCab_Gcal = int.Parse(WithCab_GcalTextBox.Text),
                    WithKSN_Gcal = int.Parse(WithKSN_GcalTextBox.Text),
                    WithRealLoad_FN_Gcal = int.Parse(WithRealLoad_FN_GcalTextBox.Text),
                    WithDocAndKSN_Gcal = int.Parse(WithDocAndKSN_GcalTextBox.Text),
                    WithDoc_Gcal = int.Parse(WithDoc_GcalTextBox.Text),
                    CorrectNotesCount = int.Parse(CorrectNotesCountTextBox.Text),
                    Q_T_M_IsNull = int.Parse(Q_T_M_IsNullTextBox.Text),
                    ActsBS = int.Parse(ActsBSTextBox.Text),
                    Max_Min_Q_M = int.Parse(Max_Min_Q_MTextBox.Text),
                    IsValid_VNR = int.Parse(IsValid_VNRTextBox.Text),
                    IsValid_M1_M2 = int.Parse(IsValid_M1_M2TextBox.Text),
                    IsValid_M1_M2_2 = int.Parse(IsValid_M1_M2_2TextBox.Text),
                    Q_eng = int.Parse(Q_engTextBox.Text),
                    IsValid_T = int.Parse(IsValid_TTextBox.Text)
                };

                ConfigModel.SaveConfig(ConfigModel._Consumers_ConfigPath, consumersConf);
                MessageBox.Show("Настройки потребителей сохранены!", "Успех",
                              MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении настроек потребителей: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TableSettingsSaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!AreAllTextboxesValid())
            {
                MessageBox.Show("Пожалуйста, исправьте ошибки в полях ввода. Все числа должны быть в диапазоне от 0 до 199.", "Ошибка валидации",
                              MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                var constConf = new ConstantsConfig
                {
                    ConsumersTableRowStart = int.Parse(ConsumersTableRowStartTextBox.Text),
                    ConsumersDataRowStart = int.Parse(ConsumersDataRowStartTextBox.Text),

                    Consumers_2DataRowStart = int.Parse(Consumers_2DataRowStartTextBox.Text),
                    Consumers_2TableRowStart = int.Parse(Consumers_2TableRowStartTextBox.Text),

                    SACTableRowStart = int.Parse(SACTableRowStartTextBox.Text),
                    SACDataRowStart = int.Parse(SACDataRowStartTextBox.Text),

                    GiTDataRowStart = int.Parse(GiTDataRowStartTextBox.Text),
                    GiTTableRowStart = int.Parse(GiTTableRowStartTextBox.Text),

                    QlickDataRowStart = int.Parse(QlickDataRowStartTextBox.Text),
                    QlickDataColStart = int.Parse(QlickDataColStartTextBox.Text),

                    DatesColStart = int.Parse(DatesColStartTextBox.Text),
                    DatesRowStart = int.Parse(DatesRowStartTextBox.Text),

                    Dates_2ColStart = int.Parse(DatesColStart_2TextBox.Text),
                    Dates_2RowStart = int.Parse(DatesRowStart_2TextBox.Text),

                    DaysInMonth = int.Parse(DaysInMonthTextBox.Text),

                    DaysInMonth_2 = int.Parse(DaysInMonth_2TextBox.Text)

                    
                };

                ConfigModel.SaveConfig(ConfigModel._Constants_ConfigPath, constConf);
                MessageBox.Show("Настройки таблиц сохранены!", "Успех",
                              MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении настроек таблиц: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GiTSaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!AreAllTextboxesValid())
            {
                MessageBox.Show("Пожалуйста, исправьте ошибки в полях ввода. Все числа должны быть в диапазоне от 0 до 199.", "Ошибка валидации",
                              MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                var GiTConf = new GiTConfig
                {
                    City = int.Parse(GiTCityTextBox.Text),
                    BuildingId = int.Parse(GiTBuildingIdTextBox.Text),
                    BuildingType = int.Parse(GiTBuildingTypeTextBox.Text)
                };

                ConfigModel.SaveConfig(ConfigModel._GiT_ConfigPath, GiTConf);
                MessageBox.Show("Настройки таблиц сохранены!", "Успех",
                              MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении настроек ГиТ: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void QlickSaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!AreAllTextboxesValid())
            {
                MessageBox.Show("Пожалуйста, исправьте ошибки в полях ввода. Все числа должны быть в диапазоне от 0 до 199.", "Ошибка валидации",
                              MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                var QlickConf = new QlickConfig
                {
                    Guid = int.Parse(QlickGuidTextBox.Text),
                    Id = int.Parse(QlickBuildingIdTextBox.Text)
                };

                ConfigModel.SaveConfig(ConfigModel._Qlick_ConfigPath, QlickConf);
                MessageBox.Show("Настройки таблиц сохранены!", "Успех",
                              MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении настроек Qlick: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Consumers_2SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!AreAllTextboxesValid())
            {
                MessageBox.Show("Пожалуйста, исправьте ошибки в полях ввода. Все числа должны быть в диапазоне от 0 до 199.", "Ошибка валидации",
                              MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                var consumers_2Config = new Consumers_2Config
                {
                    Address = int.Parse(Consumers_2AddressTextBox.Text),
                    PU_GcalTotal = int.Parse(Consumers_2PU_GcalTotalTextBox.Text),
                    ZM_GcalTotal = int.Parse(Consumers_2ZM_GcalTotalTextBox.Text)
                };

                ConfigModel.SaveConfig(ConfigModel._Consumers_2_ConfigPath, consumers_2Config);
                MessageBox.Show("Настройки таблиц сохранены!", "Успех",
                              MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении настроек доп. файла потребителей: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
