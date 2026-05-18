using PoteryGVS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoteryGVS.Configuration;

namespace PoteryGVS.Extensions.FilterExtensions
{
    public static class MPDataFilterExtension
    {
        private static readonly string mpIsOdpu = ConfigModel.mpIsOdpu;
        private static readonly string mpLoadTypeGvs = ConfigModel.mpLoadTypeGvs;
        private static readonly string mpLoadTypeTeplo = ConfigModel.mpLoadTypeTeplo;

        public static List<MPDataObject> GetNeedObjects(this List<MPDataObject> MPData)
        {
            return MPData.GetObjects()
                         .getGroupedData();
        }

        public static List<MPDataObject> GetNeedObjects_ITP(this List<MPDataObject> MPData)
        {
            return MPData.GetObjects_ITP()
                         .getGroupedData();
        }

        public static List<MPDataObject> GetObjects(this List<MPDataObject> MPData)
        {
            return MPData.Where(x => x.IsODPU.ToLowerInvariant().Contains(mpIsOdpu.ToLowerInvariant()) &&
                                !string.IsNullOrWhiteSpace(x.dV_Calc.ToString()) &&
                                x.Q_Calc != 0 &&
                                x.LoadType.ToLowerInvariant().Contains(mpLoadTypeGvs.ToLowerInvariant()))
                                .ToList();
        }

        public static List<MPDataObject> GetObjects_ITP(this List<MPDataObject> MPData)
        {
            return MPData.Where(x => x.IsODPU.ToLowerInvariant().Contains(mpIsOdpu.ToLowerInvariant()) &&
                                !string.IsNullOrWhiteSpace(x.dV_Calc.ToString()) &&
                                x.Q_Calc != 0 &&
                                x.LoadType.ToLowerInvariant().Contains(mpLoadTypeTeplo.ToLowerInvariant()))
                                .ToList();
        }

        public static List<MPDataObject> getGroupedData(this List<MPDataObject> MPData)////////////опт
        {
            return MPData.GroupBy(x => x.BuildingId)
                 .Select(g =>
                 {
                     var first = g.First();
                     var sumQ_Calc = g.Sum(x => x.Q_Calc);
                     var sumdV_Calc = g.Sum(x => x.dV_Calc);
                     var sumVNR_Calc = g.Sum(x => x.VNR_Calc);

                     return new MPDataObject
                     {
                         BuildingId = g.Key,
                         Q_Calc = sumQ_Calc,
                         dV_Calc = sumdV_Calc,
                         VNR_Calc = sumVNR_Calc,
                         TU_AIIS_Id = first.TU_AIIS_Id,
                         SysPU_Id = first.SysPU_Id,
                         Address = first.Address
                     };
                 })
                 .ToList();
        }
    }
}
