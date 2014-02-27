using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapDataClasses.TutorialMapGenerators
{
    public class EmergenceCavernGenerator : IVillageGenerator
    {
        private static EmergenceCavernGenerator _implementation;
        public static EmergenceCavernGenerator Implementation
        {
            get
            {
                if (_implementation == null)
                {
                    _implementation = new EmergenceCavernGenerator();
                }

                return _implementation;
            }
        }

        public event MapDataManager.LoadMapEventHandler onSelectMap;

        public MapModel getMap()
        {
            MapModel mm = new MapModel();
            mm.name = "Emergence Cavern";
            mm.map = new string[50, 40];
            mm.startX = 26;
            mm.startY = 37;
            for (var x = 0; x < 50; x++)
            {
                for (var y = 0; y < 40; y++)
                {
                    mm.map[x, y] = "Empty";
                }
                mm.map[x, 0] = "Wall";
                mm.map[x, 39] = "Wall";
            }

            for (var y = 0; y < 40; y++)
            {
                mm.map[0, y] = "Wall";
                mm.map[49, y] = "Wall";
            }

            //Draw walls
            for (var x = 0; x < 25; x++)
            {
                mm.map[x, 34] = "Wall";
            }

            for (var x = 28; x < 44; x++)
            {
                mm.map[x, 34] = "Wall";
            }

            for (var y = 34; y >= 25; y--)
            {
                mm.map[24, y] = "Wall";
                mm.map[28, y] = "Wall";
                if (y % 2 == 0)
                {
                    mm.map[23, y] = "Wall";
                    mm.map[29, y] = "Wall";
                }
            }

            for (var x = 24; x < 28; x++)
            {
                mm.map[x, 25] = "Wall";
            }

            for (var y = 34; y >= 10; y--)
            {
                mm.map[42, y] = "Wall";
                mm.map[47, y] = "Wall";
                if (y % 2 == 0)
                {
                    mm.map[41, y] = "Wall";
                    mm.map[48, y] = "Wall";
                }
            }
            for (var x = 42; x < 47; x++)
            {
                mm.map[x, 10] = "Wall";
            }

            mm.map[26, 38] = "Exit";

            return mm;
        }

        public MapInteraction getInteraction(MapModel mm, int x, int y)
        {
            //No interactions at the moment
            MapInteraction mi = new MapInteraction();
            if (mm.map[x, y] == "Exit")
            {
                mi.hasDialog = true;
                mi.hasOptions = true;
                mi.dialog = "Would you like to leave this dungeon?";
                mi.options = new List<string>();
                mi.options.Add("Yes");
                mi.options.Add("No");
            }
            return mi;
        }

        public void performInteraction(MapModel mm, int x, int y, string selectedOption)
        {
            //Do logic for determining what to do
            //This may need to be event driven (i.e. fire onRemoveGold and onRestoreHealth and onRestoreMP
            if (mm.map[x, y] == "Exit")
            {
                if (selectedOption == "Yes")
                {
                    onSelectMap(this, new MapDataClasses.MapSelectedEventArgs() { mapName = "Ensemble Village" });
                }
            }
        }
    }
}
