using PoteryGVS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoteryGVS.Services;
using PoteryGVS.Configuration;

namespace PoteryGVS.Extensions.FilterExtensions
{
    public static class GVSDataUnionExtension
    {
        private static readonly string NoData = ConfigModel.NoData;
        private static readonly string ByIndications = ConfigModel.ByIndications;
        private static readonly string Normative = ConfigModel.Normative;

        public static List<GVSDataObject> GetUnionData_With_ODPU(this List<GVSDataObject> GVSData, List<NakladDataObject> nakladData,
                                                        List<MPDataObject> MPData, List<ODNDataObject> ODNdata,
                                                        List<GVSDataObject> oldData)
        {
            var nakladDict = nakladData.GroupBy(x => x.BuildingId)
                                      .ToDictionary(g => g.Key, g => g.First());

            var odnDict = ODNdata.GroupBy(x => x.BuildingId)
                                .ToDictionary(g => g.Key, g => g.First());

            var oldDict = oldData.GroupBy(x => x.BuildingId)
                                .ToDictionary(g => g.Key, g => g.First());

            // ПРЕДВАРИТЕЛЬНЫЙ РАСЧЕТ CAPACITY
            var result = new List<GVSDataObject>(MPData.Count);

            foreach (var mp in MPData)
            {
                nakladDict.TryGetValue(mp.BuildingId, out var nakladItem);
                odnDict.TryGetValue(mp.BuildingId, out var odnItem);
                oldDict.TryGetValue(nakladItem?.BuildingId ?? mp.BuildingId, out var oldItem);

                result.Add(new GVSDataObject
                {
                    BuildingId = mp.BuildingId ?? NoData,
                    TU_AIIS_Id = mp.TU_AIIS_Id ?? NoData,
                    City = DataServices.TryGetCityByAddress(mp.Address),
                    HeatSupplyZone = !string.IsNullOrWhiteSpace(oldItem?.HeatSupplyZone) ? oldItem.HeatSupplyZone : NoData,
                    SysPU_Id = mp.SysPU_Id ?? NoData,
                    Address = mp.Address ?? NoData,
                    Rashod_ODPU_GVS_Gcal = mp?.Q_Calc ?? 0,
                    Rashod_ODPU_GVS_m3 = mp?.dV_Calc ?? 0,
                    PO_1C_Gcal = nakladItem?.Quantity_Gcal ?? 0,
                    PO_1C_m3 = nakladItem?.Quantity_m3 ?? 0,
                    PO_1C_WithOutRecalc_Gcal = nakladItem?.Quantity_WithOutRecalc_Gcal ?? 0,
                    PO_1C_WithOutRecalc_m3 = nakladItem?.Quantity_WithOutRecalc_m3 ?? 0,
                    ZTP = !string.IsNullOrWhiteSpace(oldItem?.ZTP) ? oldItem.ZTP : NoData,
                    NegativeODN_Gcal = odnItem?.NegativeODN_Gcal ?? 0,
                    NegativeODN_m3 = odnItem?.NegativeODN_m3 ?? 0,
                    BuildingType = nakladItem.BuildingType ?? NoData,
                });
            }

            return result;
        }



        // по идее должен считать ПО из изначальной накладной(из параметра), ане из сгруппированной
        //public static List<GVSDataObject> GetUnionData_With_ODPU_V2(this List<GVSDataObject> GVSData,
        //                                                                List<NakladDataObject> nakladData, List<MPDataObject> MPData,
        //                                                                List<ODNDataObject> ODNdata, List<GVSDataObject> oldData)
        //{
        //    var nakladDict = nakladData.GroupBy(x => x.BuildingId)
        //                              .ToDictionary(g => g.Key, g => new {
        //                                  Quantity_Gcal = g.Sum(x => x.Quantity_Gcal),
        //                                  Quantity_m3 = g.Sum(x => x.Quantity_m3)
        //                              });

        //    var odnDict = ODNdata.GroupBy(x => x.BuildingId)
        //                        .ToDictionary(g => g.Key, g => g.First());

