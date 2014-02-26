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
        }

        public MapDataClasses.ClientMap getClientRootMap()
        {
            return MapDataClasses.MapDataManager.getClientMap(rootMap);
        }

        public ClientPlayer getClientPlayer()
        {
            ClientPlayer cp = new ClientPlayer() { x = player.rootX, y = player.rootY };
            return cp;
        }
    }
}
