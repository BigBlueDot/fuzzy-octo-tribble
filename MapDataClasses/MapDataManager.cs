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
            cm.mapSquares = new ClientMapSquare[mm.map.GetLength(0), mm.map.GetLength(1)];
            cm.name = mm.name;
            for (var x = 0; x < mm.map.GetLength(0); x++)
            {
                for (var y = 0; y < mm.map.GetLength(1); y++)
                {
                    switch (mm.map[x, y])
                    {
                        case "Empty":
                            cm.mapSquares[x, y] = new ClientMapSquare() { imageUrl = "/images/empty.png", isInteractable = false, isTraversable = true };
                            break;
                        case "Rest":
                            cm.mapSquares[x, y] = new ClientMapSquare() { imageUrl = "/images/rest.png", isInteractable = true, isTraversable = false };
                            break;
                        case "Quest":
                            cm.mapSquares[x, y] = new ClientMapSquare() { imageUrl = "/images/quest.png", isInteractable = true, isTraversable = false };
                            break;
                        case "Wall":
                            cm.mapSquares[x, y] = new ClientMapSquare() { imageUrl = "/images/wall.png", isInteractable = false, isTraversable = false };
                            break;
                        default:
                            cm.mapSquares[x, y] = new ClientMapSquare() { imageUrl = "/images/empty.png", isInteractable = false, isTraversable = true };
                            break;
                    }
                }
            }

            return cm;
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
