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

                var aliases = config.UseAutoDetect ? ColumnAliases.SourcesAndConsumers : new System.Collections.Generic.Dictionary<string, string[]>();
                var detected = ColumnResolver.ResolveExtending(
                    worksheet, headerRowStart, headerRowEnd, aliases);

                int C(string field, int fallback) =>
                    ColumnResolver.GetColumnOrFallback(detected, field, fallback);

                int tu_IdCol  = C("TU_Id",  config.TU_Id);
                int obj_IdCol = C("Obj_Id", config.Obj_Id);

                if (config.UseAutoDetect)
                {
                    ColumnResolver.WarnAutoDetectMissed(filePath, detected, new (string, string)[] {
                        ("TU_Id",  "Идентификатор точки учёта"),
                        ("Obj_Id", "Идентификатор объекта"),
                    });
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
