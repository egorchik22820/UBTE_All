using PotrebAuto.Configuration;
using PotrebAuto.Extensions;
using PotrebAuto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PotrebAuto.Servises.ExcelReaderServices
{
    public class GiTFileReaderService
    {
        public static List<GiTDataObject> ReadExcelFile(string filePath)
        {
            var result = new List<GiTDataObject>();

            using (var package = ExcelReaderExtensions.GetExcelPackage(filePath))
            {
                // Получаем первый лист из книги
                var worksheet = package.GetWorksheet(1);

                // Кэшируем конфигурацию перед циклом
                var config = ConfigModel.GiTConf;
                var constConfig = ConfigModel.ConstantsConf;
                int startRow = constConfig.GiTDataRowStart;
                int buildingId = config.BuildingId;
                int buildingType = config.BuildingType;
                int city = config.City;

                // Определяем количество строк с данными
                int rowCount = worksheet.Dimension.Rows;

                for (int row = startRow; row <= rowCount; row++)
                {
                    try
                    {
                        if (worksheet.IsEmptyRow(row))/////////
                            break;

                        var data = new GiTDataObject
                        {

                            BuildingId = worksheet.Cells[row, buildingId].GetCellDTO(),
                            BuildingType = worksheet.Cells[row, buildingType].GetCellDTO(),
                            City = worksheet.Cells[row, city].GetCellDTO()

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
