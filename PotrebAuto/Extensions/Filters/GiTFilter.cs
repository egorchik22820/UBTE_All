using PotrebAuto.Models;
using PotrebAuto.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PotrebAuto.Extensions.Filters
{
    public static class GiTFilter
    {
        public static Dictionary<string, GiTDataObject> GetFilteredDict(this List<GiTDataObject> GiTData)
        {
            return GiTData.Where(x => x.BuildingId.Value != null).GroupBy(x => x.BuildingId.Value)
                                            .ToDictionary(g => g.Key.ToString(), g => g.First());
        }
    }
}
