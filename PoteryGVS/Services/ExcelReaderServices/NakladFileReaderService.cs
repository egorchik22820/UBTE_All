using PoteryGVS.Configuration;
using PoteryGVS.Extensions;
using PoteryGVS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoteryGVS.Services
{
    public static class NakladFileReaderService
    {
        public static List<NakladDataObject> ReadExcelFile(string filePath)
        {
            var result = new List<NakladDataObject>();

            using (var package = ExcelReaderExtensions.GetExcelPackage(filePath))
            {
                // Получаем первый лист из книги
                var worksheet = package.GetWorksheet(1);

                // Кэшируем конфигурацию перед циклом
                var config = ConfigModel.Naklad;
                int startRow = config.StartRow;
                int docTypeCol = config.DocType;
                int nomenclatureCol = config.Nomenclature;
                int tariffCol = config.Tariff;
                int calcTypeCol = config.CalcType;
                int nomenclatureUnitCol = config.NomenclatureUnit;
                int quantityTotalCol = config.QuantityTotal;
                int heatSourseCol = config.HeatSourse;
                int recalcYearCol = config.RecalcYear;
                int loadTypeCol = config.LoadType;
                int departmentCol = config.Department;
                int buildingIdCol = config.BuildingId;
                int buildingTypeCol = config.BuildingType;
                int spaceTypeCol = config.SpaceType;
                int addressTUCol = config.AddressTU;
                int buildingAddressCol = config.BuildingAddress;

                // Определяем количество строк с данными
                int rowCount = worksheet.Dimension.Rows;

                for (int row = startRow; row <= rowCount; row++)
                {
                    try
                    {
                        if (worksheet.IsEmptyRow(row))
                            break;

                        var data = new NakladDataObject
                        {
                            DocType = worksheet.SafeGetCellValue(row, docTypeCol),
                            Nomenclature = worksheet.SafeGetCellValue(row, nomenclatureCol),
                            Tariff = worksheet.SafeGetCellValue(row, tariffCol),
                            CalcType = worksheet.SafeGetCellValue(row, calcTypeCol),
                            NomenclatureUnit = worksheet.SafeGetCellValue(row, nomenclatureUnitCol),

                            QuantityTotal = worksheet.Cells[row, quantityTotalCol].GetDecimalValue(),

                            HeatSourse = worksheet.SafeGetCellValue(row, heatSourseCol),

                            RecalcYear = worksheet.Cells[row, recalcYearCol].GetStringYear(),

                            LoadType = worksheet.SafeGetCellValue(row, loadTypeCol),
                            Department = worksheet.SafeGetCellValue(row, departmentCol),
                            BuildingId = worksheet.SafeGetCellValue(row, buildingIdCol),
                            BuildingType = worksheet.SafeGetCellValue(row, buildingTypeCol),
                            SpaceType = worksheet.SafeGetCellValue(row, spaceTypeCol),
                            AddressTU = worksheet.SafeGetCellValue(row, addressTUCol),
                            BuildingAddress = worksheet.SafeGetCellValue(row, buildingAddressCol)
                        };

                        result.Add(data);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка чтения строки {row}: {ex.Message}");
                    }
                }
            }

            return result;
        }
    }
}
