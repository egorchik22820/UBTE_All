using Newtonsoft.Json;
using PotrebAuto.Configuration.jsonModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PotrebAuto.Configuration
{
    public static class ConfigModel
    {

        public readonly static string Name = "";
        public readonly static string Description = "from potreb";

        public readonly static string NoData = "Нет данных";
        public readonly static string TemplateSheetName = "Page 1";
        public readonly static int DaysInMonth_MAX = 35;

        public readonly static string DefoultString = "Файл не выбран";

        // для шаблона
        public readonly static int TemplateDatesStartRow = 2;
        public readonly static int TemplateDatesStartCol = 30;

        public readonly static int TemplateSecondDatesStartRow_FirstDates = 2;
        public readonly static int TemplateSecondDatesStartCol_FirstDates = 38;

        public readonly static int TemplateSecondDatesStartRow = 2;
        public readonly static int TemplateSecondDatesStartCol = 71;

        public readonly static int TemplateDATAStartRow = 5;
        public readonly static int TemplateDATAStartCol = 1;

        // Текущие конфигурации (загружаются из JSON)
        public static ConstantsConfig ConstantsConf { get; set; } = new ConstantsConfig();
        public static ConsumersConfig ConsumersConf { get; set; } = new ConsumersConfig();
        public static Consumers_2Config Consumers_2Conf { get; set; } = new Consumers_2Config();
        public static SACConfig SACConf { get; set; } = new SACConfig();
        public static GiTConfig GiTConf { get; set; } = new GiTConfig();
        public static QlickConfig QlickConf { get; set; } = new QlickConfig();


        // пути к исходным json в проекте
        public readonly static string _Constants_ConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Configuration", "json", "Constants.json");
        public readonly static string _Consumers_ConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Configuration", "json", "Consumers.json");
        public readonly static string _Consumers_2_ConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Configuration", "json", "Consumers_2.json");
        public readonly static string _SAC_ConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Configuration", "json", "SourcesAndConsumers.json");
        public readonly static string _GiT_ConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Configuration", "json", "GiT.json");
        public readonly static string _Qlick_ConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Configuration", "json", "Qlick.json");
        private static readonly string name;


        //Метод для загрузки всех конфигураций при старте приложения
        public static void LoadAllConfigurations()
        {
            try
            {
                ConstantsConf = LoadConfig<ConstantsConfig>(_Constants_ConfigPath);
                ConsumersConf = LoadConfig<ConsumersConfig>(_Consumers_ConfigPath);
                Consumers_2Conf = LoadConfig<Consumers_2Config>(_Consumers_2_ConfigPath);
                SACConf = LoadConfig<SACConfig>(_SAC_ConfigPath);
                GiTConf = LoadConfig<GiTConfig>(_GiT_ConfigPath);
                QlickConf = LoadConfig<QlickConfig>(_Qlick_ConfigPath);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка загрузки конфигураций: {ex.Message}");
            }
        }


        // Обновление статического свойства после сохранения
        private static void UpdateStaticConfig<T>(string filePath, T config) where T : class
        {
            switch (filePath)
            {
                case var path when path == _Constants_ConfigPath:
                    ConstantsConf = config as ConstantsConfig;
                    break;
                case var path when path == _Consumers_ConfigPath:
                    ConsumersConf = config as ConsumersConfig;
                    break;
                case var path when path == _Consumers_2_ConfigPath:
                    Consumers_2Conf = config as Consumers_2Config;
                    break;
                case var path when path == _SAC_ConfigPath:
                    SACConf = config as SACConfig;
                    break;
                case var path when path == _GiT_ConfigPath:
                    GiTConf = config as GiTConfig;
                    break;
                case var path when path == _Qlick_ConfigPath:
                    QlickConf = config as QlickConfig;
                    break;
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

        
    }
}
