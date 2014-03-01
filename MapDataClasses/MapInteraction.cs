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
        public List<MapOption> options { get; set; }
    }

    public class MapOption
    {
        public string text;
        public string value;
    }

    public class DungeonSelectInteraction : MapInteraction
    {
        public bool isDungeon { get { return true; } }
        public bool isExit { get; set; }
        public int maxPartySize { get; set; }
    }
}
