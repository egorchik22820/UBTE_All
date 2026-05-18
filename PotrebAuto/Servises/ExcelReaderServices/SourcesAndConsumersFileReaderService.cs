using PotrebAuto.Extensions;
using PotrebAuto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PotrebAuto.Configuration;

namespace PotrebAuto.Servises.ExcelReaderServices
{
    public class SourcesAndConsumersFileReaderService
    {
        public static List<SourcesAndConsumersObject> ReadExcelFile(string filePath)
        {
            var result = new List<SourcesAndConsumersObject>();

            using (var package = ExcelReaderExtensions.GetExcelPackage(filePath))
            {
                // Получаем первый лист из книги
                var worksheet = package.GetWorksheet(1);

                // Кэшируем конфигурацию перед циклом
                var config = ConfigModel.SACConf;
                var constConfig = ConfigModel.ConstantsConf;
                int startRow = constConfig.SACDataRowStart;
                int tu_IdCol = config.TU_Id;
                int obj_IdCol = config.Obj_Id;

                // Определяем количество строк с данными
                int rowCount = worksheet.Dimension.Rows;

                for (int row = startRow; row <= rowCount; row++)
                {
                    try
                    {
                        if (worksheet.IsEmptyRow(row))/////////
                            break;

                        var data = new SourcesAndConsumersObject
                        {

                            TU_Id = worksheet.SafeGetCellValue(row, tu_IdCol),
                            Obj_Id = worksheet.SafeGetCellValue(row, obj_IdCol)

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
