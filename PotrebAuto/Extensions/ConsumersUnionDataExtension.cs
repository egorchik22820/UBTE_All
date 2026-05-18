using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PotrebAuto.Models;
using PotrebAuto.Models.DTO;
using PotrebAuto.Configuration;
using PotrebAuto.Servises;

namespace PotrebAuto.Extensions
{
    public static class ConsumersUnionDataExtension
    {
        private readonly static string _noData = ConfigModel.NoData;
        public static List<ConsumersDataObject> GetUnionData(this List<ConsumersDataObject> consumers,
                                                     Dictionary<string, SourcesAndConsumersObject> SACDict)
        {
            foreach (var cm in consumers)
            {
                SACDict.TryGetValue(cm.TU_AIIS.Value?.ToString(), out var sacItem);

                cm.ObjectId = new CellDTO { Value = sacItem?.Obj_Id ?? _noData };

                // Сложение PU_GcalTotal и ZM_GcalTotal
                cm.PO_AIIS_Total = new CellDTO
                {
                    Value = DataServices.AddDecimals(
                        cm.PU_GcalTotal.Value,
                        cm.ZM_GcalTotal.Value
                    )
                };

                cm.ColorDaysCount = new CellDTO { Value = cm.DaysValue.GetColorDaysCount() };
            }
            return consumers;
        }

        
    }
}
