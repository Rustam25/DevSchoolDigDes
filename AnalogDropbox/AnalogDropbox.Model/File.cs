using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalogDropbox.Model
{
    public class File
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public User Owner { get; set; }
        public Int64 Size { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime LastWriteTime { get; set; }
    }
}
