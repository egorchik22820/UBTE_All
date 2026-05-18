using OfficeOpenXml;
using PoteryGVS.Configuration;
using PoteryGVS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PoteryGVS.Services
{
    public class DataServices
    {
        private static readonly string NoData = ConfigModel.NoData;

        private static readonly string OrenCity = ConfigModel.OrenCity;
        private static readonly string MednoCity = ConfigModel.MednoCity;

        private static string _thisYear = DateTime.Now.Year.ToString();
        private static readonly HashSet<string> _orenCityPatterns = ConfigModel._orenburgCityPatterns;
        private static readonly HashSet<string> _mednogorskCityPatterns = ConfigModel._mednogorskCityPatterns;

        public static string TryGetCityByAddress(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
                return NoData;

            string normalizedAddress = address.ToLowerInvariant();

            foreach (var pattern in _orenCityPatterns)
            {
                if (normalizedAddress.Contains(pattern))
                    return OrenCity;
            }

            foreach (var pattern in _mednogorskCityPatterns)
            {
                if (normalizedAddress.Contains(pattern))
                    return MednoCity;
            }

            return NoData;
        }

        public static string ParseStringYear(string year)
        {
            
            if (!int.TryParse(year, out var res) || string.IsNullOrWhiteSpace(year))
            {
                MessageBox.Show("Значение Года в настройках конфигурации проставленно некорректно.\nУстановленно значение равное текущему году.\nЗакройте и откройте заново окно настроек", "Внимание",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return _thisYear;
            }

            return string.Join("", year.Split()); // Split() разделяет по всем пробельным символам
        }
    }
}
