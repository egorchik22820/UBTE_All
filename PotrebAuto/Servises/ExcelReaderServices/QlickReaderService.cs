using PotrebAuto.Configuration;
using PotrebAuto.Extensions;
using PotrebAuto.Models;
using PotrebAuto.Servises;
using System;
using System.Collections.Generic;

namespace PotrebAuto.Servises.ExcelReaderServices
{
    public class QlickReaderService
    {
        public static List<QlickDataObject> ReadExcelFile(string filePath)
        {
            var result = new List<QlickDataObject>();

            using (var package = ExcelReaderExtensions.GetExcelPackage(filePath))
            {
                var worksheet = package.GetWorksheet(1);
                var config = ConfigModel.QlickConf;
                var constConfig = ConfigModel.ConstantsConf;

                int startRow = constConfig.QlickDataRowStart;
                int headerRowStart = 1;
                int headerRowEnd = startRow - 1;

                var extraWarnings = new System.Collections.Generic.List<string>();
                if (constConfig.UseAutoDetectTableStructure)
                {
                    var (detHeaderEnd, detDataStart) = ColumnResolver.DetectTableStructure(
                        worksheet, ColumnAliases.Qlick, Math.Max(constConfig.MaxExtraHeaderRows + 3, 8));
                    if (detDataStart > 0) { headerRowEnd = detHeaderEnd; startRow = detDataStart; }
                    else extraWarnings.Add("Структура таблицы (начало заголовков и данных)");
                }

                var aliases = config.UseAutoDetect ? ColumnAliases.Qlick : new System.Collections.Generic.Dictionary<string, string[]>();
                var detected = ColumnResolver.ResolveExtending(
                    worksheet, headerRowStart, headerRowEnd, aliases, constConfig.MaxExtraHeaderRows);

                int C(string field, int fallback) =>
                    ColumnResolver.GetColumnOrFallback(detected, field, fallback);

                int buildingGuidCol = C("Guid", config.Guid);
                int buildingIdCol   = C("Id",   config.Id);

                if (config.UseAutoDetect || constConfig.UseAutoDetectTableStructure)
                {
                    var colFields = config.UseAutoDetect ? new (string, string)[] {
                        ("Guid", "Идентификатор АИИС"),
                        ("Id",   "Код строения"),
                    } : new (string, string)[0];
                    ColumnResolver.WarnAutoDetectMissed(filePath, detected, colFields, extraWarnings.ToArray());
                }

                int rowCount = worksheet.Dimension.Rows;

                for (int row = startRow; row <= rowCount; row++)
                {
                    try
                    {
                        if (worksheet.IsEmptyRow(row))
                            break;

                        result.Add(new QlickDataObject
                        {
                            BuildingGUID = worksheet.Cells[row, buildingGuidCol].GetCellDTO().TryGetIdFromIdWithBase(),
                            BuildingId   = worksheet.Cells[row, buildingIdCol].GetCellDTO(),
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
