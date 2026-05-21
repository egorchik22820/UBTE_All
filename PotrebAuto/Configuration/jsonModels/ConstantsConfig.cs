using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PotrebAuto.Configuration.jsonModels
{
    public class ConstantsConfig
    {
        public int ConsumersTableRowStart { get; set; }
        public int ConsumersDataRowStart { get; set; }
        public int Consumers_2TableRowStart { get; set; }
        public int Consumers_2DataRowStart { get; set; }


        public int SACTableRowStart { get; set; }
        public int SACDataRowStart { get; set; }


        public int GiTTableRowStart { get; set; }
        public int GiTDataRowStart {  get; set; }


        public int QlickDataRowStart { get; set; }
        public int QlickDataColStart { get; set; }


        public int DatesColStart { get; set; }
        public int DatesRowStart { get; set; }
        public int Dates_2ColStart { get; set; }
        public int Dates_2RowStart { get; set; }

        public int DaysInMonth { get; set; }
        public int DaysInMonth_2 { get; set; }
    }
}
