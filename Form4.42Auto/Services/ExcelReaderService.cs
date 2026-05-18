using Form4._42Auto.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Form4._42Auto.Services
{
    public class ExcelReaderService : BaseExcelReaderServise
    {
        public List<ExportDataTable> ReadExcelFile(string filePath)
        {
            var result = new List<ExportDataTable>();

            using (var package = GetExcelPackage(filePath))
            {
                // Получаем первый лист из книги
                var worksheet = GetWorksheet(package, 1);

                // Определяем количество строк с данными (пропускаем заголовок)
                int rowCount = worksheet.Dimension.Rows;


                for (int row = 12; row <= rowCount; row++) // row = 12, чтобы пропустить заголовок
                {
                    try
                    {
                        if (IsEmptyRow(worksheet, row))
                            break;

                        ////////////////////// НАСТРОЙКА ПОРЯДКА СТОБЦОВ ////////////////////////////
                        var data = new ExportDataTable
                        {
                            CounterAgent = worksheet.Cells[row, 4]?.Text?.Trim(),
                            IsVGO = worksheet.Cells[row, 9]?.Text?.Trim(),
                            CounterAgentExpenseType = worksheet.Cells[row, 7]?.Text?.Trim(),
                            Classifier_9_1 = worksheet.Cells[row, 12]?.Text.Trim(),
                            Classifier_9_2 = worksheet.Cells[row, 13]?.Text?.Trim(),
                            Classifier_9_5 = worksheet.Cells[row, 16]?.Text?.Trim(),
                            SaldoDZ = GetDecimalValue(worksheet.Cells[row, 23]),
                            Quantity_GCal = GetDecimalValue(worksheet.Cells[row, 30]),
                            Quantity_m3 = GetDecimalValue(worksheet.Cells[row, 34]),
                            Amount_rub = GetDecimalValue(worksheet.Cells[row, 36]),
                            PaidInPeriod_rub = GetDecimalValue(worksheet.Cells[row, 44]),
                            CorrectionDZ = GetDecimalValue(worksheet.Cells[row, 50]),
                            TransferDZ = GetDecimalValue(worksheet.Cells[row, 51]),
                            OffsKZ = GetDecimalValue(worksheet.Cells[ row, 54]),
                            ValueOfDZ = GetDecimalValue(worksheet.Cells[row, 56]),
                            KZ_InEndPeriod = GetDecimalValue(worksheet.Cells[row, 67]),
                            TotalDZ = GetDecimalValue(worksheet.Cells[row, 60]),
                            CurrentDZ = GetDecimalValue(worksheet.Cells[row, 61])
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
