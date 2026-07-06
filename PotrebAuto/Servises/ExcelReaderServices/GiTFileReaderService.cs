using PotrebAuto.Configuration;
using PotrebAuto.Extensions;
using PotrebAuto.Models;
using PotrebAuto.Servises;
using System;
using System.Collections.Generic;

namespace PotrebAuto.Servises.ExcelReaderServices
{
    public class GiTFileReaderService
    {
        public static List<GiTDataObject> ReadExcelFile(string filePath)
        {
            var result = new List<GiTDataObject>();

            using (var package = ExcelReaderExtensions.GetExcelPackage(filePath))
            {
                var worksheet = package.GetWorksheet(1);
                var config = ConfigModel.GiTConf;
                var constConfig = ConfigModel.ConstantsConf;

                int startRow = constConfig.GiTDataRowStart;

                // GiTTableRowStart = 0 означает отсутствие явного заголовка в настройках,
                // но авто-определение всё равно пробуем с row 1 как fallback.
                int headerRowStart = constConfig.GiTTableRowStart;
                int headerRowEnd = startRow - 1;

                int buildingIdCol   = config.BuildingId;
                int buildingTypeCol = config.BuildingType;
                int cityCol         = config.City;

                if (config.UseAutoDetect)
                {
                    int effHeaderStart = headerRowStart >= 1 ? headerRowStart : 1;
                    int effHeaderEnd   = Math.Max(effHeaderStart, headerRowEnd);

                    var detected = ColumnResolver.ResolveExtending(
                        worksheet, effHeaderStart, effHeaderEnd, ColumnAliases.GiT, constConfig.MaxExtraHeaderRows);

                    int C(string field, int fallback) =>
                        ColumnResolver.GetColumnOrFallback(detected, field, fallback);

                    buildingIdCol   = C("BuildingId",   config.BuildingId);
                    buildingTypeCol = C("BuildingType", config.BuildingType);
                    cityCol         = C("City",         config.City);

                    ColumnResolver.WarnAutoDetectMissed(filePath, detected, new (string, string)[] {
                        ("BuildingId",   "Код строения"),
                        ("BuildingType", "Тип здания"),
                        ("City",         "Город"),
                    });
                }

                int rowCount = worksheet.Dimension.Rows;

                for (int row = startRow; row <= rowCount; row++)
                {
                    try
                    {
                        if (worksheet.IsEmptyRow(row))
                            break;

                        result.Add(new GiTDataObject
                        {
                            BuildingId   = worksheet.Cells[row, buildingIdCol].GetCellDTO(),
                            BuildingType = worksheet.Cells[row, buildingTypeCol].GetCellDTO(),
                            City         = worksheet.Cells[row, cityCol].GetCellDTO(),
                        });
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
