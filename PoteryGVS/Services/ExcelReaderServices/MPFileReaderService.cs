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
    public static class MPFileReaderService
    {
        public static List<MPDataObject> ReadExcelFile(string filePath)
        {
            var result = new List<MPDataObject>();

            using (var package = ExcelReaderExtensions.GetExcelPackage(filePath))
            {
                // Получаем первый лист из книги
                var worksheet = package.GetWorksheet(1);

                // Кэшируем конфигурацию перед циклом
                var config = ConfigModel.MP;
                int startRow = config.StartRow;
                int isODPUCol = config.IsODPU;
                int sysPU_IdCol = config.SysPU_Id;
                int addressCol = config.Address;
                int q_CalcCol = config.Q_Calc;
                int dV_CalcCol = config.dV_Calc;
                int vNR_CalcCol = config.VNR_Calc;
                int loadTypeCol = config.LoadType;
                int buildingIdCol = config.BuildingId;
                int tu_AIIS_IdCol = config.TU_AIIS_Id;

                // Определяем количество строк с данными
                int rowCount = worksheet.Dimension.Rows;

                for (int row = startRow; row <= rowCount; row++)
                {
                    try
                    {
                        if (worksheet.IsEmptyRow(row))
                            break;

                        var data = new MPDataObject
                        {
                            IsODPU = worksheet.SafeGetCellValue(row, isODPUCol),
                            SysPU_Id = worksheet.SafeGetCellValue(row, sysPU_IdCol),
                            Address = worksheet.SafeGetCellValue(row, addressCol),

                            Q_Calc = worksheet.Cells[row, q_CalcCol].GetDecimalValue(),
                            dV_Calc = worksheet.Cells[row, dV_CalcCol].GetDecimalValue(),
                            VNR_Calc = worksheet.Cells[row, vNR_CalcCol].GetDecimalValue(),

                            LoadType = worksheet.SafeGetCellValue(row, loadTypeCol),
                            BuildingId = worksheet.SafeGetCellValue(row, buildingIdCol),
                            TU_AIIS_Id = worksheet.SafeGetCellValue(row, tu_AIIS_IdCol)
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
