using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UBTE_Auto.AppData.DTO
{
    public class ProgramDTO
    {
        public ProgramDTO(string name, string description, string version)
        {
            Name = name;
            Description = description;
            Version = version;
        }

        public ProgramDTO()
        {

        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
        public string ExecutablePath { get; set; }   // путь к .exe
        public string WorkingDirectory { get; set; } // рабочая папка (опционально)
        public string Arguments { get; set; }
    }
}
