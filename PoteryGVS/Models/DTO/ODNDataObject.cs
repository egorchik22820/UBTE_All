using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoteryGVS.Models
{
    public class ODNDataObject
    {
        public string BuildingId { get; set; }
        public decimal NegativeODN_Gcal { get; set; }
        public decimal NegativeODN_m3 { get; set; }

        public override string ToString()
        {
            return $"{BuildingId}, {NegativeODN_Gcal}, {NegativeODN_m3}";
        }
    }
}
