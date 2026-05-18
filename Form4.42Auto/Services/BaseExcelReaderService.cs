using OfficeOpenXml;
using System;
using System.IO;
using System.Collections.Generic;

namespace Form4._42Auto.Services
{
    public abstract class BaseExcelReaderServise
    {
        protected ExcelPackage GetExcelPackage(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Файл не найден: {filePath}");
            }

            return new ExcelPackage(new FileInfo(filePath));
        }

        protected decimal GetDecimalValue(ExcelRange cell)
        {
            if (cell == null || string.IsNullOrEmpty(cell.Text))
                return 0m;

            // Обработка чисел с пробелами (например: "1 687 351.86659")
            string cleanedText = cell.Text.Replace(" ", "").Replace(",", ".");

            if (decimal.TryParse(cell.Text, out decimal result))
                return result;

            return 0m;
        }

        protected bool IsEmptyRow(ExcelWorksheet worksheet, int row)
        {
            if (worksheet.Dimension == null) return true;

            for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
            {
                if (!string.IsNullOrEmpty(worksheet.Cells[row, col]?.Text?.Trim()))
                    return false;
            }
            return true;
        }

        protected string SafeGetCellValue(ExcelWorksheet worksheet, int row, int col)
        {
            try
            {
                return worksheet.Cells[row, col]?.Text?.Trim() ?? string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        protected ExcelWorksheet GetWorksheet(ExcelPackage package, int worksheetIndex = 1)
        {
            if (package.Workbook.Worksheets.Count < worksheetIndex)
            {
                throw new Exception($"Лист с индексом {worksheetIndex} не найден. Всего листов: {package.Workbook.Worksheets.Count}");
            }

            var worksheet = package.Workbook.Worksheets[worksheetIndex];

            if (worksheet.Dimension == null)
            {
                throw new Exception("Лист не содержит данных");
            }

            return worksheet;
        }
    }
}