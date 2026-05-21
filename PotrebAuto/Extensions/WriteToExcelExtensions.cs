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

        private readonly static int templateSecondDatesStartRow_FirstDates = ConfigModel.TemplateSecondDatesStartRow_FirstDates;
        private readonly static int templateSecondDatesStartCol_FirstDates = ConfigModel.TemplateSecondDatesStartCol_FirstDates;

        private readonly static int templateSecondDatesStartRow = ConfigModel.TemplateSecondDatesStartRow;
        private readonly static int templateSecondDatesStartCol = ConfigModel.TemplateSecondDatesStartCol;


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


        ///////////////////////////НАСТРОЙКА СТОЛБЦОВ В ШАБЛОНЕ EXTRA///////////////////////////////////



        public static void InsertDataExtra(this List<ConsumersDataObject> consumers,
                                                    ExcelPackage package, string sheetName)
        {
            var worksheet = package.Workbook.Worksheets[sheetName];

            for (int i = 0; i < consumers.Count; i++)
            {
                var consumer = consumers[i];
                int row = i + templateStartRow;

                worksheet.Cells[row, 1].InsertToCell(consumer.Number);
                worksheet.Cells[row, 2].InsertToCell(consumer.Address);
                worksheet.Cells[row, 3].InsertToCell(consumer.CityGiT);
                worksheet.Cells[row, 4].InsertToCell(consumer.City);
                worksheet.Cells[row, 5].InsertToCell(consumer.TU_AIIS);
                worksheet.Cells[row, 6].InsertToCell(consumer.BuildingId);// лучше ставить из ГиТ
                worksheet.Cells[row, 7].InsertToCell(consumer.BuildingType);
                worksheet.Cells[row, 8].InsertToCell(consumer.ObjectId);

                worksheet.Cells[row, 9].InsertToCell(consumer.PO_AIIS_Total);// из 1 файла ПО
                worksheet.Cells[row, 10].InsertToCell(consumer.ColorDaysCount);// из 1 файла цветные

                worksheet.Cells[row, 11].InsertToCell(consumer.PO_AIIS_Total_2);// из 2 файла ПО
                worksheet.Cells[row, 12].InsertToCell(consumer.ColorDaysCount_2);// из 2 файла цветные

                worksheet.Cells[row, 13].InsertToCell(consumer.Id);

                worksheet.Cells[row, 14].InsertToCell(consumer.PU_GcalTotal); // из 1 файла ПУ
                worksheet.Cells[row, 15].InsertToCell(consumer.PU_WithVNR_Gcal);
                worksheet.Cells[row, 16].InsertToCell(consumer.ZM_GcalTotal); // из 1 файла ЗМ

                worksheet.Cells[row, 17].InsertToCell(consumer.PU_GcalTotal_2);// из 2 файла ПУ
                worksheet.Cells[row, 18].InsertToCell(consumer.ZM_GcalTotal_2);// из 2 файла ЗМ

                worksheet.Cells[row, 19].InsertToCell(consumer.ZM_WithAverage_Gcal);
                worksheet.Cells[row, 20].InsertToCell(consumer.WithBS_Gcal);
                worksheet.Cells[row, 21].InsertToCell(consumer.WithRealLoadBS_Gcal);
                worksheet.Cells[row, 22].InsertToCell(consumer.WithRealLoadTU_Gcal);
                worksheet.Cells[row, 23].InsertToCell(consumer.WithNP_Gcal);
                worksheet.Cells[row, 24].InsertToCell(consumer.WithCab_Gcal);
                worksheet.Cells[row, 25].InsertToCell(consumer.WithKSN_Gcal);
                worksheet.Cells[row, 26].InsertToCell(consumer.WithRealLoad_FN_Gcal);
                worksheet.Cells[row, 27].InsertToCell(consumer.WithDocAndKSN_Gcal);
                worksheet.Cells[row, 28].InsertToCell(consumer.WithDoc_Gcal);
                worksheet.Cells[row, 29].InsertToCell(consumer.CorrectNotesCount);

                worksheet.Cells[row, 30].InsertToCell(consumer.Q_T_M_IsNull);
                worksheet.Cells[row, 31].InsertToCell(consumer.ActsBS);
                worksheet.Cells[row, 32].InsertToCell(consumer.Max_Min_Q_M);
                worksheet.Cells[row, 33].InsertToCell(consumer.IsValid_VNR);
                worksheet.Cells[row, 34].InsertToCell(consumer.IsValid_M1_M2);
                worksheet.Cells[row, 35].InsertToCell(consumer.IsValid_M1_M2_2);
                worksheet.Cells[row, 36].InsertToCell(consumer.Q_eng);
                worksheet.Cells[row, 37].InsertToCell(consumer.IsValid_T);


                if (consumer.DaysValue != null)
                {
                    for (int j = 0; j < consumer.DaysValue.Count; j++) // цветные из 1 файла
                    {
                        if (consumer.DaysValue[j] != null)
                        {
                            worksheet.Cells[row, templateSecondDatesStartCol_FirstDates + j].InsertToCell(consumer.DaysValue[j]);// 30 - начало цветов в шаблоне
                        }

                    }
                }
                

                // поменять на верные для второго файла
                if (consumer.DaysValue_2 != null)
                {
                    for (int j = 0; j < consumer.DaysValue_2.Count; j++) // цветные из 2 файла
                    {
                        if (consumer.DaysValue_2[j] != null)
                        {
                            worksheet.Cells[row, templateSecondDatesStartCol + j].InsertToCell(consumer.DaysValue_2[j]);// 30 - начало цветов в шаблоне
                        }
                    }
                }
                
            }

            int counter = 0;
            foreach (var consumer in consumers) // даты из 1 файла      НЕ РОБИТ
            {
                if (counter <= ConsumersDataObject.DateList_2.Count - 1) // нужно оптимизир
                {
                    worksheet.Cells[templateSecondDatesStartRow_FirstDates, templateSecondDatesStartCol_FirstDates + counter].InsertToCell(ConsumersDataObject.DateList_2[counter]);
                    counter++;
                }
            }
            // поменять на верные для второго файла
            counter = 0;
            foreach (var consumer in consumers) // даты из 2 файла      НЕ РОБИТ
            {
                if (counter <= ConsumersDataObject.DateList.Count - 1)
                {
                    worksheet.Cells[templateSecondDatesStartRow, templateSecondDatesStartCol + counter].InsertToCell(ConsumersDataObject.DateList[counter]);
                    counter++;
                }
            }
        }

    }
}
