using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using PoteryGVS.Configuration.jsonModels;

namespace PoteryGVS.Configuration
{
    public static class ConfigModel
    {
        // значения для фильтрации MP
        public static readonly string mpIsOdpu = "Да";
        public static readonly string mpLoadTypeGvs = "Горячее водоснабжение";
        public static readonly string mpLoadTypeTeplo = "Теплоэнергия";

        // значения для фильтрации Naklad
        public static readonly string NakladDepartmentOren = "Оренбургский филиал ПАО \"Т Плюс\" г. Оренбург";
        public static readonly string NakladDepartmentMedno = "Оренбургский филиал ПАО \"Т Плюс\" г. Медногорск";
        public static readonly string NakladHeatSourse = "КТЭЦ";
        public static readonly string NakladLoadTypeHeatWater = "Подогрев ХВ";
        public static readonly string NakladLoadTypeGvs = "Горячее водоснабжение";
        public static readonly string NakladBuildingType = "Нежилое";
        public static readonly string NakladNomenclatureUnit_Gcal = "Гкал";
        public static readonly string NakladNomenclatureUnit_m3 = "м3";

        // значения для GVSDataUnion
        public static readonly string NoData = "Нет данных";
        public static readonly string ByIndications = "По показаниям";
        public static readonly string Normative = "Норматив";

        // значения для DataServices
        public static readonly string OrenCity = "Оренбург";
        public static readonly string MednoCity = "Медногорск";


        // Названия листов Excel
        public static string MKD_WithODPU_SheetName { get; private set; } = "МКД с ОДПУ";
        public static string MKD_WithOutODPU_SheetName { get; private set; } = "МКД без ОДПУ";
        public static string MKD_WithITP_SheetName { get; private set; } = "МКД с ИТП";

        // Города
        public static readonly HashSet<string> _orenburgCityPatterns = new HashSet<string>
        {
            "оренбург г", "г оренбург", "г. оренбург", "оренбург г.",
            "город оренбург", "оренбург город", "гор оренбург",
            "гор. оренбург", "оренбург гор", "оренбург гор."
        };

        public static readonly HashSet<string> _mednogorskCityPatterns = new HashSet<string>
        {
            "медногорск г", "г медногорск", "г. медногорск", "медногорск г.",
            "город медногорск", "медногорск город", "гор медногорск",
            "гор. медногорск", "медногорск гор", "медногорск гор."
        };

        // Текущие конфигурации (загружаются из JSON)
        public static MPConfig MP { get; set; } = new MPConfig();
        public static NakladConfig Naklad { get; set; } = new NakladConfig();
        public static ODNConfig ODN { get; set; } = new ODNConfig();
        public static PoteryGVSConfig PoteryGVS { get; set; } = new PoteryGVSConfig();

        // пути к исходным json в проекте
        public readonly static string _MP_ConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Configuration", "json", "MP.json");
        public readonly static string _Naklad_ConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Configuration", "json", "Naklad.json");
        public readonly static string _ODN_ConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Configuration", "json", "ODN.json");
        public readonly static string _PoteryGVS_ConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Configuration", "json", "PoteryGVS.json");

        //Метод для загрузки всех конфигураций при старте приложения
        public static void LoadAllConfigurations()
        {
            try
            {
                MP = LoadConfig<MPConfig>(_MP_ConfigPath);
                Naklad = LoadConfig<NakladConfig>(_Naklad_ConfigPath);
                ODN = LoadConfig<ODNConfig>(_ODN_ConfigPath);
                PoteryGVS = LoadConfig<PoteryGVSConfig>(_PoteryGVS_ConfigPath);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка загрузки конфигураций: {ex.Message}");
            }
        }

        //Метод для сохранения конфигурации в JSON
        public static void SaveConfig<T>(string filePath, T config) where T : class
        {
            try
            {
                string json = JsonConvert.SerializeObject(config, Formatting.Indented);

                // Сохраняем в bin (для работы приложения)
                SaveToBin(filePath, json);

                // Сохраняем в исходники проекта
                SaveToSource(filePath, json);

                // Обновляем статическое свойство после сохранения
                UpdateStaticConfig(filePath, config);

                System.Diagnostics.Debug.WriteLine($"Сохранено в bin и исходники: {filePath}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка сохранения конфигурации {filePath}: {ex.Message}");
            }
        }

        private static void SaveToBin(string filePath, string json)
        {
            try
            {
                // Создаем директорию если она не существует
                string directory = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка сохранения в bin: {ex.Message}");
            }
        }

        private static void SaveToSource(string filePath, string json)
        {
            try
            {
                // Получаем путь к исходникам
                string sourceFilePath = GetSourcePath(filePath);

                // Создаем директорию если её нет
                string directory = Path.GetDirectoryName(sourceFilePath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                File.WriteAllText(sourceFilePath, json);
                System.Diagnostics.Debug.WriteLine($"Сохранено в исходники: {sourceFilePath}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка сохранения в исходники: {ex.Message}");
            }
        }

        private static string GetSourcePath(string binFilePath)
        {
            // Получаем путь к папке проекта
            string projectRoot = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;

            // Получаем относительный путь от bin к файлу
            string binPath = AppDomain.CurrentDomain.BaseDirectory;
            string relativePath = binFilePath.Replace(binPath, "");

            // Комбинируем путь проекта с относительным путем
            return Path.Combine(projectRoot, relativePath);
        }


        // Метод для загрузки конфигурации из JSON
        private static T LoadConfig<T>(string filePath) where T : class, new()
        {
            try
            {
                if (File.Exists(filePath))
                {
                    string json = File.ReadAllText(filePath);
                    var result = JsonConvert.DeserializeObject<T>(json);
                    if (result != null)
                    {
                        System.Diagnostics.Debug.WriteLine($"Успешно загружен из bin: {filePath}");
                        return result;
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Файл не найден в bin: {filePath}");
                    // Пробуем загрузить из исходников если в bin нет
                    return LoadFromSource<T>(filePath);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка загрузки из bin {filePath}: {ex.Message}");
            }

            System.Diagnostics.Debug.WriteLine($"Возвращаем новый объект для {filePath}");
            return new T();
        }

        private static T LoadFromSource<T>(string binFilePath) where T : class, new()
        {
            try
            {
                string sourceFilePath = GetSourcePath(binFilePath);
                if (File.Exists(sourceFilePath))
                {
                    string json = File.ReadAllText(sourceFilePath);
                    var result = JsonConvert.DeserializeObject<T>(json);
                    if (result != null)
                    {
                        System.Diagnostics.Debug.WriteLine($"Успешно загружен из исходников: {sourceFilePath}");
                        // Копируем из исходников в bin
                        string directory = Path.GetDirectoryName(binFilePath);
                        if (!Directory.Exists(directory))
                        {
                            Directory.CreateDirectory(directory);
                        }
                        File.Copy(sourceFilePath, binFilePath, true);
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка загрузки из исходников: {ex.Message}");
            }
            return new T();
        }

        // Обновление статического свойства после сохранения
        private static void UpdateStaticConfig<T>(string filePath, T config) where T : class
        {
            switch (filePath)
            {
                case var path when path == _MP_ConfigPath:
                    MP = config as MPConfig;
                    break;
                case var path when path == _Naklad_ConfigPath:
                    Naklad = config as NakladConfig;
                    break;
                case var path when path == _ODN_ConfigPath:
                    ODN = config as ODNConfig;
                    break;
                case var path when path == _PoteryGVS_ConfigPath:
                    PoteryGVS = config as PoteryGVSConfig;
                    break;
            }
        }
    }
}