using PotrebAuto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PotrebAuto.Extensions.Filters
{
    public static class ConsumersFilter
    {
        public static List<ConsumersDataObject> GetFiltered(this List<ConsumersDataObject> consumers)
        {
            return consumers.Where(x => x.TU_AIIS.Value != null)
                                .ToList();
        }
    }
}
