namespace PoteryGVS.Models
{
    public class GVSDataObject
    {
        public string Base { get; set; } = "OREN";
        public string BuildingId { get; set; }
        public string TU_AIIS_Id { get; set; }
        public string SysPU_Id { get; set; }
        public string City { get; set; }
        public string HeatSupplyZone { get; set; }
        public string Address { get; set; }
        public string ZTP { get; set; }
        public string BuildingType { get; set; }
        public string CalcType { get; set; }

        public decimal Rashod_ODPU_GVS_Gcal { get; set; }
        public decimal Rashod_ODPU_GVS_m3 { get; set; }
        public decimal Rashod_ODPU_GVS_Gcal_2 { get; set; }
        public decimal Rashod_ODPU_GVS_m3_2 { get; set; }
        public decimal Rashod_GVS_Gcal_2 { get; set; }
        public decimal RashodIndications { get; set; }

        public decimal h_Normative { get; set; } = (decimal)0.051;

        public decimal h_CoeffHeatContent_With_ODPU
        {
            get
            {
                return Rashod_ODPU_GVS_m3 != 0 ? Rashod_ODPU_GVS_Gcal / Rashod_ODPU_GVS_m3 : 0;
            }
        }

        public decimal h_CoeffHeatContent_WithOut_ODPU { get; set; }
        public decimal h_CoeffHeatContent_With_ITP { get; set; }

        public decimal h_Normative_m3 { get; set; }

        public decimal PO_1C_Gcal { get; set; }
        public decimal PO_1C_m3 { get; set; }
        public decimal PO_1C_WithOutRecalc_Gcal { get; set; }
        public decimal PO_1C_WithOutRecalc_m3 { get; set; }

        public decimal LossGVS_Without_ODPU { get; set; }

        public decimal LossGVS_With_ODPU
        {
            get
            {
                return Rashod_ODPU_GVS_Gcal - Rashod_ODPU_GVS_m3 * h_Normative;
            }
        }

        public decimal LossGVS_WithOut_ODPU_Mismatch { get; set; }
        public decimal LossGVS_with_ITP { get; set; }
        public decimal LossGVS_With_ITP_2 { get; set; }

        public decimal FormulaValue
        {
            get
            {
                return PO_1C_m3 * h_CoeffHeatContent_With_ODPU - PO_1C_Gcal;
            }
        }

        public decimal m3_By_h_Normative
        {
            get
            {
                return PO_1C_Gcal / h_Normative;
            }
        }

        public decimal NegativeODN_Gcal { get; set; }
        public decimal NegativeODN_m3 { get; set; }

        public decimal ODN_LossGVS_with_ODPU { get; set; }

        public override string ToString()
        {
            return $"{BuildingId}, {City}, {Rashod_ODPU_GVS_Gcal}";
        }
    }
}