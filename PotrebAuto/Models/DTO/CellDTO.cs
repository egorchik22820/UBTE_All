using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace PotrebAuto.Models.DTO
{
    public class CellDTO
    {
        public CellDTO() { }

        public object Value { get; set; }

        public Uri Hyperlink { get; set; }
        public string BackGroundColorRgb { get; set; }
    }
}
