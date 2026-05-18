using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using PotrebAuto.Configuration;
using PotrebAuto.Extensions;
using PotrebAuto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PotrebAuto.Servises.ExcelReaderServices
{
    public class ConsumersFileReaderService
    {
        public static List<ConsumersDataObject> ReadExcelFile(string filePath)
        {
            var result = new List<ConsumersDataObject>();

            using (var package = ExcelReaderExtensions.GetExcelPackage(filePath))
            {
                // Получаем первый лист из книги
                var worksheet = package.GetWorksheet(1);

                // Кэшируем конфигурацию перед циклом
                var config = ConfigModel.ConsumersConf;
                var constConfig = ConfigModel.ConstantsConf;
                int startDatesRow = constConfig.DatesRowStart;
                int startDatesCol = constConfig.DatesColStart;
                int startRow = constConfig.ConsumersDataRowStart;
                int numberCol = config.Number;
                int addressCol = config.Address;
                int pu_GcalTotalCol = config.PU_GcalTotal;
                int pu_WithVNR_GcalCol = config.PU_WithVNR_Gcal;
                int zm_GcalTotalCol = config.ZM_GcalTotal;
                int zm_WithAverage_GcalCol = config.ZM_WithAverage_Gcal;
                int withBS_GcalCol = config.WithBS_Gcal;
                int withRealLoadBS_GcalCol = config.WithRealLoadBS_Gcal;
                int withRealLoadTU_GcalCol = config.WithRealLoadTU_Gcal;
                int withNP_GcalCol = config.WithNP_Gcal;
                int withCab_GcalCol = config.WithCab_Gcal;
                int withKSN_GcalCol = config.WithKSN_Gcal;
                int withRealLoad_FN_GcalCol = config.WithRealLoad_FN_Gcal;
                int withDocAndKSN_GcalCol = config.WithDocAndKSN_Gcal;
                int withDoc_GcalCol = config.WithDoc_Gcal;
                int correctNotesCountCol = config.CorrectNotesCount;
                int q_t_m_IsNullCol = config.Q_T_M_IsNull;
                int actsBSCol = config.ActsBS;
                int max_Min_Q_MCol = config.Max_Min_Q_M;
                int isValid_VNRCol = config.IsValid_VNR;
                int isValid_M1_M2Col = config.IsValid_M1_M2;
                int isValid_M1_M2_2Col = config.IsValid_M1_M2_2;
                int q_engCol = config.Q_eng;
                int isValid_TCol = config.IsValid_T;
                int daysValueCol = constConfig.DatesColStart;

                // Определяем количество строк с данными
                int rowCount = worksheet.Dimension.Rows;

                if (worksheet.Cells[startDatesRow, startDatesCol].Value != null)// даты
                {
                    ConsumersDataObject.DateList = worksheet.GetDateList(startDatesRow, startDatesCol);
                }

                for (int row = startRow; row <= rowCount; row++)
                {
                    try
                    {
                        if (worksheet.IsEmptyRow(row))
                            break;

                        var data = new ConsumersDataObject
                        {
                            Number = worksheet.Cells[row, numberCol].GetCellDTO(),
                            Address = worksheet.Cells[row, addressCol].GetCellDTO(),
                            Id = worksheet.Cells[row, 3].GetCellDTO(),
                            PU_GcalTotal = worksheet.Cells[row, pu_GcalTotalCol].GetCellDTO(),
                            PU_WithVNR_Gcal = worksheet.Cells[row, pu_WithVNR_GcalCol].GetCellDTO(),
                            ZM_GcalTotal = worksheet.Cells[row, zm_GcalTotalCol].GetCellDTO(),
                            ZM_WithAverage_Gcal = worksheet.Cells[row, zm_WithAverage_GcalCol].GetCellDTO(),
                            WithBS_Gcal = worksheet.Cells[row, withBS_GcalCol].GetCellDTO(),
                            WithRealLoadBS_Gcal = worksheet.Cells[row, withRealLoadBS_GcalCol].GetCellDTO(),
                            WithRealLoadTU_Gcal = worksheet.Cells[row, withRealLoadTU_GcalCol].GetCellDTO(),
                            WithNP_Gcal = worksheet.Cells[row, withNP_GcalCol].GetCellDTO(),
                            WithCab_Gcal = worksheet.Cells[row, withCab_GcalCol].GetCellDTO(),
                            WithKSN_Gcal = worksheet.Cells[row, withKSN_GcalCol].GetCellDTO(),
                            WithRealLoad_FN_Gcal = worksheet.Cells[row, withRealLoad_FN_GcalCol].GetCellDTO(),
                            WithDocAndKSN_Gcal = worksheet.Cells[row, withDocAndKSN_GcalCol].GetCellDTO(),
                            WithDoc_Gcal = worksheet.Cells[row, withDoc_GcalCol].GetCellDTO(),
                            CorrectNotesCount = worksheet.Cells[row, correctNotesCountCol].GetCellDTO(),
                            Q_T_M_IsNull = worksheet.Cells[row, q_t_m_IsNullCol].GetCellDTO(),
                            ActsBS = worksheet.Cells[row, actsBSCol].GetCellDTO(),
                            Max_Min_Q_M = worksheet.Cells[row, max_Min_Q_MCol].GetCellDTO(),
                            IsValid_VNR = worksheet.Cells[row, isValid_VNRCol].GetCellDTO(),
                            IsValid_M1_M2 = worksheet.Cells[row, isValid_M1_M2Col].GetCellDTO(),
                            IsValid_M1_M2_2 = worksheet.Cells[row, isValid_M1_M2_2Col].GetCellDTO(),
                            Q_eng = worksheet.Cells[row, q_engCol].GetCellDTO(),
                            IsValid_T = worksheet.Cells[row, isValid_TCol].GetCellDTO(),
                            DaysValue = worksheet.GetMonthIndicationsList(row,daysValueCol)

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