        //    var oldDict = oldData.GroupBy(x => x.BuildingId)
        //                        .ToDictionary(g => g.Key, g => g.First());

        //    return MPData.Select(mp =>
        //    {
        //        nakladDict.TryGetValue(mp.BuildingId, out var nakladItem);
        //        odnDict.TryGetValue(mp.BuildingId, out var odnItem);
        //        oldDict.TryGetValue(mp.BuildingId, out var oldItem);

        //        return new GVSDataObject
        //        {
        //            BuildingId = mp.BuildingId,
        //            City = oldItem?.City ?? string.Empty,
        //            HeatSupplyZone = oldItem?.HeatSupplyZone ?? string.Empty,
        //            TU_AIIS_Id = mp.TU_AIIS_Id,
        //            SysPU_Id = mp.SysPU_Id,
        //            Address = mp.Address,
        //            Rashod_ODPU_GVS_Gcal = mp.Q_Calc,
        //            Rashod_ODPU_GVS_m3 = mp.VNR_Calc,
        //            PO_1C_Gcal = nakladItem?.Quantity_Gcal ?? 0,
        //            PO_1C_m3 = nakladItem?.Quantity_m3 ?? 0,
        //            NegativeODN_Gcal = odnItem?.NegativeODN_Gcal ?? 0,
        //            NegativeODN_m3 = odnItem?.NegativeODN_m3 ?? 0,
        //            BuildingType = oldItem?.BuildingType ?? string.Empty,////////////////
        //            ZTP = oldItem?.ZTP ?? string.Empty//////////////////
        //        };
        //    }).ToList();
        //}

        //public static List<GVSDataObject> GetUnionData_With_ODPU(this List<GVSDataObject> GVSData, List<NakladDataObject> nakladData,
        //                                                    List<MPDataObject> MPData, List<ODNDataObject> ODNdata)
        //{
        //    return MPData.Select(mp => new GVSDataObject
        //    {
        //        BuildingId = mp.BuildingId,
        //        TU_AIIS_Id = mp.TU_AIIS_Id,
        //        SysPU_Id = mp.SysPU_Id,
        //        Address = mp.Address,
        //        Rashod_ODPU_GVS_Gcal = mp.Q_Calc,
        //        Rashod_ODPU_GVS_m3 = mp.VNR_Calc,
        //        PO_1C_Gcal = nakladData.FirstOrDefault(x => x.BuildingId == mp.BuildingId)?.Quantity_Gcal ?? 0,
        //        PO_1C_m3 = nakladData.FirstOrDefault(x => x.BuildingId == mp.BuildingId)?.Quantity_m3 ?? 0,
        //        NegativeODN_Gcal = ODNdata.FirstOrDefault(x => x.BuildingId == mp.BuildingId)?.NegativeODN_Gcal ?? 0,
        //        NegativeODN_m3 = ODNdata.FirstOrDefault(x => x.BuildingId == mp.BuildingId)?.NegativeODN_m3 ?? 0
        //    }).ToList();
        //}

        //public static List<GVSDataObject> GetUnionData_WithOut_ODPU(this List<GVSDataObject> GVSData, List<NakladDataObject> nakladData,
        //                                                                                                List<GVSDataObject> GVS_with_ODPU)
        //{
        //    var temp = nakladData.Select(nk => new GVSDataObject
        //    {
        //        BuildingId = nk.BuildingId,
        //        Address = nk.AddressTU,
        //        h_CoeffHeatContent_WithOut_ODPU = GVS_with_ODPU.GetGroupedDataByHeatZone()
        //                                                        .FirstOrDefault(x => x.BuildingId == nk.BuildingId)?
        //                                                        .h_CoeffHeatContent_WithOut_ODPU ?? 0,

        //        PO_1C_Gcal = nakladData.FirstOrDefault(x => x.BuildingId == nk.BuildingId)?.Quantity_Gcal ?? 0,
        //        PO_1C_m3 = nakladData.FirstOrDefault(x => x.BuildingId == nk.BuildingId)?.Quantity_m3 ?? 0
        //    }).ToList();

