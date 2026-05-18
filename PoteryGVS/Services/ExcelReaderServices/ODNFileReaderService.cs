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
    public static class ODNFileReaderService
    {
        public static List<ODNDataObject> ReadExcelFile(string filePath)
        {
            var result = new List<ODNDataObject>();

            using (var package = ExcelReaderExtensions.GetExcelPackage(filePath))
            {
                // Получаем первый лист из книги
                var worksheet = package.GetWorksheet(1);

                // Кэшируем конфигурацию перед циклом
                var config = ConfigModel.ODN;
                int startRow = config.StartRow;
                int buildingIdCol = config.BuildingId;
                int negativeODN_GcalCol = config.NegativeODN_Gcal;
                int negativeODN_m3Col = config.NegativeODN_m3;

                // Определяем количество строк с данными
                int rowCount = worksheet.Dimension.Rows;

                for (int row = startRow; row <= rowCount; row++)
                {
                    try
                    {
                        if (worksheet.IsEmptyRow(row))
                            break;

                        var data = new ODNDataObject
                        {
                            BuildingId = worksheet.SafeGetCellValue(row, buildingIdCol),
                            NegativeODN_Gcal = worksheet.Cells[row, negativeODN_GcalCol].GetDecimalValue(),
                            NegativeODN_m3 = worksheet.Cells[row, negativeODN_m3Col].GetDecimalValue()
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
