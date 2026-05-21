using OfficeOpenXml;
using PotrebAuto.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PotrebAuto.Extensions;
using PotrebAuto.Configuration;

namespace PotrebAuto.Servises
{
    public class ExcelInsertService
    {
        private readonly static string templateSheetName = ConfigModel.TemplateSheetName;

        public static void ExcelDataInsert(string templatePath, string newFilePath,
                                            List<ConsumersDataObject> consumers)
        {

            CopyTemplate(templatePath, newFilePath);

            using (var package = new ExcelPackage(new FileInfo(newFilePath)))
            {
                consumers.InsertData(package, templateSheetName);

                package.Save();
            }
        }

        public static void ExcelDataInsertExtra(string templatePath, string newFilePath,
                                            List<ConsumersDataObject> consumers, Dictionary<string, ConsumersDataObject> consumersSecond,
                                            Dictionary<string, QlickDataObject> QlickData, Dictionary<string, SourcesAndConsumersObject> sources)
        {

            CopyTemplate(templatePath, newFilePath);

            using (var package = new ExcelPackage(new FileInfo(newFilePath)))
            {
                consumers.InsertDataExtra(package, templateSheetName);

                package.Save();
            }
        }

        private static void CopyTemplate(string templatePath, string newFilePath)
        {
            File.Copy(templatePath, newFilePath, true);
        }
    }
}
