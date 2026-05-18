using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Form4._42Auto.Models
{
    public class ExportDataTable//vagina
    {
        public string CounterAgent { get; set; }
        public string CounterAgentExpenseType { get; set; }
        public string IsVGO { get; set; }

        public string Classifier_9_1 { get; set; }
        public string Classifier_9_2 { get; set; }
        public string Classifier_9_5 { get; set; }

        public decimal Quantity_GCal { get; set; }
        public decimal Quantity_m3 { get; set; }
        public decimal Amount_rub {  get; set; }

        public decimal PaidInPeriod_rub { get; set; }
        public decimal CorrectionDZ { get; set; }
        public decimal TransferDZ { get; set; }

        public decimal ValueOfDZ { get; set; }

        public decimal KZ_InEndPeriod { get; set; }
        public decimal TotalDZ { get; set; }
        public decimal CurrentDZ { get; set; }

        public decimal OffsKZ { get; set; }
        public decimal SaldoDZ { get; set; }

        public override string ToString()
        {
            return $"9.1:\t{Classifier_9_1}\tvgo:\t{IsVGO}\tCounterAgentExpenseType:\t{CounterAgentExpenseType}";
        }
    }
}
