using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UBTE_Auto.AppData.Configuration
{
    public class ProgramConfig
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }          // опционально, если не указана - будет взята из файла
        public string ExecutablePath { get; set; }
        public string WorkingDirectory { get; set; }
        public string Arguments { get; set; }
    }
}
