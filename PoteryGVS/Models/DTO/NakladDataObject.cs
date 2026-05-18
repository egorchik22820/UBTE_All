using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoteryGVS.Models
{
    public class NakladDataObject
    {
        public string DocType { get; set; }
        public string Nomenclature { get; set; }
        public string Tariff { get; set; }
        public string CalcType { get; set; }
        public string NomenclatureUnit { get; set; }
        public string RecalcYear { get; set; }
        public string LoadType { get; set; }
        public string Department { get; set; }
        public string BuildingId { get; set; }
        public string BuildingType { get; set; }
        public string SpaceType { get; set; }
        public string AddressTU { get; set; }
        public string BuildingAddress { get; set; }
        public string HeatSourse { get; set; }
        public decimal QuantityTotal { get; set; }
        public decimal Quantity_Gcal { get; set; }
        public decimal Quantity_m3 { get; set; }
        public decimal Quantity_WithOutRecalc_Gcal { get; set; }
        public decimal Quantity_WithOutRecalc_m3 { get; set; }
        public string BuildinId_BuildingAddress_Join
        {
            get
            {
                if (this.BuildingAddress == null || this.BuildingId == null)
                    return string.Empty;
                else
                    return $"{BuildingId}{BuildingAddress}";
            }
            set { this.BuildingAddress = value ; }
        }

        public override string ToString()
        {
            return $"{BuildingId}, {Department}, {RecalcYear}, {LoadType}, {HeatSourse}";
        }
    }
}
