using OfficeOpenXml;
using OfficeOpenXml.Style;
using PotrebAuto.Configuration;
using PotrebAuto.Models;
using PotrebAuto.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PotrebAuto.Servises
{
    public static class DataServices
    {
        private readonly static string noData = ConfigModel.NoData;
        public static void ApplyRgbColorToCell(ExcelRange cell, string rgbColor)
        {
            if (string.IsNullOrEmpty(rgbColor)) return;

            try
            {
                // Прямое преобразование RGB в Color
                var color = System.Drawing.Color.FromArgb(
                    int.Parse(rgbColor, System.Globalization.NumberStyles.HexNumber));

                cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cell.Style.Fill.BackgroundColor.SetColor(color);
            }
            catch
            {
                // Игнорируем ошибки преобразования цвета
            }
        }

        public static string TryGetIdFromHyperlink(this Uri hyperlink)
        {
            if (hyperlink == null)
                return noData;

            string url = hyperlink.ToString();

            if (string.IsNullOrWhiteSpace(url))
                return noData;

            url = url.TrimEnd('/');

            int lastSlashIndex = url.LastIndexOf('/');

            if (lastSlashIndex >= 0 && lastSlashIndex < url.Length - 1)
                return url.Substring(lastSlashIndex + 1);

            return noData;

        }

        public static void InsertToCell(this ExcelRange cell, CellDTO dto)
        {
            if (dto == null)
            {
                cell.Value = string.Empty;
                return;
            }

            // Сначала устанавливаем гиперссылку (если есть)
            if (dto.Hyperlink != null)
            {
                cell.Hyperlink = dto.Hyperlink;
            }

            // Затем устанавливаем значение
            cell.Value = dto.Value ?? string.Empty;

            // Цвет фона
            if (!string.IsNullOrEmpty(dto.BackGroundColorRgb))
            {
                ApplyRgbColorToCell(cell, dto.BackGroundColorRgb);
            }
        }


        public static decimal? AddDecimals(object value1, object value2)
        {
            var dec1 = ConvertToDecimal(value1);
            var dec2 = ConvertToDecimal(value2);

            if (dec1.HasValue && dec2.HasValue)
                return dec1.Value + dec2.Value;

            return dec1 ?? dec2; // Возвращаем то, что есть
        }

        private static decimal? ConvertToDecimal(object value)
        {
            if (value == null) return null;

            try
            {
                return Convert.ToDecimal(value);
            }
            catch
            {
                return null;
            }
        }

    }
}
