using PotrebAuto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PotrebAuto.Extensions.Filters
{
    public static class SACFilter
    {
        public static Dictionary<string, SourcesAndConsumersObject> GetFilteredDict(this List<SourcesAndConsumersObject> sourcesAndConsumers)
        {
            return sourcesAndConsumers.GroupBy(x => x.TU_Id).ToDictionary(g => g.Key, g => g.First());
        }
    }
}
