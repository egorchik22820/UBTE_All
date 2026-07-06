using PotrebAuto.Configuration;
using PotrebAuto.Extensions;
using PotrebAuto.Models;
using PotrebAuto.Servises;
using System;
using System.Collections.Generic;

namespace PotrebAuto.Servises.ExcelReaderServices
{
    public class ConsumersFileReaderService
    {
        public static List<ConsumersDataObject> ReadExcelFile(string filePath)
        {
            var result = new List<ConsumersDataObject>();

            using (var package = ExcelReaderExtensions.GetExcelPackage(filePath))
            {
                var worksheet = package.GetWorksheet(1);
                var config = ConfigModel.ConsumersConf;
                var constConfig = ConfigModel.ConstantsConf;

                int headerRowStart = constConfig.ConsumersTableRowStart > 0
                    ? constConfig.ConsumersTableRowStart : 1;
                int startRow = constConfig.ConsumersDataRowStart;
                int headerRowEnd = startRow - 1;

                // Автоопределение столбцов по заголовкам (или пустые псевдонимы если отключено)
                var aliases = config.UseAutoDetect ? ColumnAliases.Consumers : new System.Collections.Generic.Dictionary<string, string[]>();
                var detected = ColumnResolver.ResolveExtending(
                    worksheet, headerRowStart, headerRowEnd, aliases);

                // Для каждого поля: берём auto-detected или значение из конфига
                int C(string field, int fallback) =>
                    ColumnResolver.GetColumnOrFallback(detected, field, fallback);

                int numberCol               = C("Number",              config.Number);
                int addressCol              = C("Address",             config.Address);
                int idCol                   = C("Id",                  config.Id);
                int pu_GcalTotalCol         = C("PU_GcalTotal",        config.PU_GcalTotal);
                int pu_WithVNR_GcalCol      = C("PU_WithVNR_Gcal",     config.PU_WithVNR_Gcal);
                int zm_GcalTotalCol         = C("ZM_GcalTotal",        config.ZM_GcalTotal);
                int zm_WithAverage_GcalCol  = C("ZM_WithAverage_Gcal", config.ZM_WithAverage_Gcal);
                int withBS_GcalCol          = C("WithBS_Gcal",         config.WithBS_Gcal);
                int withRealLoadBS_GcalCol  = C("WithRealLoadBS_Gcal", config.WithRealLoadBS_Gcal);
                int withRealLoadTU_GcalCol  = C("WithRealLoadTU_Gcal", config.WithRealLoadTU_Gcal);
                int withNP_GcalCol          = C("WithNP_Gcal",         config.WithNP_Gcal);
                int withCab_GcalCol         = C("WithCab_Gcal",        config.WithCab_Gcal);
                int withKSN_GcalCol         = C("WithKSN_Gcal",        config.WithKSN_Gcal);
                int withRealLoad_FN_GcalCol = C("WithRealLoad_FN_Gcal",config.WithRealLoad_FN_Gcal);
                int withDocAndKSN_GcalCol   = C("WithDocAndKSN_Gcal",  config.WithDocAndKSN_Gcal);
                int withDoc_GcalCol         = C("WithDoc_Gcal",        config.WithDoc_Gcal);
                int correctNotesCountCol    = C("CorrectNotesCount",   config.CorrectNotesCount);
                int q_t_m_IsNullCol         = C("Q_T_M_IsNull",        config.Q_T_M_IsNull);
                int actsBSCol               = C("ActsBS",              config.ActsBS);
                int max_Min_Q_MCol          = C("Max_Min_Q_M",         config.Max_Min_Q_M);
                int isValid_VNRCol          = C("IsValid_VNR",         config.IsValid_VNR);
                int isValid_M1_M2Col        = C("IsValid_M1_M2",       config.IsValid_M1_M2);
                int isValid_M1_M2_2Col      = C("IsValid_M1_M2_2",     config.IsValid_M1_M2_2);
                int q_engCol                = C("Q_eng",               config.Q_eng);
                int isValid_TCol            = C("IsValid_T",           config.IsValid_T);

                if (config.UseAutoDetect)
                {
                    ColumnResolver.WarnAutoDetectMissed(filePath, detected, new (string, string)[] {
                        ("Address",             "Адрес"),
                        ("Id",                  "Идентификатор"),
                        ("PU_GcalTotal",        "ПУ Всего, Гкал"),
                        ("PU_WithVNR_Gcal",     "ПУ В т.ч. досчет по ВНР, Гкал"),
                        ("ZM_GcalTotal",        "ЗМ Всего, Гкал"),
                        ("CorrectNotesCount",   "Корректных записей"),
                        ("Q_T_M_IsNull",        "Q, T, M = null"),
                        ("ActsBS",              "Акты БС"),
                        ("Max_Min_Q_M",         "MAX/MIN(Q,M)"),
                        ("IsValid_VNR",         "ВНР>105% или ВНР<75%"),
                        ("IsValid_M1_M2",       "М1 < 0,96 × М2"),
                        ("IsValid_M1_M2_2",     "M1 > 2 x M2"),
                        ("Q_eng",               "Qинж"),
                        ("IsValid_T",           "Т < 0°С или Т > 150°С"),
                    });
                }

                // Автоопределение начала колонок с датами (1, 2, 3...)
                int datesStartCol = ColumnResolver.DetectDatesStart(
                    worksheet, headerRowStart, headerRowEnd);
                if (datesStartCol < 0)
                    datesStartCol = constConfig.DatesColStart;

                int startDatesRow = constConfig.DatesRowStart;
                int rowCount = worksheet.Dimension.Rows;

                if (worksheet.Cells[startDatesRow, datesStartCol].Value != null)
                    ConsumersDataObject.DateList = worksheet.GetDateList(startDatesRow, datesStartCol);

                for (int row = startRow; row <= rowCount; row++)
                {
                    try
                    {
                        if (worksheet.IsEmptyRow(row))
                            break;

                        result.Add(new ConsumersDataObject
                        {
                            Number              = worksheet.Cells[row, numberCol].GetCellDTO(),
                            Address             = worksheet.Cells[row, addressCol].GetCellDTO(),
                            Id                  = worksheet.Cells[row, idCol].GetCellDTO(),
                            PU_GcalTotal        = worksheet.Cells[row, pu_GcalTotalCol].GetCellDTO(),
                            PU_WithVNR_Gcal     = worksheet.Cells[row, pu_WithVNR_GcalCol].GetCellDTO(),
                            ZM_GcalTotal        = worksheet.Cells[row, zm_GcalTotalCol].GetCellDTO(),
                            ZM_WithAverage_Gcal = worksheet.Cells[row, zm_WithAverage_GcalCol].GetCellDTO(),
                            WithBS_Gcal         = worksheet.Cells[row, withBS_GcalCol].GetCellDTO(),
                            WithRealLoadBS_Gcal = worksheet.Cells[row, withRealLoadBS_GcalCol].GetCellDTO(),
                            WithRealLoadTU_Gcal = worksheet.Cells[row, withRealLoadTU_GcalCol].GetCellDTO(),
                            WithNP_Gcal         = worksheet.Cells[row, withNP_GcalCol].GetCellDTO(),
                            WithCab_Gcal        = worksheet.Cells[row, withCab_GcalCol].GetCellDTO(),
                            WithKSN_Gcal        = worksheet.Cells[row, withKSN_GcalCol].GetCellDTO(),
                            WithRealLoad_FN_Gcal= worksheet.Cells[row, withRealLoad_FN_GcalCol].GetCellDTO(),
                            WithDocAndKSN_Gcal  = worksheet.Cells[row, withDocAndKSN_GcalCol].GetCellDTO(),
                            WithDoc_Gcal        = worksheet.Cells[row, withDoc_GcalCol].GetCellDTO(),
                            CorrectNotesCount   = worksheet.Cells[row, correctNotesCountCol].GetCellDTO(),
                            Q_T_M_IsNull        = worksheet.Cells[row, q_t_m_IsNullCol].GetCellDTO(),
                            ActsBS              = worksheet.Cells[row, actsBSCol].GetCellDTO(),
                            Max_Min_Q_M         = worksheet.Cells[row, max_Min_Q_MCol].GetCellDTO(),
                            IsValid_VNR         = worksheet.Cells[row, isValid_VNRCol].GetCellDTO(),
                            IsValid_M1_M2       = worksheet.Cells[row, isValid_M1_M2Col].GetCellDTO(),
                            IsValid_M1_M2_2     = worksheet.Cells[row, isValid_M1_M2_2Col].GetCellDTO(),
                            Q_eng               = worksheet.Cells[row, q_engCol].GetCellDTO(),
                            IsValid_T           = worksheet.Cells[row, isValid_TCol].GetCellDTO(),
                            DaysValue           = worksheet.GetMonthIndicationsList(row, datesStartCol),
                        });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка чтения строки {row}: {ex.Message}");
                    }
                }
            }

            return result;
        }

