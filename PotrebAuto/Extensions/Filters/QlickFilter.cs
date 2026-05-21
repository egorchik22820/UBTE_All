using PotrebAuto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PotrebAuto.Models.DTO;

namespace PotrebAuto.Extensions.Filters
{
    public static class QlickFilter
    {
        public static Dictionary<string, QlickDataObject> GetFilteredDict(this List<QlickDataObject> qlickData)
        {
            return qlickData.Where(x => x.BuildingGUID.Value != null).GroupBy(x => x.BuildingGUID.Value)
                                            .ToDictionary(g => g.Key.ToString(), g => g.First());
        }
    }
}
