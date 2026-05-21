using PotrebAuto.Configuration;
using PotrebAuto.Extensions;
using PotrebAuto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PotrebAuto.Servises;

namespace PotrebAuto.Servises.ExcelReaderServices
{
    public class QlickReaderService
    {
        public static List<QlickDataObject> ReadExcelFile(string filePath)
        {
            var result = new List<QlickDataObject>();

            using (var package = ExcelReaderExtensions.GetExcelPackage(filePath))
            {
                // Получаем первый лист из книги
                var worksheet = package.GetWorksheet(1);

                // Кэшируем конфигурацию перед циклом   // поменять
                var config = ConfigModel.QlickConf;
                var constConfig = ConfigModel.ConstantsConf;
                int startRow = constConfig.QlickDataRowStart;
                int buildingId = config.Id;
                int buildingGuid = config.Guid;

                // Определяем количество строк с данными
                int rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    try
                    {
                        if (worksheet.IsEmptyRow(row))/////////
                            break;

                        var data = new QlickDataObject
                        {

                            BuildingGUID = worksheet.Cells[row, buildingGuid].GetCellDTO().TryGetIdFromIdWithBase(),
                            BuildingId = worksheet.Cells[row, buildingId].GetCellDTO()

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
