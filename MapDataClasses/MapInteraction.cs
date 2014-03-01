using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapDataClasses
{
    public class MapInteraction
    {
        public bool hasDialog { get; set; }
        public string dialog { get; set; }
        public bool hasOptions { get; set; }
        public List<string> options { get; set; }
    }

    public class DungeonSelectInteraction : MapInteraction
    {
        public bool isDungeon { get { return true; } }
        public int maxPartySize { get; set; }
    }
}
