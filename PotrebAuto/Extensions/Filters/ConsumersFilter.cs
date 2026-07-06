using PotrebAuto.Configuration;
using PotrebAuto.Models;
using PotrebAuto.Servises;
using System.Collections.Generic;
using System.Linq;

namespace PotrebAuto.Extensions.Filters
{
    public static class ConsumersFilter
    {
        public static List<ConsumersDataObject> GetFiltered(this List<ConsumersDataObject> consumers)
        {
            return consumers.Where(x => x.TU_AIIS.Value != null)
                                .ToList();
        }

        public static Dictionary<string, ConsumersDataObject> GetFilteredDict(this List<ConsumersDataObject> consumers)
        {
            string noData = ConfigModel.NoData;
            var result = new Dictionary<string, ConsumersDataObject>();

            foreach (var item in consumers.Where(x => x.TU_AIIS.Value != null))
            {
                string tuKey = item.TU_AIIS.Value.ToString();
                if (tuKey == noData)
                {
                    // Нет гиперссылки в адресе — используем нормализованный текст адреса как ключ
                    string addrKey = ColumnResolver.Normalize(item.Address?.Value?.ToString());
                    if (!string.IsNullOrEmpty(addrKey) && !result.ContainsKey(addrKey))
                        result[addrKey] = item;
                }
                else if (!result.ContainsKey(tuKey))
                {
                    result[tuKey] = item;
                }
            }
            return result;
        }
    }
}
