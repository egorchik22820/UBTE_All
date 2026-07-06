using PotrebAuto.Configuration;
using System.Collections.Generic;

namespace PotrebAuto.Servises
{
    /// <summary>
    /// Псевдонимы заголовков столбцов для каждого типа файла.
    /// Данные загружаются из Configuration/json/ColumnAliases.json.
    /// Чтобы добавить новый вариант названия столбца — отредактируйте
    /// ColumnAliases.json и перезапустите программу (пересборка не нужна).
    /// </summary>
    public static class ColumnAliases
    {
        public static Dictionary<string, string[]> Consumers =>
            ConfigModel.GetAliases("Consumers");

        public static Dictionary<string, string[]> Consumers_2 =>
            ConfigModel.GetAliases("Consumers_2");

        public static Dictionary<string, string[]> GiT =>
            ConfigModel.GetAliases("GiT");

        public static Dictionary<string, string[]> Qlick =>
            ConfigModel.GetAliases("Qlick");

        public static Dictionary<string, string[]> SourcesAndConsumers =>
            ConfigModel.GetAliases("SourcesAndConsumers");
    }
}
