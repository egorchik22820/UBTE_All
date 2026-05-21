using PotrebAuto.Models.DTO;
using PotrebAuto.Servises;
using System;
using System.Collections.Generic;
using PotrebAuto.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PotrebAuto.Extensions;

namespace PotrebAuto.Models
{
    public class ConsumersDataObject
    {
        private static readonly int maxDays = ConfigModel.DaysInMonth_MAX;
        public static List<CellDTO> DateList = new List<CellDTO>(maxDays);
        public static List<CellDTO> DateListTemp = new List<CellDTO>(maxDays);
        public static List<CellDTO> DateList_2 = new List<CellDTO>(maxDays);

        public ConsumersDataObject()
        {
            DaysValue = new List<CellDTO>(maxDays);
            DaysValue_2 = new List<CellDTO>(maxDays);
        }


        public CellDTO Number {  get; set; }
        public CellDTO Address { get; set; }
        public CellDTO TU_AIIS // опт
        {
            get
            {
                return Address != null ? new CellDTO { Value = Address.Hyperlink.TryGetIdFromHyperlink() }
                                            : new CellDTO { Value = "Нет данных" };
            }
        } // added
        public CellDTO ObjectId { get; set; } // added
        public CellDTO PO_AIIS_Total
        {
            get
            {
                return new CellDTO { Value = DataServices.AddDecimals(
                        PU_GcalTotal.Value,
                        ZM_GcalTotal.Value)};
            }
        }// added
        public CellDTO ColorDaysCount
        {
            get
            {
                return new CellDTO { Value = DaysValue?.GetColorDaysCount() };
            }
        }// added

        // for new
        public CellDTO City { get; set; }
        public CellDTO CityGiT { get; set; }
        public CellDTO BuildingType { get; set; }
        public CellDTO BuildingId { get; set; }

        public CellDTO PO_AIIS_Total_2 { get; set; }
        public CellDTO ColorDaysCount_2 { get; set; }

        public CellDTO PU_GcalTotal_2 { get; set; }
        public CellDTO ZM_GcalTotal_2 { get; set; }

        public List<CellDTO> DaysValue_2 { get; set; }



        public CellDTO Id { get; set; }
        public CellDTO PU_GcalTotal { get; set; }
        public CellDTO PU_WithVNR_Gcal { get; set; }
        public CellDTO ZM_GcalTotal { get; set; }
        public CellDTO ZM_WithAverage_Gcal { get; set; }
        public CellDTO WithBS_Gcal { get; set; }
        public CellDTO WithRealLoadBS_Gcal { get; set; }
        public CellDTO WithRealLoadTU_Gcal { get; set; }
        public CellDTO WithNP_Gcal { get; set; }
        public CellDTO WithCab_Gcal { get; set; }
        public CellDTO WithKSN_Gcal { get; set; }
        public CellDTO WithRealLoad_FN_Gcal { get; set; }
        public CellDTO WithDocAndKSN_Gcal { get; set; }
        public CellDTO WithDoc_Gcal { get; set; }
        public CellDTO CorrectNotesCount {  get; set; }
        public CellDTO Q_T_M_IsNull {  get; set; }
        public CellDTO ActsBS { get; set; }
        public CellDTO Max_Min_Q_M { get; set; }
        public CellDTO IsValid_VNR { get; set; }
        public CellDTO IsValid_M1_M2 { get; set; }
        public CellDTO IsValid_M1_M2_2 { get; set; }
        public CellDTO Q_eng {  get; set; }
        public CellDTO IsValid_T {  get; set; }
        public List<CellDTO> DaysValue { get; set; }

    }
}
