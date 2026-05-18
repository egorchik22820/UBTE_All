using PoteryGVS.Configuration;
using PoteryGVS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoteryGVS.Extensions.FilterExtensions
{
    public static class NakladDataFilterExtension
    {
        private static readonly string NakladDepartmentOren = ConfigModel.NakladDepartmentOren;
        private static readonly string NakladDepartmentMedno = ConfigModel.NakladDepartmentMedno;
        private static readonly string NakladHeatSourse = ConfigModel.NakladHeatSourse;
        private static readonly string NakladLoadTypeHeatWater = ConfigModel.NakladLoadTypeHeatWater;
        private static readonly string NakladLoadTypeGvs = ConfigModel.NakladLoadTypeGvs;
        private static readonly string NakladBuildingType = ConfigModel.NakladBuildingType;
        private static readonly string NakladNomenclatureUnit_Gcal = ConfigModel.NakladNomenclatureUnit_Gcal;
        private static readonly string NakladNomenclatureUnit_m3 = ConfigModel.NakladNomenclatureUnit_m3;
        private static readonly string configYear = ConfigModel.Naklad.RecalcYearValue;

        public static List<NakladDataObject> GetNeedObjects_With_ODPU(this List<NakladDataObject> nakladData)
        {
            return nakladData.GetObjects()
                             .GetGroupedData_JoinString_V2();
        }
        public static List<NakladDataObject> GetNeedObjects_WithOut_ODPU(this List<NakladDataObject> nakladData,
                                                                                List<GVSDataObject> GVSData_With_ODPU)
        {
            return nakladData.GetObjects()
                             .GetGroupedData_JoinString_V2()
                             .DeleteMatched_With_ODPU_BuildingId(GVSData_With_ODPU);
        }

        public static List<NakladDataObject> DeleteEmptyNull_m3(this List<NakladDataObject> nakladData)
        {
            return nakladData.Where(x => !string.IsNullOrWhiteSpace(x.Quantity_m3.ToString()))
                                .Where(x => x.Quantity_m3 != 0)
                                .ToList();
        }
        public static List<NakladDataObject> GetObjects(this List<NakladDataObject> nakladData)
        {

            return nakladData.Where(x => (x.Department.ToLowerInvariant() == NakladDepartmentOren.ToLowerInvariant() ||
                                        x.Department.ToLowerInvariant() == NakladDepartmentMedno.ToLowerInvariant()) &&
                                        x.HeatSourse.ToLowerInvariant() != NakladHeatSourse.ToLowerInvariant() &&
                                        (x.LoadType.ToLowerInvariant() == NakladLoadTypeHeatWater.ToLowerInvariant() ||
                                        x.LoadType.ToLowerInvariant() == NakladLoadTypeGvs.ToLowerInvariant()) &&
                                        x.BuildingType.ToLowerInvariant() != NakladBuildingType.ToLowerInvariant() &&
                                        (x.RecalcYear == configYear ||
                                        string.IsNullOrWhiteSpace(x.RecalcYear)))
                                        .ToList();
        }

        // скорее всего проблема в том, что брать данные для кол-ва Гкал/м3 нужно из изначального nakladData
        public static List<NakladDataObject> GetGroupedData_JoinString(this List<NakladDataObject> nakladData)
        {
            return nakladData.GroupBy(x => x.BuildinId_BuildingAddress_Join)
                             .Select(g =>
                             {
                                 var first = g.First(); // кэшируем вызов
                                 var sumGcal = g.Sum(x => x.NomenclatureUnit.ToLowerInvariant() == NakladNomenclatureUnit_Gcal.ToLowerInvariant()
                                                                                                                                    ? x.QuantityTotal : 0);
                                 var sum_m3 = g.Sum(x => x.NomenclatureUnit.ToLowerInvariant() == NakladNomenclatureUnit_m3.ToLowerInvariant()
                                                                                                                                    ? x.QuantityTotal : 0);

                                 return new NakladDataObject
                                 {
                                     BuildinId_BuildingAddress_Join = g.Key,
                                     BuildingId = first.BuildingId,
                                     AddressTU = first.AddressTU,
                                     Quantity_Gcal = sumGcal,
                                     Quantity_m3 = sum_m3
                                 };
                             })
                             .ToList();
        }


        // по идее должен вычислять сумму из изначальной накладной, а не из сгруппированной
        public static List<NakladDataObject> GetGroupedData_JoinString_V2(this List<NakladDataObject> nakladData)
        {
            return nakladData.GroupBy(x => x.BuildinId_BuildingAddress_Join)
                             .Select(g =>
                             {
                                 var first = g.First();
                                 decimal gcalSum = 0;
                                 decimal m3Sum = 0;

                                 decimal gcalPosSum = 0;
                                 decimal m3PosSum = 0;

                                 foreach (var item in g)
                                 {
                                     if (item.NomenclatureUnit.ToLowerInvariant() == NakladNomenclatureUnit_Gcal.ToLowerInvariant())
                                     {
                                         gcalSum += item.QuantityTotal;
                                         gcalPosSum += item.QuantityTotal > 0 ? item.QuantityTotal : 0;
                                     }
                                     else if (item.NomenclatureUnit.ToLowerInvariant() == NakladNomenclatureUnit_m3.ToLowerInvariant())
                                     {
                                         m3Sum += item.QuantityTotal;
                                         m3PosSum += item.QuantityTotal > 0 ? item.QuantityTotal : 0;
                                     }
                                 }

                                 return new NakladDataObject
                                 {
                                     BuildinId_BuildingAddress_Join = g.Key,
                                     BuildingId = first.BuildingId,
                                     AddressTU = first.AddressTU,
                                     Quantity_Gcal = gcalSum,
                                     Quantity_m3 = m3Sum,
                                     Quantity_WithOutRecalc_Gcal = gcalPosSum,
                                     Quantity_WithOutRecalc_m3 = m3PosSum,
                                     BuildingType = first.BuildingType
                                 };
                             })
                             .ToList();
        }

        public static List<NakladDataObject> GetGroupedData_BuildingId(this List<NakladDataObject> nakladData)// опт сумму
        {
            return nakladData.GroupBy(x => x.BuildingId)
                             .Select(g =>
                             {
                                 var first = g.First(); // кэшируем вызов
                                 var sumGcal = g.Sum(x => x.NomenclatureUnit.ToLowerInvariant() == NakladNomenclatureUnit_Gcal.ToLowerInvariant()
                                                                                                                                    ? x.QuantityTotal : 0);
                                 var sum_m3 = g.Sum(x => x.NomenclatureUnit.ToLowerInvariant() == NakladNomenclatureUnit_m3.ToLowerInvariant()
                                                                                                                                    ? x.QuantityTotal : 0);

                                 return new NakladDataObject
                                 {
                                     BuildingId = g.Key,
                                     AddressTU = first.AddressTU,
                                     Quantity_Gcal = sumGcal,
                                     Quantity_m3 = sum_m3,
                                 };
                             })
                             .ToList();
        }

        public static List<NakladDataObject> DeleteMatched_With_ODPU_BuildingId(this List<NakladDataObject> nakladData,
                                                                                    List<GVSDataObject>GVSData_With_ODPU)
        {
            var odpuBuildingIds = GVSData_With_ODPU.Select(x => x.BuildingId).ToHashSet();

            return nakladData.Where(x => !odpuBuildingIds.Contains(x.BuildingId)).ToList();
        }
    }
}
