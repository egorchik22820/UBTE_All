using Newtonsoft.Json;
using PoteryGVS.Configuration;
using PoteryGVS.Configuration.jsonModels;
using PoteryGVS.Models;
using PoteryGVS.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PoteryGVS.Windows
{
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
                // Пропускаем TextBox для года (особое поле)
                if (textBox.Name == "NakladCalcYearValueTextBox")
                    continue;

                if (Validation.GetHasError(textBox))
                    return false;

                if (string.IsNullOrEmpty(textBox.Text) || !int.TryParse(textBox.Text, out int value) || value < 0 || value >= 100)
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
                                        ConfigModel._MP_ConfigPath,
                                        ConfigModel._Naklad_ConfigPath,
                                        ConfigModel._ODN_ConfigPath,
                                        ConfigModel._PoteryGVS_ConfigPath
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

                // Заполняем поля для MP вкладки
                MPIsODPUTextBox.Text = ConfigModel.MP.IsODPU.ToString();
                MPSysPU_IdTextBox.Text = ConfigModel.MP.SysPU_Id.ToString();
                MPAddressTextBox.Text = ConfigModel.MP.Address.ToString();
                MPQ_CalcTextBox.Text = ConfigModel.MP.Q_Calc.ToString();
                MPdV_CalcTextBox.Text = ConfigModel.MP.dV_Calc.ToString();
                MPVNR_CalcTextBox.Text = ConfigModel.MP.VNR_Calc.ToString();
                MPLoadTypeTextBox.Text = ConfigModel.MP.LoadType.ToString();
                MPBuildingIdTextBox.Text = ConfigModel.MP.BuildingId.ToString();
                MPTU_AIIS_IdTextBox.Text = ConfigModel.MP.TU_AIIS_Id.ToString();
                MP_StartRowTextBox.Text = ConfigModel.MP.StartRow.ToString();

                // Заполняем поля для Naklad вкладки
                NakladDocTypeTextBox.Text = ConfigModel.Naklad.DocType.ToString();
                NakladNomenclatureTextBox.Text = ConfigModel.Naklad.Nomenclature.ToString();
                NakladTariffTextBox.Text = ConfigModel.Naklad.Tariff.ToString();
                NakladCalcTypeTextBox.Text = ConfigModel.Naklad.CalcType.ToString();
                NakladNomenclatureUnitTextBox.Text = ConfigModel.Naklad.NomenclatureUnit.ToString();
                NakladQuantityTotalTextBox.Text = ConfigModel.Naklad.QuantityTotal.ToString();
                NakladHeatSourseTextBox.Text = ConfigModel.Naklad.HeatSourse.ToString();
                NakladRecalcYearTextBox.Text = ConfigModel.Naklad.RecalcYear.ToString();
                NakladLoadTypeTextBox.Text = ConfigModel.Naklad.LoadType.ToString();
                NakladDepartmentTextBox.Text = ConfigModel.Naklad.Department.ToString();
                NakladAddressTUTextBox.Text = ConfigModel.Naklad.AddressTU.ToString();
                NakladBuildingIdTextBox.Text = ConfigModel.Naklad.BuildingId.ToString();
                NakladBuildingAddressTextBox.Text = ConfigModel.Naklad.BuildingAddress.ToString();
                NakladBuildingTypeTextBox.Text = ConfigModel.Naklad.BuildingType.ToString();
                NakladSpaceTypeTextBox.Text = ConfigModel.Naklad.SpaceType.ToString();
                Naklad_StartRowTextBox.Text = ConfigModel.Naklad.StartRow.ToString();
                NakladCalcYearValueTextBox.Text = ConfigModel.Naklad.RecalcYearValue;

                // Заполняем поля для ODN вкладки
                ODNBuildingIdTextBox.Text = ConfigModel.ODN.BuildingId.ToString();
                ODNNegativeODN_GcalTextBox.Text = ConfigModel.ODN.NegativeODN_Gcal.ToString();
                ODNNegativeODN_m3TextBox.Text = ConfigModel.ODN.NegativeODN_m3.ToString();
                ODN_StartRowTextBox.Text = ConfigModel.ODN.StartRow.ToString();

                // Заполняем поля для PoteryGVS вкладки
                // With ODPU
                WithODPUBuildingIdTextBox.Text = ConfigModel.PoteryGVS.with_odpu.buildingId.ToString();
                WithODPUCityTextBox.Text = ConfigModel.PoteryGVS.with_odpu.city.ToString();
                WithODPUHeatSupplyZoneTextBox.Text = ConfigModel.PoteryGVS.with_odpu.heatSupplyZone.ToString();
                WithODPUZTPTextBox.Text = ConfigModel.PoteryGVS.with_odpu.ztp.ToString();
                WithODPU_StartRowTextBox.Text= ConfigModel.PoteryGVS.with_odpu.StartRow.ToString();

                // Without ODPU
                WithoutODPUBuildingIdTextBox.Text = ConfigModel.PoteryGVS.without_odpu.buildingId.ToString();
                WithoutODPUCityTextBox.Text = ConfigModel.PoteryGVS.without_odpu.city.ToString();
                WithoutODPUHeatSupplyZoneTextBox.Text = ConfigModel.PoteryGVS.without_odpu.heatSupplyZone.ToString();
                WithoutODPUBuildingTypeTextBox.Text = ConfigModel.PoteryGVS.without_odpu.buildingType.ToString();
                WithoutODPUZTPTextBox.Text = ConfigModel.PoteryGVS.without_odpu.ztp.ToString();
                WithOutODPU_StartRowTextBox.Text = ConfigModel.PoteryGVS.without_odpu.StartRow.ToString();

                // With ITP
                WithITPBuildingIdTextBox.Text = ConfigModel.PoteryGVS.with_itp.buildingId.ToString();
                WithITPCityTextBox.Text = ConfigModel.PoteryGVS.with_itp.city.ToString();
                WithITPHeatSupplyZoneTextBox.Text = ConfigModel.PoteryGVS.with_itp.heatSupplyZone.ToString();
                WithITPCalcTypeTextBox.Text = ConfigModel.PoteryGVS.with_itp.calcType.ToString();
                WithITPBuildingTypeTextBox.Text = ConfigModel.PoteryGVS.with_itp.buildingType.ToString();
                WithITPZTPTextBox.Text = ConfigModel.PoteryGVS.with_itp.ztp.ToString();
                WithITP_StartRowTextBox.Text = ConfigModel.PoteryGVS.with_itp.StartRow.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке конфигураций: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Обработчики событий для кнопок сохранения
        private void MPSaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!AreAllTextboxesValid())
            {
                MessageBox.Show("Пожалуйста, исправьте ошибки в полях ввода. Все числа должны быть в диапазоне от 0 до 99.", "Ошибка валидации",
                              MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                var mpConfig = new MPConfig
                {
                    IsODPU = int.Parse(MPIsODPUTextBox.Text),
                    SysPU_Id = int.Parse(MPSysPU_IdTextBox.Text),
                    Address = int.Parse(MPAddressTextBox.Text),
                    Q_Calc = int.Parse(MPQ_CalcTextBox.Text),
                    dV_Calc = int.Parse(MPdV_CalcTextBox.Text),
                    VNR_Calc = int.Parse(MPVNR_CalcTextBox.Text),
                    LoadType = int.Parse(MPLoadTypeTextBox.Text),
                    BuildingId = int.Parse(MPBuildingIdTextBox.Text),
                    TU_AIIS_Id = int.Parse(MPTU_AIIS_IdTextBox.Text),
                    StartRow = int.Parse(MP_StartRowTextBox.Text)
                };

                ConfigModel.SaveConfig(ConfigModel._MP_ConfigPath, mpConfig);
                MessageBox.Show("Настройки МП сохранены!", "Успех",
                              MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении настроек МП: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void NakladSaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!AreAllTextboxesValid())
            {
                MessageBox.Show("Пожалуйста, исправьте ошибки в полях ввода. Все числа должны быть в диапазоне от 0 до 99.", "Ошибка валидации",
                              MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                var nakladConfig = new NakladConfig
                {
                    DocType = int.Parse(NakladDocTypeTextBox.Text),
                    Nomenclature = int.Parse(NakladNomenclatureTextBox.Text),
                    Tariff = int.Parse(NakladTariffTextBox.Text),
                    CalcType = int.Parse(NakladCalcTypeTextBox.Text),
                    NomenclatureUnit = int.Parse(NakladNomenclatureUnitTextBox.Text),
                    QuantityTotal = int.Parse(NakladQuantityTotalTextBox.Text),
                    HeatSourse = int.Parse(NakladHeatSourseTextBox.Text),
                    RecalcYear = int.Parse(NakladRecalcYearTextBox.Text),
                    LoadType = int.Parse(NakladLoadTypeTextBox.Text),
                    Department = int.Parse(NakladDepartmentTextBox.Text),
                    AddressTU = int.Parse(NakladAddressTUTextBox.Text),
                    BuildingId = int.Parse(NakladBuildingIdTextBox.Text),
                    BuildingAddress = int.Parse(NakladBuildingAddressTextBox.Text),
                    BuildingType = int.Parse(NakladBuildingTypeTextBox.Text),
                    SpaceType = int.Parse(NakladSpaceTypeTextBox.Text),
                    StartRow = int.Parse(Naklad_StartRowTextBox.Text),
                    RecalcYearValue = DataServices.ParseStringYear(NakladCalcYearValueTextBox.Text)
                };

                ConfigModel.SaveConfig(ConfigModel._Naklad_ConfigPath, nakladConfig);
                MessageBox.Show("Настройки Накладной сохранены!", "Успех",
                              MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении настроек Накладной: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ODNSaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!AreAllTextboxesValid())
            {
                MessageBox.Show("Пожалуйста, исправьте ошибки в полях ввода. Все числа должны быть в диапазоне от 0 до 99.", "Ошибка валидации",
                              MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                var odnConfig = new ODNConfig
                {
                    BuildingId = int.Parse(ODNBuildingIdTextBox.Text),
                    NegativeODN_Gcal = int.Parse(ODNNegativeODN_GcalTextBox.Text),
                    NegativeODN_m3 = int.Parse(ODNNegativeODN_m3TextBox.Text),
                    StartRow = int.Parse(ODN_StartRowTextBox.Text)
                };

                ConfigModel.SaveConfig(ConfigModel._ODN_ConfigPath, odnConfig);
                MessageBox.Show("Настройки ОДН сохранены!", "Успех",
                              MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении настроек ОДН: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PoteryGVSSaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!AreAllTextboxesValid())
            {
                MessageBox.Show("Пожалуйста, исправьте ошибки в полях ввода. Все числа должны быть в диапазоне от 0 до 99.", "Ошибка валидации",
                              MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                var poteryGVSConfig = new PoteryGVSConfig
                {
                    with_odpu = new WithODPUConfig
                    {
                        buildingId = int.Parse(WithODPUBuildingIdTextBox.Text),
                        city = int.Parse(WithODPUCityTextBox.Text),
                        heatSupplyZone = int.Parse(WithODPUHeatSupplyZoneTextBox.Text),
                        ztp = int.Parse(WithODPUZTPTextBox.Text),
                        StartRow = int.Parse(WithODPU_StartRowTextBox.Text)
                    },
                    without_odpu = new WithoutODPUConfig
                    {
                        buildingId = int.Parse(WithoutODPUBuildingIdTextBox.Text),
                        city = int.Parse(WithoutODPUCityTextBox.Text),
                        heatSupplyZone = int.Parse(WithoutODPUHeatSupplyZoneTextBox.Text),
                        buildingType = int.Parse(WithoutODPUBuildingTypeTextBox.Text),
                        ztp = int.Parse(WithoutODPUZTPTextBox.Text),
                        StartRow = int.Parse(WithOutODPU_StartRowTextBox.Text)
                    },
                    with_itp = new WithITPConfig
                    {
                        buildingId = int.Parse(WithITPBuildingIdTextBox.Text),
                        city = int.Parse(WithITPCityTextBox.Text),
                        heatSupplyZone = int.Parse(WithITPHeatSupplyZoneTextBox.Text),
                        calcType = int.Parse(WithITPCalcTypeTextBox.Text),
                        buildingType = int.Parse(WithITPBuildingTypeTextBox.Text),
                        ztp = int.Parse(WithITPZTPTextBox.Text),
                        StartRow = int.Parse(WithITP_StartRowTextBox.Text)
                    }
                };

                ConfigModel.SaveConfig(ConfigModel._PoteryGVS_ConfigPath, poteryGVSConfig);
                MessageBox.Show("Настройки Потери ГВС сохранены!", "Успех",
                              MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении настроек Потери ГВС: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SatrtRowsSaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!AreAllTextboxesValid())
            {
                MessageBox.Show("Пожалуйста, исправьте ошибки в полях ввода. Все числа должны быть в диапазоне от 0 до 99.", "Ошибка валидации",
                              MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                var mpConfig = new MPConfig
                {
                    IsODPU = int.Parse(MPIsODPUTextBox.Text),
                    SysPU_Id = int.Parse(MPSysPU_IdTextBox.Text),
                    Address = int.Parse(MPAddressTextBox.Text),
                    Q_Calc = int.Parse(MPQ_CalcTextBox.Text),
                    dV_Calc = int.Parse(MPdV_CalcTextBox.Text),
                    VNR_Calc = int.Parse(MPVNR_CalcTextBox.Text),
                    LoadType = int.Parse(MPLoadTypeTextBox.Text),
                    BuildingId = int.Parse(MPBuildingIdTextBox.Text),
                    TU_AIIS_Id = int.Parse(MPTU_AIIS_IdTextBox.Text),
                    StartRow = int.Parse(MP_StartRowTextBox.Text)
                };

                var nakladConfig = new NakladConfig
                {
                    DocType = int.Parse(NakladDocTypeTextBox.Text),
                    Nomenclature = int.Parse(NakladNomenclatureTextBox.Text),
                    Tariff = int.Parse(NakladTariffTextBox.Text),
                    CalcType = int.Parse(NakladCalcTypeTextBox.Text),
                    NomenclatureUnit = int.Parse(NakladNomenclatureUnitTextBox.Text),
                    QuantityTotal = int.Parse(NakladQuantityTotalTextBox.Text),
                    HeatSourse = int.Parse(NakladHeatSourseTextBox.Text),
                    RecalcYear = int.Parse(NakladRecalcYearTextBox.Text),
                    LoadType = int.Parse(NakladLoadTypeTextBox.Text),
                    Department = int.Parse(NakladDepartmentTextBox.Text),
                    AddressTU = int.Parse(NakladAddressTUTextBox.Text),
                    BuildingId = int.Parse(NakladBuildingIdTextBox.Text),
                    BuildingAddress = int.Parse(NakladBuildingAddressTextBox.Text),
                    BuildingType = int.Parse(NakladBuildingTypeTextBox.Text),
                    SpaceType = int.Parse(NakladSpaceTypeTextBox.Text),
                    StartRow = int.Parse(Naklad_StartRowTextBox.Text),
                    RecalcYearValue = DataServices.ParseStringYear(NakladCalcYearValueTextBox.Text)
                };

                var odnConfig = new ODNConfig
                {
                    BuildingId = int.Parse(ODNBuildingIdTextBox.Text),
                    NegativeODN_Gcal = int.Parse(ODNNegativeODN_GcalTextBox.Text),
                    NegativeODN_m3 = int.Parse(ODNNegativeODN_m3TextBox.Text),
                    StartRow = int.Parse(ODN_StartRowTextBox.Text)
                };



                var poteryGVSConfig = new PoteryGVSConfig
                {
                    with_odpu = new WithODPUConfig
                    {
                        buildingId = int.Parse(WithODPUBuildingIdTextBox.Text),
                        city = int.Parse(WithODPUCityTextBox.Text),
                        heatSupplyZone = int.Parse(WithODPUHeatSupplyZoneTextBox.Text),
                        ztp = int.Parse(WithODPUZTPTextBox.Text),
                        StartRow = int.Parse(WithODPU_StartRowTextBox.Text)
                    },
                    without_odpu = new WithoutODPUConfig
                    {
                        buildingId = int.Parse(WithoutODPUBuildingIdTextBox.Text),
                        city = int.Parse(WithoutODPUCityTextBox.Text),
                        heatSupplyZone = int.Parse(WithoutODPUHeatSupplyZoneTextBox.Text),
                        buildingType = int.Parse(WithoutODPUBuildingTypeTextBox.Text),
                        ztp = int.Parse(WithoutODPUZTPTextBox.Text),
                        StartRow = int.Parse(WithOutODPU_StartRowTextBox.Text)
                    },
                    with_itp = new WithITPConfig
                    {
                        buildingId = int.Parse(WithITPBuildingIdTextBox.Text),
                        city = int.Parse(WithITPCityTextBox.Text),
                        heatSupplyZone = int.Parse(WithITPHeatSupplyZoneTextBox.Text),
                        calcType = int.Parse(WithITPCalcTypeTextBox.Text),
                        buildingType = int.Parse(WithITPBuildingTypeTextBox.Text),
                        ztp = int.Parse(WithITPZTPTextBox.Text),
                        StartRow = int.Parse(WithITP_StartRowTextBox.Text)
                    }
                };

                ConfigModel.SaveConfig(ConfigModel._MP_ConfigPath, mpConfig);
                ConfigModel.SaveConfig(ConfigModel._Naklad_ConfigPath, nakladConfig);
                ConfigModel.SaveConfig(ConfigModel._ODN_ConfigPath, odnConfig);
                ConfigModel.SaveConfig(ConfigModel._PoteryGVS_ConfigPath, poteryGVSConfig);

                MessageBox.Show("Настройки начальных строк сохранены!", "Успех",
                              MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении начальных строк: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}