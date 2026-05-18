using Form46Auto.DTO;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace Form46Auto.Services
{
    // Читает накладную
    public class ExcelReaderService
    {
        public List<ExportDataTable> ReadExcelFile(string filePath)
        {
            var result = new List<ExportDataTable>();

            // Проверка существования файла
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Файл не найден: {filePath}");
            }

            //// Настройка лицензии EPPlus (бесплатная для некоммерческого использования)
            //ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                // Получаем первый лист из книги
                var worksheet = package.Workbook.Worksheets[1];

                // Определяем количество строк с данными (пропускаем заголовок)
                int rowCount = worksheet.Dimension.Rows;

                
                for (int row = 2; row <= rowCount; row++) // row = 2, чтобы пропустить заголовок
                {
                    try
                    {
                        ////////////////////// НАСТРОЙКА ПОРЯДКА СТОБЦОВ ////////////////////////////
                        var data = new ExportDataTable
                        {
                            ContractClassifier = worksheet.Cells[row, 4]?.Text?.Trim(),
                            Nomenclature = worksheet.Cells[row, 11]?.Text?.Trim(),
                            NomenclatureUnit = worksheet.Cells[row,14]?.Text.Trim(),
                            Tariff = worksheet.Cells[row, 12]?.Text?.Trim(),
                            Quantity = GetDecimalValue(worksheet.Cells[row, 15]),
                            QuantityByMeter = GetDecimalValue(worksheet.Cells[row, 16]),
                            Amount = GetDecimalValue(worksheet.Cells[row, 20]),
                            AmountByMeter = GetDecimalValue(worksheet.Cells[row, 21]),
                            AmountWithoutVAT = GetDecimalValue(worksheet.Cells[row, 22]),
                            HeatSource = worksheet.Cells[row, 26]?.Text?.Trim(),
                            LoadType = worksheet.Cells[row, 36]?.Text?.Trim(),
                            Department = worksheet.Cells[row, 38]?.Text?.Trim(),
                            BusinessCalculationMethod = worksheet.Cells[row, 48]?.Text?.Trim(),
                            RecalculationYear = GetDecimalValue(worksheet.Cells[row, 30])
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

        private decimal GetDecimalValue(ExcelRange cell)
        {
            if (cell == null || string.IsNullOrEmpty(cell.Text))
                return 0m;

            if (decimal.TryParse(cell.Text, out decimal result))
                return result;

            return 0m;
        }
    }
}