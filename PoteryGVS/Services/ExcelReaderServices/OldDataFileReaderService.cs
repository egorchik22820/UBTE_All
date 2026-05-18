using PoteryGVS.Extensions;
using PoteryGVS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoteryGVS.Services.ExcelReaderServices
{
    public static class OldDataFileReaderService
    {

        public static List<GVSDataObject> ReadExcelSheet_With_ODPU(string filePath)
        {
            var result = new List<GVSDataObject>();

            using (var package = ExcelReaderExtensions.GetExcelPackage(filePath))
            {
                // Получаем лист из книги
                return package.GetWorksheet(1)
                              .SaveDataFromExcelSheet_With_ODPU();

            }   

        }

        public static List<GVSDataObject> ReadExcelSheet_WithOut_ODPU(string filePath)
        {
            var result = new List<GVSDataObject>();

            using (var package = ExcelReaderExtensions.GetExcelPackage(filePath))
            {
                // Получаем лист из книги
                return package.GetWorksheet(2)
                              .SaveDataFromExcelSheet_WithOut_ODPU();

            }

        }

        public static List<GVSDataObject> ReadExcelSheet_With_ITP(string filePath)
        {
            var result = new List<GVSDataObject>();

            using (var package = ExcelReaderExtensions.GetExcelPackage(filePath))
            {
                // Получаем лист из книги
                return package.GetWorksheet(3)
                              .SaveDataFromExcelSheet_With_ITP();

            }

        }
    }
}
