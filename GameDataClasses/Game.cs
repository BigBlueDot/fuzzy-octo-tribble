using GameDataClasses.GamePlayerClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDataClasses
{
    public class Game
    {
        //needs to store the player and any active maps
        private PlayerModels.PlayerModel player;
        private MapDataClasses.MapModel rootMap;
        private int x
        {
            get
            {
                return player.rootX;
            }
        }
        private int y
        {
            get
            {
                return player.rootY;
            }
        }

        public Game(PlayerModels.PlayerModel player)
        {
            this.player = player;
            rootMap = MapDataClasses.MapDataManager.createMap(player.rootMap);
            MapDataClasses.MapDataManager.onLoadMap += MapDataManager_onLoadMap;
        }

        void MapDataManager_onLoadMap(object sender, MapDataClasses.MapDataClasses.MapSelectedEventArgs e)
        {
            rootMap = MapDataClasses.MapDataManager.createMap(e.mapName);
            player.rootX = rootMap.startX;
            player.rootY = rootMap.startY;
        }

        public MapDataClasses.ClientMap getClientRootMap()
        {
            return MapDataClasses.MapDataManager.getClientMap(rootMap);
        }

        public ClientPlayer getClientPlayer()
        {
            ClientPlayer cp = new ClientPlayer() { gp = player.gp, x = player.rootX, y = player.rootY, characters = player.characters };

            return cp;
        }

        public MapDataClasses.MapInteraction getInteraction(int x, int y)
        {
            if (Math.Abs(player.rootX - x) + Math.Abs(player.rootY - y) > 1)
            {
                return new MapDataClasses.MapInteraction() { hasDialog = false };
            }

            return MapDataClasses.MapDataManager.getMapInteraction(x, y, rootMap);
        }

        public void setOptionInteraction(int x, int y, string option)
        {
            MapDataClasses.MapDataManager.interactWithMap(rootMap.name, x, y, rootMap, option);
        }

        public void moveLeft()
        {
            if (player.rootX == 0 || !MapDataClasses.MapDataManager.getTraversable(rootMap.map[player.rootX - 1, player.rootY]))
            {
                return;
            }
            player.rootX -= 1;
        }

        public void moveUp()
        {
            if (player.rootY == 0 || !MapDataClasses.MapDataManager.getTraversable(rootMap.map[player.rootX, player.rootY - 1]))
            {
                return;
            }
            player.rootY -= 1;
        }

        public void moveRight()
        {
            if (player.rootX == rootMap.map.GetLength(0) - 1 || !MapDataClasses.MapDataManager.getTraversable(rootMap.map[player.rootX + 1, player.rootY]))
            {
                return;
            }
            player.rootX += 1;
        }

        public void moveDown()
        {
            if (player.rootY == rootMap.map.GetLength(1) || !MapDataClasses.MapDataManager.getTraversable(rootMap.map[player.rootX, player.rootY + 1]))
            {
                return;
            }
            player.rootY += 1;
        }
    }
}
