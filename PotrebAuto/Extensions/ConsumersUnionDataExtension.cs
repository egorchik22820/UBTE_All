using System.Collections.Generic;
using System.Linq;
using PotrebAuto.Models;
using PotrebAuto.Models.DTO;
using PotrebAuto.Configuration;
using PotrebAuto.Servises;

namespace PotrebAuto.Extensions
{
    public static class ConsumersUnionDataExtension
    {
        private readonly static string _noData = ConfigModel.NoData;
        private readonly static string _empty = string.Empty;
        public static List<ConsumersDataObject> GetUnionData(this List<ConsumersDataObject> consumers,
                                                     Dictionary<string, SourcesAndConsumersObject> SACDict)
        {
            foreach (var cm in consumers)
            {
                SACDict.TryGetValue(cm.TU_AIIS.Value?.ToString(), out var sacItem);

                cm.ObjectId = new CellDTO { Value = sacItem?.Obj_Id ?? _noData };

            }
            return consumers;
        }

        public static List<ConsumersDataObject> GetUnionDataExtra(this List<ConsumersDataObject> consumers, Dictionary<string, ConsumersDataObject> consumersSecond,
                                                                     Dictionary<string, SourcesAndConsumersObject> SACDict,
                                                                     Dictionary<string, GiTDataObject> GiTData,
                                                                     Dictionary<string, QlickDataObject> qlickData)
        {

            foreach (var cm in consumers)
            {

                consumersSecond.TryGetValue(cm.TU_AIIS?.Value?.ToString(), out var secondItem);
                if (secondItem == null)
                {
                    // Fallback: второй файл без гиперссылок — ищем по нормализованному тексту адреса
                    string addrKey = ColumnResolver.Normalize(cm.Address?.Value?.ToString());
                    if (!string.IsNullOrEmpty(addrKey))
                        consumersSecond.TryGetValue(addrKey, out secondItem);
                }

                cm.PO_AIIS_Total_2 = new CellDTO { Value = secondItem?.PO_AIIS_Total?.Value ?? _empty };
                cm.ColorDaysCount_2 = new CellDTO { Value = secondItem?.ColorDaysCount?.Value ?? _empty };
                //cm.ColorDaysCount_2 = new CellDTO { Value = secondItem?.ColorDaysCount.Value ?? _noData };
                cm.PU_GcalTotal_2 = new CellDTO { Value = secondItem?.PU_GcalTotal?.Value ?? _empty };
                cm.ZM_GcalTotal_2 = new CellDTO { Value = secondItem?.ZM_GcalTotal?.Value ?? _empty };
                cm.DaysValue_2 = secondItem?.DaysValue;
                ConsumersDataObject.DateList_2 = ConsumersDataObject.DateListTemp;


                SACDict.TryGetValue(cm.TU_AIIS.Value?.ToString(), out var sacItem);

                cm.ObjectId = new CellDTO { Value = sacItem?.Obj_Id ?? _noData };



                qlickData.TryGetValue(cm.ObjectId?.Value?.ToString(), out var qlickItem);

                cm.BuildingId = new CellDTO { Value = qlickItem?.BuildingId?.Value ?? _noData };


                GiTData.TryGetValue(cm.BuildingId?.Value?.ToString(), out var GiTItem);
                
                cm.BuildingType = new CellDTO { Value = GiTItem?.BuildingType?.Value ?? _noData };
                cm.CityGiT = new CellDTO { Value = GiTItem?.City?.Value ?? _noData };


                
            }
            return consumers;
        }
    }
}
