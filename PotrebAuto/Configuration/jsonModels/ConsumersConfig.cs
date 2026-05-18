using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PotrebAuto.Configuration.jsonModels
{
    public class ConsumersConfig
    {
        public int Number { get; set; }
        public int Address { get; set; }
        public int Id { get; set; }
        public int PU_GcalTotal { get; set; }
        public int PU_WithVNR_Gcal { get; set; }
        public int ZM_GcalTotal { get; set; }
        public int ZM_WithAverage_Gcal { get; set; }
        public int WithBS_Gcal { get; set; }
        public int WithRealLoadBS_Gcal { get; set; }
        public int WithRealLoadTU_Gcal { get; set; }
        public int WithNP_Gcal { get; set; }
        public int WithCab_Gcal { get; set; }
        public int WithKSN_Gcal { get; set; }
        public int WithRealLoad_FN_Gcal { get; set; }
        public int WithDocAndKSN_Gcal { get; set; }
        public int WithDoc_Gcal { get; set; }
        public int CorrectNotesCount { get; set; }
        public int Q_T_M_IsNull { get; set; }
        public int ActsBS { get; set; }
        public int Max_Min_Q_M { get; set; }
        public int IsValid_VNR { get; set; }
        public int IsValid_M1_M2 { get; set; }
        public int IsValid_M1_M2_2 { get; set; }
        public int Q_eng { get; set; }
        public int IsValid_T { get; set; }
        public int DaysValue { get; set; }
    }
}
