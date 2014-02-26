using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapDataClasses
{
    public class MapDataManager
    {
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
                case "Rest":
                    return "/images/game/rest.png";
                case "Quest":
                    return "/images/game/quest.png";
                case "Wall":
                    return "/images/game/wall.png";
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
                case "Rest":
                    return true;
                case "Quest":
                    return true;
                case "Wall":
                    return false;
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
                case "Rest":
                    return false;
                case "Quest":
                    return false;
                case "Wall":
                    return false;
                default:
                    return false;
            }
        }

        public static List<string> getMapNames()
        {
            List<string> results = new List<string>();
            results.Add("Ensemble Village");
            return results;
        }

        public static MapModel createMap(string name)
        {
            switch (name)
            {
                case "Ensemble Village":
                    return TutorialMapGenerators.EnsembleVillageGenerator.Implementation.getMap();
                default:
                    return new MapModel();
            }
        }

        public static MapInteraction getMapInteraction(string name, int x, int y, MapModel mm)
        {
            switch (name)
            {
                case "Ensemble Village":
                    return TutorialMapGenerators.EnsembleVillageGenerator.Implementation.getInteraction(mm, x, y);
                default:
                    return new MapInteraction();
            }
        }

        public static void interactWithMap(string name, int x, int y, MapModel mm, string selectedOption)
        {
            switch (name)
            {
                case "Ensemble Village":
                    TutorialMapGenerators.EnsembleVillageGenerator.Implementation.performInteraction(mm, x, y, selectedOption);
                    break;
                default:
                    break;
            }
        }
    }
}