        //    foreach (var item in temp)
        //    {
        //        item.LossGVS_Without_ODPU = item.PO_1C_m3 * (item.h_CoeffHeatContent_WithOut_ODPU - item.h_Normative);
        //    }

        //    return temp;
        //}

        public static List<GVSDataObject> GetUnionData_WithOut_ODPU(this List<GVSDataObject> GVSData,
                                     List<NakladDataObject> nakladData, List<GVSDataObject> GVS_with_ODPU,
                                     List<GVSDataObject> oldData)
        {
            // 1. ПРЕДВАРИТЕЛЬНЫЕ ВЫЧИСЛЕНИЯ (один раз!) - БЕЗ ИЗМЕНЕНИЙ
            var coefficientsByHeatZone = GVS_with_ODPU
                 .GetGroupedDataByHeatZone()
                 .Where(x => !string.IsNullOrEmpty(x.HeatSupplyZone))
                 .ToDictionary(x => x.HeatSupplyZone, x => x.h_CoeffHeatContent_With_ODPU);

            var nakladByBuildingId = nakladData
                .Where(x => !string.IsNullOrEmpty(x.BuildingId))
                .ToDictionary(x => x.BuildingId, x => x);

            var oldDict = oldData
                .Where(x => !string.IsNullOrEmpty(x.BuildingId))
                .GroupBy(x => x.BuildingId)
                .ToDictionary(g => g.Key, g => g.First());

            // Оптимизированное вычисление CityConst для каждого города - БЕЗ ИЗМЕНЕНИЙ
            var cityConstants = GVS_with_ODPU
                .Where(x => !string.IsNullOrEmpty(x.City) && x.PO_1C_m3 != 0)
                .GroupBy(x => x.City)
                .ToDictionary(
                    g => g.Key,
                    g => g.Sum(x => x.Rashod_ODPU_GVS_m3) / g.Sum(x => x.PO_1C_m3)// Mismatch
                );

            // средние h_CoefHeatContent по городам - БЕЗ ИЗМЕНЕНИЙ
            var city_h_Averages = GVS_with_ODPU
                .Where(x => !string.IsNullOrEmpty(x.Rashod_ODPU_GVS_m3.ToString()) || x.Rashod_ODPU_GVS_m3 != 0)
                .GroupBy(x => x.City)
                .ToDictionary(
                    g => g.Key,
                    g => g.Sum(x => x.Rashod_ODPU_GVS_Gcal) / g.Sum(x => x.Rashod_ODPU_GVS_m3)
                );

            // 2. СОЗДАНИЕ РЕЗУЛЬТАТА - ОПТИМИЗАЦИЯ
            var filteredNaklad = nakladData.Where(nk => !string.IsNullOrEmpty(nk.BuildingId)).ToList();
            var result = new List<GVSDataObject>(filteredNaklad.Count); // ← Capacity

            foreach (var nk in filteredNaklad) // ← обычный цикл вместо LINQ
            {
                var nakladItem = nakladByBuildingId[nk.BuildingId];
                oldDict.TryGetValue(nk.BuildingId, out var oldItem);

                // Безопасное получение коэффициента по HeatSupplyZone - БЕЗ ИЗМЕНЕНИЙ
                decimal heatCoeff = 0;
                string heatSupplyZone = oldItem?.HeatSupplyZone;
                if (!string.IsNullOrEmpty(heatSupplyZone) &&
                    coefficientsByHeatZone.ContainsKey(heatSupplyZone))
                {
                    heatCoeff = coefficientsByHeatZone[heatSupplyZone];
                }

                var resultItem = new GVSDataObject
                {
                    BuildingId = nk.BuildingId ?? NoData,
                    HeatSupplyZone = !string.IsNullOrWhiteSpace(oldItem?.HeatSupplyZone) ? oldItem.HeatSupplyZone : NoData,
                    Address = nk.AddressTU ?? NoData,
                    h_CoeffHeatContent_WithOut_ODPU = heatCoeff,
                    PO_1C_Gcal = nakladItem?.Quantity_Gcal ?? 0,
                    PO_1C_m3 = nakladItem?.Quantity_m3 ?? 0,
                    PO_1C_WithOutRecalc_Gcal = nakladItem?.Quantity_WithOutRecalc_Gcal ?? 0,
                    PO_1C_WithOutRecalc_m3 = nakladItem?.Quantity_WithOutRecalc_m3 ?? 0,
                    BuildingType = !string.IsNullOrWhiteSpace(oldItem?.BuildingType) ? oldItem.BuildingType : NoData,
                    ZTP = !string.IsNullOrWhiteSpace(oldItem?.ZTP) ? oldItem.ZTP : NoData
                };

                resultItem.City = DataServices.TryGetCityByAddress(resultItem.Address);

                // берем средний коэф по городу - БЕЗ ИЗМЕНЕНИЙ
                string resultCity = resultItem.City;
                bool hasCity = !string.IsNullOrEmpty(resultCity);
                bool hasCityInAverages = hasCity && city_h_Averages.ContainsKey(resultCity);

                if (hasCityInAverages)
                {
                    decimal cityAverage = city_h_Averages[resultCity];
                    if (resultItem.h_CoeffHeatContent_WithOut_ODPU == 0)
                    {
                        resultItem.h_CoeffHeatContent_WithOut_ODPU = cityAverage;
                    }
                }

                resultItem.LossGVS_Without_ODPU = resultItem.PO_1C_m3 * (resultItem.h_CoeffHeatContent_WithOut_ODPU - resultItem.h_Normative);

                // Оптимизированное вычисление LossGVS_WithOut_ODPU_Mismatch - БЕЗ ИЗМЕНЕНИЙ
                decimal cityConst = 0;
                bool hasCityInConstants = hasCity && cityConstants.ContainsKey(resultCity);
                if (hasCityInConstants)
                {
                    cityConst = cityConstants[resultCity];
                }

                resultItem.LossGVS_WithOut_ODPU_Mismatch = (resultItem.PO_1C_m3 * cityConst - resultItem.PO_1C_m3) * resultItem.h_Normative;

                result.Add(resultItem);
            }

            return result;
        }



