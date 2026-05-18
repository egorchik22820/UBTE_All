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
    public class OldFileReader : BaseExcelReaderServise
    {
        public List<List<decimal>> ReadExcelFile(string filePath)
        {
            var result = new List<List<decimal>>();

            using (var package = GetExcelPackage(filePath))
            {
                // Получаем первый лист из книги
                var worksheet = GetWorksheet(package, 1);

                // Определяем количество строк с данными (пропускаем заголовок)
                int rowCount = 94;
                int colCount = 17;

                int startRow = 85;
                int startCol = 16;

                for (int row = startRow; row <= rowCount; row++)
                {
                    try
                    {
                        List<decimal> rowValues = new List<decimal>();
                        for (int col = startCol; col <= colCount; col++)
                        {
                            rowValues.Add(GetDecimalValue(worksheet.Cells[row, col]));
                        }

                        result.Add(rowValues);

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
