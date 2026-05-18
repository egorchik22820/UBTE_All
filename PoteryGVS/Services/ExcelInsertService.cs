using OfficeOpenXml;
using PoteryGVS.Configuration;
using PoteryGVS.Extensions;
using PoteryGVS.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoteryGVS.Services
{
    public static class ExcelInsertService
    {
        public static void ExcelDataInsert(string templatePath, string newFilePath,
                                            List<GVSDataObject> data_With_ODPU,
                                            List<GVSDataObject> data_WithOut_ODPU,
                                            List<GVSDataObject> data_With_ITP)
        {

            CopyTemplate(templatePath, newFilePath);

            using (var package = new ExcelPackage(new FileInfo(newFilePath)))
            {
                data_With_ODPU.InsertData_With_ODPU(package, ConfigModel.MKD_WithODPU_SheetName);
                data_WithOut_ODPU.InsertData_WithOut_ODPU(package, ConfigModel.MKD_WithOutODPU_SheetName);
                data_With_ITP.InsertData_With_ITP(package, ConfigModel.MKD_WithITP_SheetName);

                package.Save();
            }
        }

        private static void CopyTemplate(string templatePath, string newFilePath)
        {
            File.Copy(templatePath, newFilePath, true);
        }
    }
}
