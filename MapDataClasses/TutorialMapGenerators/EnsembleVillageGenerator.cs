using MapDataClasses.MapDataClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapDataClasses.TutorialMapGenerators
{
    public class EnsembleVillageGenerator : IVillageGenerator
    {
        private Func<List<string>> getCharacters;
        private Func<List<string>> getClasses;

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

        public void setFunctions(Func<List<string>> getCharacters, Func<List<string>> getClasses)
        {
            this.getCharacters = getCharacters;
            this.getClasses = getClasses;
        }

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

            mm.map[6, 6] = "ClassTrainer";

            return mm;
        }

        public MapInteraction getInteraction(MapModel mm, int x, int y)
        {
            MapInteraction mi = new MapInteraction();
            if(mm.map[x,y] == "Rest")
            {
                mi.hasDialog = true;
                mi.dialog = "Would you like to rest?";
            }
            else if(mm.map[x,y] == "Quest")
            {
                mi.hasDialog = true;
                mi.dialog = "What quest would you like to do?";
            }
            else if (mm.map[x, y] == "DungeonMaster")
            {
                mi = new DungeonSelectInteraction();
                mi.hasDialog = true;
                mi.hasOptions = true;
                mi.dialog = "What dungeon would you like to go to?";
                mi.options = new List<MapOption>();
                mi.options.Add(new MapOption() { text = "Emergence Cavern", value = "Emergence Cavern" });
                mi.options.Add(new MapOption() { text = "Emergence Cavern F2", value = "Emergence Cavern F2" });
                ((DungeonSelectInteraction)mi).maxPartySize = 2;
            }
            else if (mm.map[x, y] == "ClassTrainer")
            {
                mi = new ClassTrainerInteraction();
                mi.hasDialog = true;
                mi.hasOptions = true;
                mi.dialog = "Please select the characters you want to change classes for.";
                mi.options = new List<MapOption>();
                List<string> characters = getCharacters();
                foreach (string c in characters)
                {
                    mi.options.Add(new MapOption() { text = c, value = c });
                }
                List<string> classes = getClasses();
                foreach (string c in classes)
                {
                    ((ClassTrainerInteraction)mi).classes.Add(new MapOption() { text = c, value = c });
                }
            }


            return mi;
        }

        public bool validateDungeonSelection(MapModel mm, int x, int y, string selectedDungeon)
        {
            if (mm.map[x, y] == "DungeonMaster")
            {
                if (selectedDungeon == "Emergence Cavern")
                {
                    return true;
                }
                if (selectedDungeon == "Emergence Cavern F2")
                {
                    return true;
                }
            }

            return false;
        }

        public bool validateClassTrainer(MapModel mm, int x, int y)
        {
            if (mm.map[x, y] == "ClassTrainer")
            {
                return true;
            }

            return false;
        }

        public int getMinCombatCount()
        {
            return 0;
        }

        public int getMaxCombatCount()
        {
            return 0;
        }

        public int getRandomEncounterCount()
        {
            return 0;
        }

        public Encounter getRandomEncounter(int selection)
        {
            return new Encounter();
        }
    }
}
