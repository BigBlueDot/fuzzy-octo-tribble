using fuzzy_octo_tribble.Models;
using GameDataClasses.GamePlayerClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using CombatDataClasses.Interfaces;
using PlayerModels.Models;
using PlayerModels;
using PlayerModels.Objective;

namespace GameDataClasses
{
    public class Game : ICombat
    {
        //needs to store the player and any active maps
        public PlayerModels.PlayerModel player { get; set; }
        private MapDataClasses.MapModel currentMap;
        private string userName;
        private UserProfile user;
        private UsersContext db;
        private CombatDataClasses.CombatDirector combatDirector;
        private ICombat combat;
        private int combatCountdown;
        private GameRNG rng;
        private List<ClientMessage> messageQueue;

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
                    .Include(up => up.player.objectives)
                    .Include(up => up.player.parties)
                    .Include(up => up.player.parties.Select(c => c.characters))
                    .Include(up => up.player.parties.Select(c => c.location))
                    .Include(up => up.player.parties.Select(c => c.location).Select(t => t.eventCollection))
                    .Include(up => up.player.parties.Select(c => c.location).Select(t => t.eventCollection).Select(e => e.events))
                    .Include(up => up.player.parties.Select(c => c.location).Select(t => t.eventCollection).Select(e => e.events.Select(k => k.eventData)))
                    .Include(up => up.player.parties.Select(c => c.location).Select(t => t.eventCollection).Select(e => e.events.Select(k => k.eventData.encounter)))
                    .Include(up => up.player.parties.Select(c => c.location).Select(t => t.eventCollection).Select(e => e.events.Select(k => k.eventData.encounter.enemies)))
                    .Include(up => up.player.currentCombat)
                    .Include(up => up.player.currentCombat.pcs)
                    .Include(up => up.player.currentCombat.npcs)
                    .Include(up => up.player.currentCombat.pcs.Select(c => c.stats))
                    .Include(up => up.player.currentCombat.pcs.Select(c => c.mods))
                    .Include(up => up.player.currentCombat.npcs.Select(c => c.stats))
                    .Include(up => up.player.currentCombat.npcs.Select(c => c.mods))
                    .Include(up => up.player.unlockedClasses)
                    .Include(up => up.player.currentCombat.combatData)
                    .Include(up => up.player.currentCombat.combatData.cooldowns)
                    .FirstOrDefault(u => u.UserName.ToLower() == userName);
            this.player = user.player;
            this.userName = userName;
            this.db = db;
            this.messageQueue = new List<ClientMessage>();

            this.rng = new GameRNG();

            //Initialize player stuff if they have no value
            if (player.objectives == null)
            {
                player.objectives = new List<PlayerObjectiveModel>();
            }

            MapDataClasses.MapDataManager.setFunctions(() =>
            {
                List<string> characters = new List<string>();
                foreach (CharacterModel cm in this.player.characters)
                {
                    characters.Add(cm.name);
                }
                return characters;
            }, () =>
            {
                List<string> classes = new List<string>();
                foreach (CharacterUnlockedClassModel cm in this.player.unlockedClasses)
                {
                    classes.Add(cm.name);
                }
                return classes;
            }, (int start, int end) =>
            {
                return this.rng.getNumber(start, end);
            }, (string name) =>
                {
                    return DungeonUnlockedDirector.isDungeonUnlocked(name, player);
                }
            );

            currentMap = player.getActiveParty().location;
            if (currentMap == null)
            {
                currentMap = MapDataClasses.MapDataManager.createMap(this.player.rootMap);
                this.player.getActiveParty().location = currentMap;
                ObjectiveDirector.markCompletedObjectives(this.player);
            }
            else
            {
                MapDataClasses.MapDataManager.setupMapModel(currentMap);
                this.player.getActiveParty().location = currentMap;
                ObjectiveDirector.markCompletedObjectives(this.player);
            }

