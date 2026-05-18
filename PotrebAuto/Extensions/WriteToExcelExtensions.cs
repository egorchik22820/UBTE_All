using OfficeOpenXml;
using OfficeOpenXml.Style;
using PotrebAuto.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PotrebAuto.Servises;
using PotrebAuto.Models.DTO;
using PotrebAuto.Configuration;

namespace PotrebAuto.Extensions
{
    public static class WriteToExcelExtensions
    {
        private readonly static int templateStartRow = ConfigModel.TemplateDATAStartRow;

        private readonly static int templateDatesStartRow = ConfigModel.TemplateDatesStartRow;
        private readonly static int templateDatesStartCol = ConfigModel.TemplateDatesStartCol;

        ///////////////////////////НАСТРОЙКА СТОЛБЦОВ В ШАБЛОНЕ///////////////////////////////////


        public static void InsertData(this List<ConsumersDataObject> consumers, ExcelPackage package, string sheetName)
        {
            var worksheet = package.Workbook.Worksheets[sheetName];

            for (int i = 0; i < consumers.Count; i++)
            {
                var consumer = consumers[i];
                int row = i + templateStartRow;

                worksheet.Cells[row, 1].InsertToCell(consumer.Number);
                worksheet.Cells[row, 2].InsertToCell(consumer.Address);
                worksheet.Cells[row, 3].InsertToCell(consumer.TU_AIIS);
                worksheet.Cells[row, 4].InsertToCell(consumer.ObjectId);
                worksheet.Cells[row, 5].InsertToCell(consumer.PO_AIIS_Total);
                worksheet.Cells[row, 6].InsertToCell(consumer.ColorDaysCount);
                worksheet.Cells[row, 7].InsertToCell(consumer.Id);
                worksheet.Cells[row, 8].InsertToCell(consumer.PU_GcalTotal);
                worksheet.Cells[row, 9].InsertToCell(consumer.PU_WithVNR_Gcal);
                worksheet.Cells[row, 10].InsertToCell(consumer.ZM_GcalTotal);
                worksheet.Cells[row, 11].InsertToCell(consumer.ZM_WithAverage_Gcal);
                worksheet.Cells[row, 12].InsertToCell(consumer.WithBS_Gcal);
                worksheet.Cells[row, 13].InsertToCell(consumer.WithRealLoadBS_Gcal);
                worksheet.Cells[row, 14].InsertToCell(consumer.WithRealLoadTU_Gcal);
                worksheet.Cells[row, 15].InsertToCell(consumer.WithNP_Gcal);
                worksheet.Cells[row, 16].InsertToCell(consumer.WithCab_Gcal);
                worksheet.Cells[row, 17].InsertToCell(consumer.WithKSN_Gcal);
                worksheet.Cells[row, 18].InsertToCell(consumer.WithRealLoad_FN_Gcal);
                worksheet.Cells[row, 19].InsertToCell(consumer.WithDocAndKSN_Gcal);
                worksheet.Cells[row, 20].InsertToCell(consumer.WithDoc_Gcal);
                worksheet.Cells[row, 21].InsertToCell(consumer.CorrectNotesCount);
                worksheet.Cells[row, 22].InsertToCell(consumer.Q_T_M_IsNull);
                worksheet.Cells[row, 23].InsertToCell(consumer.ActsBS);
                worksheet.Cells[row, 24].InsertToCell(consumer.Max_Min_Q_M);
                worksheet.Cells[row, 25].InsertToCell(consumer.IsValid_VNR);
                worksheet.Cells[row, 26].InsertToCell(consumer.IsValid_M1_M2);
                worksheet.Cells[row, 27].InsertToCell(consumer.IsValid_M1_M2_2);
                worksheet.Cells[row, 28].InsertToCell(consumer.Q_eng);
                worksheet.Cells[row, 29].InsertToCell(consumer.IsValid_T);

                for (int j = 0; j < consumer.DaysValue.Count; j++)
                {
                    if (consumer.DaysValue[j] != null)
                    {
                        worksheet.Cells[row, templateDatesStartCol + j].InsertToCell(consumer.DaysValue[j]);// 30 - начало цветов в шаблоне
                    }

                }
            }

            int counter = 0;
            foreach (var consumer in consumers)
            {
                if (counter <= ConsumersDataObject.DateList.Count - 1)
                {
                    worksheet.Cells[templateDatesStartRow, templateDatesStartCol + counter].InsertToCell(ConsumersDataObject.DateList[counter]);
                    counter++;
                }
            }
        }

        


    }
}