        public static List<ConsumersDataObject> ReadExcelFileExtra(string filePath)
        {
            var result = new List<ConsumersDataObject>();

            using (var package = ExcelReaderExtensions.GetExcelPackage(filePath))
            {
                var worksheet = package.GetWorksheet(1);
                var config = ConfigModel.Consumers_2Conf;
                var constConfig = ConfigModel.ConstantsConf;

                int headerRowStart = constConfig.Consumers_2TableRowStart > 0
                    ? constConfig.Consumers_2TableRowStart : 1;
                int startRow = constConfig.Consumers_2DataRowStart;
                int headerRowEnd = startRow - 1;

                var aliases2 = config.UseAutoDetect ? ColumnAliases.Consumers_2 : new System.Collections.Generic.Dictionary<string, string[]>();
                var detected = ColumnResolver.ResolveExtending(
                    worksheet, headerRowStart, headerRowEnd, aliases2);

                int C(string field, int fallback) =>
                    ColumnResolver.GetColumnOrFallback(detected, field, fallback);

                int addressCol      = C("Address",      config.Address);
                int pu_GcalTotalCol = C("PU_GcalTotal", config.PU_GcalTotal);
                int zm_GcalTotalCol = C("ZM_GcalTotal", config.ZM_GcalTotal);

                if (config.UseAutoDetect)
                {
                    ColumnResolver.WarnAutoDetectMissed(filePath, detected, new (string, string)[] {
                        ("Address",      "Адрес"),
                        ("PU_GcalTotal", "ПУ Всего, Гкал"),
                        ("ZM_GcalTotal", "ЗМ Всего, Гкал"),
                    });
                }

                int datesStartCol = ColumnResolver.DetectDatesStart(
                    worksheet, headerRowStart, headerRowEnd);
                if (datesStartCol < 0)
                    datesStartCol = constConfig.Dates_2ColStart;

                int startDatesRow = constConfig.Dates_2RowStart;
                int rowCount = worksheet.Dimension.Rows;

                if (worksheet.Cells[startDatesRow, datesStartCol].Value != null)
                    ConsumersDataObject.DateList = worksheet.GetDateList(startDatesRow, datesStartCol);

                for (int row = startRow; row <= rowCount; row++)
                {
                    try
                    {
                        if (worksheet.IsEmptyRow(row))
                            break;

                        result.Add(new ConsumersDataObject
                        {
                            Address      = worksheet.Cells[row, addressCol].GetCellDTO(),
                            PU_GcalTotal = worksheet.Cells[row, pu_GcalTotalCol].GetCellDTO(),
                            ZM_GcalTotal = worksheet.Cells[row, zm_GcalTotalCol].GetCellDTO(),
                            DaysValue    = worksheet.GetMonthIndicationsList(row, datesStartCol),
                        });
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
