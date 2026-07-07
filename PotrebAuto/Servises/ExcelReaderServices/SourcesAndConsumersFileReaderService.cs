using PotrebAuto.Configuration;
using PotrebAuto.Extensions;
using PotrebAuto.Models;
using PotrebAuto.Servises;
using System;
using System.Collections.Generic;

namespace PotrebAuto.Servises.ExcelReaderServices
{
    public class SourcesAndConsumersFileReaderService
    {
        public static List<SourcesAndConsumersObject> ReadExcelFile(string filePath)
        {
            var result = new List<SourcesAndConsumersObject>();

            using (var package = ExcelReaderExtensions.GetExcelPackage(filePath))
            {
                var worksheet = package.GetWorksheet(1);
                var config = ConfigModel.SACConf;
                var constConfig = ConfigModel.ConstantsConf;

                int startRow = constConfig.SACDataRowStart;
                int headerRowStart = constConfig.SACTableRowStart > 0
                    ? constConfig.SACTableRowStart : 1;
                int headerRowEnd = startRow - 1;

                var extraWarnings = new System.Collections.Generic.List<string>();
                if (constConfig.UseAutoDetectTableStructure)
                {
                    var (detHeaderEnd, detDataStart) = ColumnResolver.DetectTableStructure(
                        worksheet, ColumnAliases.SourcesAndConsumers, Math.Max(constConfig.MaxExtraHeaderRows + 3, 8));
                    if (detDataStart > 0)
                    {
                        headerRowStart = 1;
                        headerRowEnd = detHeaderEnd;
                        startRow = detDataStart;
                    }
                    else
                        extraWarnings.Add("Структура таблицы (начало заголовков и данных)");
                }

                var aliases = config.UseAutoDetect ? ColumnAliases.SourcesAndConsumers : new System.Collections.Generic.Dictionary<string, string[]>();
                var detected = ColumnResolver.ResolveExtending(
                    worksheet, headerRowStart, headerRowEnd, aliases, constConfig.MaxExtraHeaderRows);

                int C(string field, int fallback) =>
                    ColumnResolver.GetColumnOrFallback(detected, field, fallback);

                int tu_IdCol  = C("TU_Id",  config.TU_Id);
                int obj_IdCol = C("Obj_Id", config.Obj_Id);

                if (config.UseAutoDetect || constConfig.UseAutoDetectTableStructure)
                {
                    var colFields = config.UseAutoDetect ? new (string, string)[] {
                        ("TU_Id",  "Идентификатор точки учёта"),
                        ("Obj_Id", "Идентификатор объекта"),
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

                        result.Add(new SourcesAndConsumersObject
                        {
                            TU_Id  = worksheet.SafeGetCellValue(row, tu_IdCol),
                            Obj_Id = worksheet.SafeGetCellValue(row, obj_IdCol),
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
