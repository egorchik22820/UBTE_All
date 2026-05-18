using PoteryGVS.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PoteryGVS.Extensions.FilterExtensions
{
    public static class GVSDataFilterExtension
    {
        public static List<GVSDataObject> GetGroupedDataByHeatZone(this List<GVSDataObject> GVSData)
        {
            return GVSData
                .Where(x => !string.IsNullOrEmpty(x.HeatSupplyZone)) // фильтруем пустые зоны
                .GroupBy(x => x.HeatSupplyZone)
                .Select(g =>
                {
                    var totalGcal = g.Sum(x => x.Rashod_ODPU_GVS_Gcal);
                    var totalM3 = g.Sum(x => x.Rashod_ODPU_GVS_m3);

                    return new GVSDataObject()
                    {
                        HeatSupplyZone = g.Key,
                        Rashod_ODPU_GVS_Gcal = totalGcal,
                        Rashod_ODPU_GVS_m3 = totalM3,
                        h_CoeffHeatContent_WithOut_ODPU = totalM3 != 0 ? totalGcal / totalM3 : 0 // защита от деления на 0
                    };
                })
                .ToList();
        }

        public static List<GVSDataObject> DeleteEmptyNull_m3(this List<GVSDataObject> GVSData)
        {
            return GVSData.Where(x => x.PO_1C_m3 != 0 ||
                                        !string.IsNullOrWhiteSpace(x.PO_1C_m3.ToString()))
                                        .ToList();
        }

        public static List<GVSDataObject> GetEmptyNull_m3(this List<GVSDataObject> GVSData)
        {
            return GVSData.Where(x => x.PO_1C_m3 == 0 ||
                                        string.IsNullOrWhiteSpace(x.PO_1C_m3.ToString()))
                                        .ToList();
        }



        
    }
}
