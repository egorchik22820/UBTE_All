using OfficeOpenXml;
using PoteryGVS.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoteryGVS.Configuration;

namespace PoteryGVS.Extensions
{
    public static class ExcelReaderExtensions
    {



        // для OldFileGVS
        public static List<GVSDataObject> SaveDataFromExcelSheet_With_ODPU(this ExcelWorksheet worksheet)
        {
            List<GVSDataObject> result = new List<GVSDataObject>();

            // Кэшируем конфигурацию перед циклом
            var config = ConfigModel.PoteryGVS.with_odpu;
            int startRow = config.StartRow;
            int buildingIdCol = config.buildingId;
            int cityCol = config.city;
            int heatSupplyZoneCol = config.heatSupplyZone;
            int ztpCol = config.ztp;

            // Определяем количество строк с данными
            int rowCount = worksheet.Dimension.Rows;

            for (int row = startRow; row <= rowCount; row++)
            {
                try
                {
                    if (worksheet.IsEmptyRow(row))
                        break;


                    var data = new GVSDataObject
                    {
                        BuildingId = worksheet.SafeGetCellValue(row, buildingIdCol),
                        City = worksheet.SafeGetCellValue(row, cityCol),
                        HeatSupplyZone = worksheet.SafeGetCellValue(row, heatSupplyZoneCol),
                        ZTP = ztpCol != 0 ? worksheet.SafeGetCellValue(row, ztpCol) : null
                    };

                    result.Add(data);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка чтения строки {row}: {ex.Message}");
                }
            }

            return result;
        }

        public static List<GVSDataObject> SaveDataFromExcelSheet_WithOut_ODPU(this ExcelWorksheet worksheet)
        {
            List<GVSDataObject> result = new List<GVSDataObject>();

            // Кэшируем конфигурацию перед циклом
            var config = ConfigModel.PoteryGVS.without_odpu;
            int startRow = config.StartRow;
            int buildingIdCol = config.buildingId;
            int cityCol = config.city;
            int heatSupplyZoneCol = config.heatSupplyZone;
            int buildingTypeCol = config.buildingType;
            int ztpCol = config.ztp;

            // Определяем количество строк с данными
            int rowCount = worksheet.Dimension.Rows;

            for (int row = startRow; row <= rowCount; row++)
            {
                try
                {
                    if (worksheet.IsEmptyRow(row))
                        break;

                    var data = new GVSDataObject
                    {
                        BuildingId = worksheet.SafeGetCellValue(row, buildingIdCol),
                        City = worksheet.SafeGetCellValue(row, cityCol),
                        HeatSupplyZone = worksheet.SafeGetCellValue(row, heatSupplyZoneCol),
                        BuildingType = worksheet.SafeGetCellValue(row, buildingTypeCol),
                        ZTP = worksheet.SafeGetCellValue(row, ztpCol)
                    };

                    result.Add(data);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка чтения строки {row}: {ex.Message}");
                }
            }

            return result;
        }

        public static List<GVSDataObject> SaveDataFromExcelSheet_With_ITP(this ExcelWorksheet worksheet)
        {
            List<GVSDataObject> result = new List<GVSDataObject>();

            // Кэшируем конфигурацию перед циклом
            var config = ConfigModel.PoteryGVS.with_itp;
            int startRow = config.StartRow;
            int buildingIdCol = config.buildingId;
            int cityCol = config.city;
            int heatSupplyZoneCol = config.heatSupplyZone;
            int calcTypeCol = config.calcType;
            int buildingTypeCol = config.buildingType;
            int ztpCol = config.ztp;

            // Определяем количество строк с данными
            int rowCount = worksheet.Dimension.Rows;

            for (int row = startRow; row <= rowCount; row++)
            {
                try
                {
                    if (worksheet.IsEmptyRow(row))
                        break;

                    var data = new GVSDataObject
                    {
                        BuildingId = worksheet.SafeGetCellValue(row, buildingIdCol),
                        City = worksheet.SafeGetCellValue(row, cityCol),
                        HeatSupplyZone = worksheet.SafeGetCellValue(row, heatSupplyZoneCol),
                        CalcType = worksheet.SafeGetCellValue(row, calcTypeCol),
                        BuildingType = worksheet.SafeGetCellValue(row, buildingTypeCol),
                        ZTP = worksheet.SafeGetCellValue(row, ztpCol)
                    };

                    result.Add(data);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка чтения строки {row}: {ex.Message}");
                }
            }

            return result;
        }


        //////////////
        public static ExcelPackage GetExcelPackage(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Файл не найден: {filePath}");
            }

            return new ExcelPackage(new FileInfo(filePath));
        }

        public static decimal GetDecimalValue(this ExcelRange cell)
        {
            if (cell == null || string.IsNullOrEmpty(cell.Text))
                return 0m;

            // Обработка чисел с пробелами (например: "1 687 351.86659")
            string cleanedText = cell.Text.Replace(" ", "").Replace(",", ".");

            if (decimal.TryParse(cell.Text, out decimal result))
                return result;

            return 0m;
        }

        public static decimal GetDecimaPositivelValue(this ExcelRange cell)
        {
            if (cell == null || string.IsNullOrEmpty(cell.Text))
                return 0m;

            // Обработка чисел с пробелами (например: "1 687 351.86659")
            string cleanedText = cell.Text.Replace(" ", "").Replace(",", ".");


            if (decimal.TryParse(cell.Text, out decimal result) && result > 0)
                return result;

            return 0m;
        }

        public static string GetStringYear(this ExcelRange cell)
        {
            if (cell == null || string.IsNullOrEmpty(cell.Text))
                return string.Empty;

            return string.Join("", cell.Text.Split()); // Split() разделяет по всем пробельным символам
        }

        public static bool IsEmptyRow(this ExcelWorksheet worksheet, int row)
        {
            if (worksheet.Dimension == null) return true;

            for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
            {
                if (!string.IsNullOrEmpty(worksheet.Cells[row, col]?.Text?.Trim()))
                    return false;
            }
            return true;
        }

        public static string SafeGetCellValue(this ExcelWorksheet worksheet, int row, int col)
        {
            try
            {
                return worksheet.Cells[row, col]?.Text?.Trim() ?? string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        public static ExcelWorksheet GetWorksheet(this ExcelPackage package, int worksheetIndex = 1)
        {
            if (package.Workbook.Worksheets.Count < worksheetIndex || package.Workbook.Worksheets.Count <= 0)
            {
                throw new Exception($"Лист с индексом {worksheetIndex} не найден. Всего листов: {package.Workbook.Worksheets.Count}");
            }

            List<int> indexes = new List<int>(package.Workbook.Worksheets.Count);

            foreach (var sheet in package.Workbook.Worksheets)
            {
                indexes.Add(sheet.Index);
            }

            var worksheet = package.Workbook.Worksheets[indexes[worksheetIndex - 1]];

            if (worksheet.Dimension == null)
            {
                throw new Exception("Лист не содержит данных");
            }

            return worksheet;
        }

    }
}