            foreach (CharacterModel cm in this.player.characters)
            {
                PlayerModels.StatCalculations.StatCalculator.updateCharacterStats(cm);
            }

            this.combatCountdown = rng.getNumber(MapDataClasses.MapDataManager.getMinCombatCount(player.rootMap), MapDataClasses.MapDataManager.getMaxCombatCount(player.rootMap));

            combatDirector = new CombatDataClasses.CombatDirector(this.player,
                currentMap.name,
                () =>
                {
                    return rng.getNumber(1, MapDataClasses.MapDataManager.getRandomEncounterCount(currentMap.name));
                },
                () =>
                {
                    return rng.calculateIntiative();
                },
                () =>
                {
                    setMap(MapDataClasses.MapDataManager.getHubMap(player.rootMap), true);
                    this.player.getActiveParty().location = currentMap;
                    this.combatCountdown = rng.getNumber(MapDataClasses.MapDataManager.getMinCombatCount(player.rootMap), MapDataClasses.MapDataManager.getMaxCombatCount(player.rootMap));
                    currentMap = MapDataClasses.MapDataManager.createMap(player.rootMap);
                    ObjectiveDirector.markCompletedObjectives(this.player);
                    player.rootX = currentMap.startX;
                    player.rootY = currentMap.startY;
                },
                () =>
                {
                    db.SaveChanges();
                },
                () =>
                {
                    if (player.currentCombat != null)
                    {
                        foreach (PartyModel pm in player.parties)
                        {
                            if (pm.uniq == player.activeParty)
                            {
                                foreach (PartyCharacterModel pcm in pm.characters)
                                {
                                    foreach (PlayerModels.CombatDataModels.CombatCharacterModel ccm in player.currentCombat.pcs)
                                    {
                                        if (pcm.characterUniq == ccm.characterUniq)
                                        {
                                            pcm.hp = ccm.stats.hp;
                                            pcm.mp = ccm.stats.mp;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    player.currentCombat = null;

                    if (player.getActiveParty() != null)
                    {
                        MapDataClasses.MapEventModel currentEvent = player.getActiveParty().location.activeEvent;
                        if (currentEvent != null && currentEvent.eventData.type == MapDataClasses.EventClasses.EventDataType.Combat)
                        {
                            if (currentEvent.eventData.nextEvent == null)
                            {
                                switch (currentEvent.rewardType)
                                {
                                    case MapDataClasses.ClientEvent.RewardType.Objective:
                                        string returnValue = ObjectiveDirector.completeObjective(this.player, currentEvent.eventData.objective);
                                        if (returnValue != string.Empty)
                                        {
                                            messageQueue.Add(new ClientMessage()
                                            {
                                                message = returnValue,
                                                type = ClientMessage.ClientMessageType.Message
                                            });
                                            messageQueue.Add(new ClientMessage()
                                            {
                                                type = ClientMessage.ClientMessageType.RefreshMap
                                            });
                                        }
                                        break;
                                    case MapDataClasses.ClientEvent.RewardType.XP:
                                        messageQueue.Add(new ClientMessage()
                                        {
                                            message = "All characters have gained " + currentEvent.rewardValue.ToString() + " XP!",
                                            type = ClientMessage.ClientMessageType.Message
                                        });
                                        messageQueue.Add(new ClientMessage()
                                        {
                                            type = ClientMessage.ClientMessageType.RefreshMap
                                        });
                                        PlayerDataManager.givePartyXP(player, currentEvent.rewardValue);
                                        break;
                                    case MapDataClasses.ClientEvent.RewardType.CP:
                                        messageQueue.Add(new ClientMessage()
                                        {
                                            message = "All characters have gained " + currentEvent.rewardValue.ToString() + " CP!",
                                            type = ClientMessage.ClientMessageType.Message
                                        });
                                        messageQueue.Add(new ClientMessage()
                                        {
                                            type = ClientMessage.ClientMessageType.RefreshMap
                                        });
                                        PlayerDataManager.givePartyCP(player, currentEvent.rewardValue);
                                        break;
                                    case MapDataClasses.ClientEvent.RewardType.Gold:
                                        messageQueue.Add(new ClientMessage()
                                        {
                                            message = "You have found " + currentEvent.rewardValue.ToString() + " GP!",
                                            type = ClientMessage.ClientMessageType.Message
                                        });
                                        messageQueue.Add(new ClientMessage()
                                        {
                                            type = ClientMessage.ClientMessageType.RefreshMap
                                        });
                                        PlayerDataManager.givePartyGP(player, currentEvent.rewardValue);
                                        break;
                                }
                                player.getActiveParty().location.eventCollection.removeEvent(currentEvent);
                                player.getActiveParty().location.activeEvent = null;
                            }
                            else
                            {
                                messageQueue.Add(new ClientMessage() { type = ClientMessage.ClientMessageType.ExecuteEvent });
                                currentEvent.eventData = currentEvent.eventData.nextEvent;
                                player.getActiveParty().location.activeEvent = currentEvent;
                            }

                        }
                    }

                    db.SaveChanges();
                }
                );
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
            bool isInCombat = true;
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
            if (player.currentCombat == null)
            {
                isInCombat = false;
            }
            ClientPlayer cp = new ClientPlayer() { gp = player.gp, x = player.rootX, y = player.rootY, characters = player.characters, currentPartyCharacters = currentPartyCharacters, isInCombat = isInCombat };

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

        public void setClass(int x, int y, string characterName, string className)
        {
            if (MapDataClasses.MapDataManager.validateClassChangeSelection(currentMap.name, x, y, currentMap))
            {
                foreach (CharacterUnlockedClassModel cucm in player.unlockedClasses)
                {
                    if (cucm.name == className) //Validate that the user has the class
                    {
                        foreach (CharacterModel cm in player.characters)
                        {
                            if (cm.name == characterName)
                            {
                                cm.currentClass = className;
                                bool foundClass = false;
                                foreach (CharacterClassModel ccm in cm.characterClasses)
                                {
                                    if (ccm.className == className)
                                    {
                                        foundClass = true;
                                    }
                                }

                                if (!foundClass)
                                {
                                    cm.characterClasses.Add(new CharacterClassModel() { className = className, cp = 0, lvl = 1 });
                                }

                                //Update stats for new class
                                PlayerModels.StatCalculations.StatCalculator.updateCharacterStats(cm);
                            }
                        }
                    }
                }
            }

            db.SaveChanges();
        }

        public void loadDungeon(int x, int y, string dungeonName, string[] party, bool combat = false)
        {
            //Verify that the dungeon selection is legitimate
            if (combat || MapDataClasses.MapDataManager.validateDungeonSelection(currentMap.name, x, y, currentMap, dungeonName))
            {
                this.combatCountdown = rng.getNumber(MapDataClasses.MapDataManager.getMinCombatCount(dungeonName), MapDataClasses.MapDataManager.getMaxCombatCount(dungeonName));
                if (MapDataClasses.MapDataManager.getHubMap(dungeonName) == dungeonName)
                {
                    //Need to disband the party that is currently being used
                    currentMap = MapDataClasses.MapDataManager.createMap(dungeonName);
                    this.player.getActiveParty().location = currentMap;
                    ObjectiveDirector.markCompletedObjectives(this.player);
                    player.rootX = currentMap.startX;
                    player.rootY = currentMap.startY;
                    setMap(dungeonName);

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
                                pm.characters.Add(new PlayerModels.Models.PartyCharacterModel() { characterUniq = cm.uniq, hp = cm.stats.maxHP, mp = cm.stats.maxMP, usedAbilities = new List<string>() });
                            }
                        }
                    }

                    player.parties.Add(pm);

                    player.copyTo(user.player);
                    db.SaveChanges();
                    player.activeParty = pm.uniq;
                    player.copyTo(user.player);
                    this.player.getActiveParty().location = currentMap;
                    ObjectiveDirector.markCompletedObjectives(this.player);
                    player.rootX = currentMap.startX;
                    player.rootY = currentMap.startY;
                    setMap(dungeonName);
                    db.SaveChanges();
                }

            }
        }

        public void setOptionInteraction(int x, int y, string option)
        {
            MapDataClasses.MapDataManager.interactWithMap(currentMap.name, x, y, currentMap, option);
        }

        public MapEvent moveLeft()
        {
            if (player.rootX == 0)
            {
                return MapEvent.Nothing;
            }
            return move(player.rootX - 1, player.rootY);
        }

        public MapEvent moveUp()
        {
            if (player.rootY == 0)
            {
                return MapEvent.Nothing;
            }
            return move(player.rootX, player.rootY - 1);
        }

        public MapEvent moveRight()
        {
            if (player.rootX == currentMap.map.GetLength(0) - 1)
            {
                return MapEvent.Nothing;
            }
            return move(player.rootX + 1, player.rootY);
        }

        public MapEvent moveDown()
        {
            if (player.rootY == currentMap.map.GetLength(1))
            {
                return MapEvent.Nothing;
            }
            return move(player.rootX, player.rootY + 1);
        }

        public MapEvent standStill()
        {
            return move(player.rootX, player.rootY);
        }

        private MapEvent move(int newX, int newY)
        {
            if (!MapDataClasses.MapDataManager.getTraversable(currentMap.map[newX, newY]))
            {
                return MapEvent.Nothing;
            }

            player.rootX = newX;
            player.rootY = newY;

            if(MapDataClasses.MapDataManager.isEvent(currentMap, newX, newY))
            {
                MapDataClasses.MapEventModel mapEvent = MapDataClasses.MapDataManager.getEvent(currentMap, newX, newY);

                switch (mapEvent.eventData.type)
                {
                    case MapDataClasses.EventClasses.EventDataType.Combat:
                        player.getActiveParty().location.activeEvent = mapEvent;
                        combat = combatDirector.getCombat();
                        return MapEvent.CombatEntered;
                    default:
                        return MapEvent.Nothing;
                }
            }
            else if (isCombat())
            {
                combat = combatDirector.getCombat(); 
                return MapEvent.CombatEntered;
            }
            return MapEvent.Nothing;
        }

        public List<ICommand> getCommands()
        {
            return combat.getCommands();
        }

        public ICombatStatus getStatus()
        {
            if (combat == null) //Game loaded without combat being loaded normally
            {
                combatCountdown = 0;
                isCombat(); //This will cause it to init the combatCountdown properly
                combat = combatDirector.getCombat(); 
            }
            return combat.getStatus();
        }

        public ICombatStatus executeCommand(SelectedCommand command)
        {
            return combat.executeCommand(command);
        }

        public ICombatStatus nextTurn()
        {
            return combat.nextTurn();
        }

        public List<ClientMessage> getMessages()
        {
            List<ClientMessage> messages = this.messageQueue;
            this.messageQueue = new List<ClientMessage>();
            return messages;
        }

        private bool isCombat()
        {
            if (MapDataClasses.MapDataManager.isCombatMap(player.rootMap) && player.currentCombat == null)
            {
                combatCountdown--;
                if (combatCountdown <= 0)
                {
                    this.combatCountdown = rng.getNumber(MapDataClasses.MapDataManager.getMinCombatCount(player.rootMap), MapDataClasses.MapDataManager.getMaxCombatCount(player.rootMap));
                    return true;
                }
            }

            return false;
        }

        private void setMap(string newMap, bool isCombat = false)
        {
            player.rootMap = newMap;
            combatDirector.setMap(newMap);
            loadDungeon(0, 0, newMap, new string[0], isCombat);
        }
    }
}
