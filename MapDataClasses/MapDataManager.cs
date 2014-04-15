using MapDataClasses.MapDataClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapDataClasses
{
    public class MapDataManager
    {
        static MapDataManager()
        {

        }

        public static ClientMap getClientMap(MapModel mm)
        {
            ClientMap cm = new ClientMap();
            cm.mapSquares = new ClientMapSquare[mm.map.GetLength(0)][];
            for (var i = 0; i < cm.mapSquares.Length; i++)
            {
                cm.mapSquares[i] = new ClientMapSquare[mm.map.GetLength(1)];
            }
            cm.name = mm.name;
            for (var x = 0; x < mm.map.GetLength(0); x++)
            {
                for (var y = 0; y < mm.map.GetLength(1); y++)
                {
                    cm.mapSquares[x][y] = new ClientMapSquare() { 
                        imageUrl = getImageURL(mm.map[x, y]), 
                        isInteractable = getInteractable(mm.map[x, y]), 
                        isTraversable = getTraversable(mm.map[x, y]) 
                    };
                }
            }

            return cm;
        }

        public static string getImageURL(string mapSquare)
        {
            switch (mapSquare)
            {
                case "Empty":
                    return "/images/game/empty.png";
                case "CaveEmpty":
                    return "/images/game/CaveEmpty.png";
                case "Rest":
                    return "/images/game/rest.png";
                case "Quest":
                    return "/images/game/quest.png";
                case "Wall":
                    return "/images/game/wall.png";
                case "CaveWall":
                    return "/images/game/cavewall.png";
                case "DungeonMaster":
                    return "/images/game/dungeon.png";
                case "Exit":
                    return "/images/game/dungeonexit.png";
                default:
                    return "/images/game/empty.png";
            }
        }

        public static bool getInteractable(string mapSquare)
        {
            switch (mapSquare)
            {
                case "Empty":
                    return false;
                case "CaveEmpty":
                    return false;
                case "Rest":
                    return true;
                case "Quest":
                    return true;
                case "Wall":
                    return false;
                case "CaveWall":
                    return false;
                case "DungeonMaster":
                    return true;
                case "Exit":
                    return true;
                default:
                    return false;
            }
        }

        public static bool getTraversable(string mapSquare)
        {
            switch (mapSquare)
            {
                case "Empty":
                    return true;
                case "CaveEmpty":
                    return true;
                case "Rest":
                    return false;
                case "Quest":
                    return false;
                case "Wall":
                    return false;
                case "CaveWall":
                    return false;
                case "DungeonMaster":
                    return false;
                case "Exit":
                    return false;
                default:
                    return false;
            }
        }

        public static List<string> getMapNames()
        {
            List<string> results = new List<string>();
            results.Add("Ensemble Village");
            results.Add("Emergence Cavern");
            return results;
        }

        public static MapModel createMap(string name)
        {
            switch (name)
            {
                case "Ensemble Village":
                    return TutorialMapGenerators.EnsembleVillageGenerator.Implementation.getMap();
                case "Emergence Cavern":
                    return TutorialMapGenerators.EmergenceCavernGenerator.Implementation.getMap();
                default:
                    return new MapModel();
            }
        }

        public static MapInteraction getMapInteraction(int x, int y, MapModel mm)
        {
            switch (mm.name)
            {
                case "Ensemble Village":
                    return TutorialMapGenerators.EnsembleVillageGenerator.Implementation.getInteraction(mm, x, y);
                case "Emergence Cavern":
                    return TutorialMapGenerators.EmergenceCavernGenerator.Implementation.getInteraction(mm, x, y);
                default:
                    return new MapInteraction();
            }
        }

        public static bool validateDungeonSelection(string name, int x, int y, MapModel mm, string selectedDungeon)
        {
            switch (name)
            {
                case "Ensemble Village":
                    return TutorialMapGenerators.EnsembleVillageGenerator.Implementation.validateDungeonSelection(mm, x, y, selectedDungeon);
                case "Emergence Cavern":
                    return TutorialMapGenerators.EmergenceCavernGenerator.Implementation.validateDungeonSelection(mm, x, y, selectedDungeon);
                default:
                    return false;
            }
        }

        public static int getMinCombatCount(string name)
        {
            switch (name)
            {
                case "Ensemble Village":
                    return TutorialMapGenerators.EnsembleVillageGenerator.Implementation.getMinCombatCount();
                case "Emergence Cavern":
                    return TutorialMapGenerators.EmergenceCavernGenerator.Implementation.getRandomEncounterCount();
                default:
                    return 0;
            }
        }

        public static int getMaxCombatCount(string name)
        {
            switch (name)
            {
                case "Ensemble Village":
                    return TutorialMapGenerators.EnsembleVillageGenerator.Implementation.getMaxCombatCount();
                case "Emergence Cavern":
                    return TutorialMapGenerators.EmergenceCavernGenerator.Implementation.getMaxCombatCount();
                default:
                    return 0;
            }
        }

        public static int getRandomEncounterCount(string name)
        {
            switch (name)
            {
                case "Ensemble Village":
                    return TutorialMapGenerators.EnsembleVillageGenerator.Implementation.getRandomEncounterCount();
                case "Emergence Cavern":
                    return TutorialMapGenerators.EmergenceCavernGenerator.Implementation.getRandomEncounterCount();
                default:
                    return 0;
            }
        }

        public static Encounter getRandomEncounter(string name, int selection)
        {
            switch (name)
            {
                case "Ensemble Village":
                    return TutorialMapGenerators.EnsembleVillageGenerator.Implementation.getRandomEncounter(selection);
                case "Emergence Cavern":
                    return TutorialMapGenerators.EmergenceCavernGenerator.Implementation.getRandomEncounter(selection);
                default:
                    return new Encounter();
            }
        }

        public static void interactWithMap(string name, int x, int y, MapModel mm, string selectedOption)
        {
            //switch (name)
            //{
            //    //case "Ensemble Village":
            //    //    TutorialMapGenerators.EnsembleVillageGenerator.Implementation.performInteraction(mm, x, y, selectedOption);
            //    //    break;
            //    //case "Emergence Cavern":
            //    //    TutorialMapGenerators.EmergenceCavernGenerator.Implementation.performInteraction(mm, x, y, selectedOption);
            //    //    break;
            //    //default:
            //    //    break;
            //}
        }
    }
}
