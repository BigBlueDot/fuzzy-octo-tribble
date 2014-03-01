using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapDataClasses.TutorialMapGenerators
{
    public class EnsembleVillageGenerator : IVillageGenerator
    {
        private static EnsembleVillageGenerator _implementation;
        public static EnsembleVillageGenerator Implementation
        {
            get
            {
                if (_implementation == null)
                {
                    _implementation = new EnsembleVillageGenerator();
                }

                return _implementation;
            }
        }

        public event MapDataManager.LoadMapEventHandler onSelectMap;

        public MapModel getMap()
        {
            MapModel mm = new MapModel();
            mm.name = "Ensemble Village";
            mm.map = new string[10, 10];
            mm.startX = 5;
            mm.startY = 5;
            for (var x = 0; x < 10; x++)
            {
                for (var y = 0; y < 10; y++)
                {
                    mm.map[x, y] = "Empty";
                }
                mm.map[x, 0] = "Wall";
                mm.map[x, 9] = "Wall";
            }

            for (var y = 0; y < 10; y++)
            {
                mm.map[0, y] = "Wall";
                mm.map[9, y] = "Wall";
            }

            mm.map[2, 2] = "Rest";
            mm.map[2, 4] = "Quest";
            mm.map[4, 2] = "DungeonMaster";

            return mm;
        }

        public MapInteraction getInteraction(MapModel mm, int x, int y)
        {
            MapInteraction mi = new MapInteraction();
            if(mm.map[x,y] == "Rest")
            {
                mi.hasDialog = true;
                mi.dialog = "Would you like to rest?";
                mi.options = new List<string>();
                mi.options.Add("Yes");
                mi.options.Add("No");
            }
            else if(mm.map[x,y] == "Quest")
            {
                mi.hasDialog = true;
                mi.dialog = "What quest would you like to do?";
                mi.options = new List<string>();
                mi.options.Add("NEED TO FILL THIS IN");
                mi.options.Add("NEED TO FILL THIS IN 2");
            }
            else if (mm.map[x, y] == "DungeonMaster")
            {
                mi = new DungeonSelectInteraction();
                mi.hasDialog = true;
                mi.hasOptions = true;
                mi.dialog = "What dungeon would you like to go to?";
                mi.options = new List<string>();
                mi.options.Add("Emergence Cavern");
                ((DungeonSelectInteraction)mi).maxPartySize = 1;
            }

            return mi;
        }

        public void performInteraction(MapModel mm, int x, int y, string selectedOption)
        {
            if (mm.map[x, y] == "DungeonMaster")
            {
                if (selectedOption == "Emergence Cavern")
                {
                    onSelectMap(this, new MapDataClasses.MapSelectedEventArgs() { mapName="Emergence Cavern" });
                }
            }
        }
    }
}