        public static List<GVSDataObject> GetUnionData_With_ITP(this List<GVSDataObject> GVSData,
                                             List<GVSDataObject> with_ODPU, List<GVSDataObject> withOut_ODPU,
                                                List<GVSDataObject> oldData, List<MPDataObject> MPData)
        {
            var filteredWithODPU = with_ODPU.GetEmptyNull_m3();
            var filteredWithOutODPU = withOut_ODPU.GetEmptyNull_m3();

            // Объединяем оба списка
            var allFilteredData = filteredWithODPU.Concat(filteredWithOutODPU).ToList();

            // 1. ПРЕДВАРИТЕЛЬНЫЕ ВЫЧИСЛЕНИЯ (один раз!)
            var coefficientsByHeatZone = with_ODPU
                .GetGroupedDataByHeatZone()
                .Where(x => !string.IsNullOrEmpty(x.HeatSupplyZone))
                .ToDictionary(x => x.HeatSupplyZone, x => x.h_CoeffHeatContent_With_ODPU);

            var oldDict = oldData
                .Where(x => !string.IsNullOrEmpty(x.BuildingId))
                .GroupBy(x => x.BuildingId)
                .ToDictionary(g => g.Key, g => g.First());

            var MPDict = MPData
                .Where(x => !string.IsNullOrEmpty(x.BuildingId))
                .GroupBy(x => x.BuildingId)
                .ToDictionary(g => g.Key, g => g.First());

            // средние h_CoefHeatContent по городам - БЕЗ ИЗМЕНЕНИЙ
            var city_h_Averages = with_ODPU
                .Where(x => !string.IsNullOrEmpty(x.Rashod_ODPU_GVS_m3.ToString()) || x.Rashod_ODPU_GVS_m3 != 0)
                .GroupBy(x => x.City)
                .ToDictionary(
                    g => g.Key,
                    g => g.Sum(x => x.Rashod_ODPU_GVS_Gcal) / g.Sum(x => x.Rashod_ODPU_GVS_m3)
                );

            // 2. СОЗДАНИЕ РЕЗУЛЬТАТА - ОПТИМИЗАЦИЯ
            var filteredData = allFilteredData.Where(item => !string.IsNullOrEmpty(item.BuildingId)).ToList();
            var result = new List<GVSDataObject>(filteredData.Count); // ← Capacity

            foreach (var item in filteredData) // ← обычный цикл вместо LINQ
            {
                oldDict.TryGetValue(item.BuildingId, out var oldItem);
                MPDict.TryGetValue(item.BuildingId, out var MPItem);

                // Безопасное получение коэффициента по HeatSupplyZone - БЕЗ ИЗМЕНЕНИЙ
                string heatSupplyZone = oldItem?.HeatSupplyZone;

                bool hasMP = MPItem != null;
                string calcType = hasMP ? ByIndications : Normative;
                decimal rashodIndications = hasMP ? MPItem.Q_Calc : 0;

                var resultItem = new GVSDataObject
                {
                    BuildingId = item.BuildingId ?? NoData,
                    HeatSupplyZone = !string.IsNullOrWhiteSpace(heatSupplyZone) ? heatSupplyZone : NoData,
                    Address = item.Address ?? NoData,
                    CalcType = calcType,
                    RashodIndications = rashodIndications,
                    PO_1C_Gcal = item?.PO_1C_Gcal ?? 0,
                    PO_1C_WithOutRecalc_Gcal = item?.PO_1C_WithOutRecalc_Gcal ?? 0,
                    BuildingType = !string.IsNullOrWhiteSpace(oldItem?.BuildingType) ? oldItem.BuildingType : NoData,
                    ZTP = !string.IsNullOrWhiteSpace(oldItem?.ZTP) ? oldItem.ZTP : NoData
                };

                resultItem.City = DataServices.TryGetCityByAddress(resultItem.Address);


                string resultCity = resultItem.City;
                bool hasCity = !string.IsNullOrEmpty(resultCity);
                bool hasCityInAverages = hasCity && city_h_Averages.ContainsKey(resultCity);

                if (hasCityInAverages)
                {
                    decimal cityAverage = city_h_Averages[resultCity];
                    if (resultItem.CalcType == Normative)
                    {
                        resultItem.h_CoeffHeatContent_With_ITP = cityAverage;
                    }
                    else
                    {
                        resultItem.h_CoeffHeatContent_With_ITP = resultItem.RashodIndications / resultItem.m3_By_h_Normative;
                    }
                }

                // 3. ВЫЧИСЛЕНИЕ ПОТЕРЬ - БЕЗ ИЗМЕНЕНИЙ
                if (hasMP)
                {
                    resultItem.LossGVS_with_ITP = rashodIndications - resultItem.PO_1C_Gcal;
                }
                else
                {
                    resultItem.LossGVS_with_ITP = resultItem.PO_1C_Gcal * resultItem.h_CoeffHeatContent_With_ITP / resultItem.h_Normative - resultItem.PO_1C_Gcal;
                }

                result.Add(resultItem);
            }

            return result;
        }

        public static List<GVSDataObject> GetUnion_OldData(this List<GVSDataObject> data,// опт
                                                        List<GVSDataObject> With_ODPU, List<GVSDataObject> WithOut_ODPU,
                                                                                        List<GVSDataObject> With_ITP)
        {
            // Создаем список с достаточной емкостью
            var result = new List<GVSDataObject>(data.Count + With_ODPU.Count + WithOut_ODPU.Count + With_ITP.Count);

            result.AddRange(data);
            result.AddRange(With_ODPU);
            result.AddRange(WithOut_ODPU);
            result.AddRange(With_ITP);

            return result;
        }
    }
}
