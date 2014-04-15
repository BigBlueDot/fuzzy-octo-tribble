using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapDataClasses.MapDataClasses
{
    public class Encounter
    {
        public string message { get; set; }
        public List<Enemy> enemies { get; set; }
    }
}
