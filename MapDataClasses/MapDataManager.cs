using MapDataClasses.EventClasses;
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
        private static Func<List<string>> getCharacterNames;
        private static Func<List<string>> getClasses;

        static MapDataManager()
        {

        }

        public static void setPlayerInformation(Func<List<string>> getCharacterNames, Func<List<string>> getClasses)
        {
            MapDataManager.getCharacterNames = getCharacterNames;
            MapDataManager.getClasses = getClasses;
            TutorialMapGenerators.EnsembleVillageGenerator.Implementation.setFunctions(getCharacterNames, getClasses);
        }

        public static ClientMap getClientMap(MapModel mm)
        {
            ClientMap cm = new ClientMap();
            cm.mapSquares = new ClientMapSquare[mm.map.GetLength(0)][];
            List<string> urls = new List<string>();
            for (var i = 0; i < cm.mapSquares.Length; i++)
            {
                cm.mapSquares[i] = new ClientMapSquare[mm.map.GetLength(1)];
            }
            cm.name = mm.name;
            for (var x = 0; x < mm.map.GetLength(0); x++)
            {
                for (var y = 0; y < mm.map.GetLength(1); y++)
                {
                    int imageUrlId;
                    string imageUrl = getImageURL(mm.map[x, y]);
                    if (urls.Contains(imageUrl))
                    {
                        imageUrlId = urls.IndexOf(imageUrl);
                    }
                    else
                    {
                        imageUrlId = urls.Count;
                        urls.Add(imageUrl);
                    }
                    cm.mapSquares[x][y] = new ClientMapSquare()
                    {
                        imageUrl = imageUrlId,
                        isInteractable = getInteractable(mm.map[x, y]),
                        isTraversable = getTraversable(mm.map[x, y])
                    };
                }
            }

            cm.mapUrl = urls.ToArray();

            cm.events = new List<ClientEvent>();
            foreach (MapEventModel me in mm.eventCollection.getAll())
            {
                ClientEvent ce = new ClientEvent();
                ce.x = me.x;
                ce.y = me.y;
                ce.rewardType = me.rewardType;
                cm.events.Add(ce);
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
                case "ClassTrainer":
                    return "/images/game/classtrainer.png";
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
                case "ClassTrainer":
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
                case "ClassTrainer":
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
            results.Add("Emergence Cavern F2");
            return results;
        }

        public static string getHubMap(string name)
        {
            switch (name)
            {
                case "Ensemble Village":
                    return "Ensemble Village";
                case "Emergence Cavern":
                    return "Ensemble Village";
                case "Emergence Cavern F2":
                    return "Ensemble Village";
                default:
                    return "Ensemble Village";
            }
        }

        public static bool isCombatMap(string name)
        {
            switch (name)
            {
                case "Ensemble Village":
                    return false;
                case "Emergence Cavern":
                    return true;
                case "Emergence Cavern F2":
                    return true;
                default:
                    return false;
            }
        }

        public static MapModel createMap(string name)
        {
            switch (name)
            {
                case "Ensemble Village":
                    return TutorialMapGenerators.EnsembleVillageGenerator.Implementation.getMap();
                case "Emergence Cavern":
                    return TutorialMapGenerators.EmergenceCavernGenerator.Implementation.getMap();
                case "Emergence Cavern F2":
                    return TutorialMapGenerators.EmergenceCavernGenerator.Implementation.getMapFloor2();
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
                case "Emergence Cavern F2":
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
                case "Emergence Cavern F2":
                    return TutorialMapGenerators.EmergenceCavernGenerator.Implementation.validateDungeonSelection(mm, x, y, selectedDungeon);
                default:
                    return false;
            }
        }

        public static bool validateClassChangeSelection(string name, int x, int y, MapModel mm)
        {
            switch (name)
            {
                case "Ensemble Village":
                    return TutorialMapGenerators.EnsembleVillageGenerator.Implementation.validateClassTrainer(mm, x, y);
                case "Emergence Cavern":
                    return false;
                case "Emergence Cavern F2":
                    return false;
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
                case "Emergence Cavern F2":
                    return TutorialMapGenerators.EmergenceCavernGenerator.Implementation.getRandomEncounterCountF2();
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
                case "Emergence Cavern F2":
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
                case "Emergence Cavern F2":
                    return TutorialMapGenerators.EmergenceCavernGenerator.Implementation.getRandomEncounterCountF2();
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
                case "Emergence Cavern F2":
                    return TutorialMapGenerators.EmergenceCavernGenerator.Implementation.getRandomEncounterF2(selection);
                default:
                    return new Encounter();
            }
        }

        public static Encounter getEncounter(MapModel mm, int x, int y, int selection)
        {
            if(isEvent(mm, x, y))
            {
                MapEventModel me = getEvent(mm, x, y);
                if (me.eventData.type == EventClasses.EventDataType.Combat)
                {
                    return ((CombatEventDataModel)me.eventData).encounter;
                }
                else
                {
                    return getRandomEncounter(mm.name, selection);
                }
            }
            else{
                return getRandomEncounter(mm.name, selection);
            }
        }


        public static Enemy getEnemy(string mapName, string enemyType)
        {
            switch (mapName)
            {
                case "Emergence Cavern":
                    return TutorialMapGenerators.EmergenceCavernGenerator.Implementation.getEnemy(enemyType);
                case "Emergence Cavern F2":
                    return TutorialMapGenerators.EmergenceCavernGenerator.Implementation.getEnemy(enemyType);
                default:
                    return new Enemy();
            }
        }

        public static bool isEvent(MapModel mm, int x, int y)
        {
            foreach (MapEventModel me in mm.eventCollection.getAll())
            {
                if (me.x == x && me.y == y)
                {
                    return true;
                }
            }

            return false;
        }

        public static MapEventModel getEvent(MapModel mm, int x, int y)
        {

            foreach (MapEventModel me in mm.eventCollection.getAll())
            {
                if (me.x == x && me.y == y)
                {
                    return me;
                }
            }

            return new MapEventModel();
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
