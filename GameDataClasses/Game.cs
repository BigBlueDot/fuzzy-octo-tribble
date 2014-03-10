using fuzzy_octo_tribble.Models;
using GameDataClasses.GamePlayerClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace GameDataClasses
{
    public class Game
    {
        //needs to store the player and any active maps
        public PlayerModels.PlayerModel player { get; set; }
        private MapDataClasses.MapModel currentMap;
        private string userName;
        private UserProfile user;
        private UsersContext db;

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

        public Game(string userName, UsersContext db)
        {
            this.user = db.UserProfiles.Include(up => up.player)
                    .Include(up => up.player.characters)
                    .Include(up => up.player.characters.Select(c => c.characterClasses))
                    .Include(up => up.player.characters.Select(c => c.stats))
                    .Include(up => up.player.parties)
                    .Include(up => up.player.parties.Select(c => c.characters))
                .FirstOrDefault(u => u.UserName.ToLower() == userName);
            this.player = user.player;
            currentMap = MapDataClasses.MapDataManager.createMap(player.rootMap);
            this.userName = userName;
            this.db = db;
        }

        public bool isInDungeon()
        {
            return this.player.activeParty != 0;
        }

        public MapDataClasses.ClientMap getClientRootMap()
        {
            MapDataClasses.ClientMap map = MapDataClasses.MapDataManager.getClientMap(currentMap);
            map.isDungeon = isInDungeon();
            return map;
        }

        public ClientPlayer getClientPlayer()
        {
            List<int> currentPartyCharacters = new List<int>();
            foreach (PlayerModels.Models.PartyModel pm in player.parties)
            {
                if (pm.uniq == player.activeParty)
                {
                    foreach (PlayerModels.Models.PartyCharacterModel pcm in pm.characters)
                    {
                        currentPartyCharacters.Add(pcm.characterUniq);
                    }
                }
            }
            ClientPlayer cp = new ClientPlayer() { gp = player.gp, x = player.rootX, y = player.rootY, characters = player.characters, currentPartyCharacters = currentPartyCharacters };

            return cp;
        }

        public MapDataClasses.MapInteraction getInteraction(int x, int y)
        {
            if (Math.Abs(player.rootX - x) + Math.Abs(player.rootY - y) > 1)
            {
                return new MapDataClasses.MapInteraction() { hasDialog = false };
            }

            return MapDataClasses.MapDataManager.getMapInteraction(x, y, currentMap);
        }

        public void loadDungeon(int x, int y, string dungeonName, string[] party)
        {
            //Verify that the dungeon selection is legitimate
            if (MapDataClasses.MapDataManager.validateDungeonSelection(currentMap.name, x, y, currentMap, dungeonName))
            {
                if (isInDungeon())
                {
                    //Need to disband the party that is currently being used
                    currentMap = MapDataClasses.MapDataManager.createMap(dungeonName);
                    player.rootX = currentMap.startX;
                    player.rootY = currentMap.startY;
                    player.rootMap = dungeonName;

                    var currentParty = new PlayerModels.Models.PartyModel();
                    foreach (PlayerModels.Models.PartyModel pm in player.parties)
                    {
                        if (pm.uniq == player.activeParty)
                        {
                            currentParty = pm;
                        }
                    }

                    if (player.parties.Contains(currentParty))
                    {
                        player.parties.Remove(currentParty);
                    }

                    player.activeParty = 0;

                    db.SaveChanges();
                }
                else
                {
                    currentMap = MapDataClasses.MapDataManager.createMap(dungeonName);
                    player.rootX = currentMap.startX;
                    player.rootY = currentMap.startY;
                    player.rootMap = dungeonName;

                    //Create party and set to default
                    PlayerModels.Models.PartyModel pm = new PlayerModels.Models.PartyModel();
                    pm.x = currentMap.startX;
                    pm.y = currentMap.startY;
                    pm.location = currentMap;
                    pm.maxSize = party.Length;
                    pm.characters = new List<PlayerModels.Models.PartyCharacterModel>();
                    foreach (string s in party)
                    {
                        foreach (PlayerModels.Models.CharacterModel cm in player.characters)
                        {
                            if (cm.name == s)
                            {
                                pm.characters.Add(new PlayerModels.Models.PartyCharacterModel() { characterUniq = cm.uniq });
                            }
                        }
                    }

                    player.parties.Add(pm);

                    player.copyTo(user.player);
                    db.SaveChanges();
                    player.activeParty = pm.uniq;
                    player.copyTo(user.player);
                    db.SaveChanges();
                }

            }
        }

        public void setOptionInteraction(int x, int y, string option)
        {
            MapDataClasses.MapDataManager.interactWithMap(currentMap.name, x, y, currentMap, option);
        }

        public void moveLeft()
        {
            if (player.rootX == 0 || !MapDataClasses.MapDataManager.getTraversable(currentMap.map[player.rootX - 1, player.rootY]))
            {
                return;
            }
            player.rootX -= 1;
        }

        public void moveUp()
        {
            if (player.rootY == 0 || !MapDataClasses.MapDataManager.getTraversable(currentMap.map[player.rootX, player.rootY - 1]))
            {
                return;
            }
            player.rootY -= 1;
        }

        public void moveRight()
        {
            if (player.rootX == currentMap.map.GetLength(0) - 1 || !MapDataClasses.MapDataManager.getTraversable(currentMap.map[player.rootX + 1, player.rootY]))
            {
                return;
            }
            player.rootX += 1;
        }

        public void moveDown()
        {
            if (player.rootY == currentMap.map.GetLength(1) || !MapDataClasses.MapDataManager.getTraversable(currentMap.map[player.rootX, player.rootY + 1]))
            {
                return;
            }
            player.rootY += 1;
        }
    }
}
