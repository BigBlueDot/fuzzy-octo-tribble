using GameDataClasses;
using MapDataClasses;
using MapDataClasses.EventClasses;
using PlayerModels.Models;
using PlayerModels.Objective;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerModels
{
    public class PlayerDataManager
    {
        public static PlayerModel initPlayerModel()
        {
            PlayerModel returnValue = new PlayerModel();
            returnValue.characters = new List<Models.CharacterModel>();
            Models.CharacterModel cm = new Models.CharacterModel();
            cm.name = "Lance Renfro";
            cm.currentClass = "Adventurer";
            cm.lvl = 1;
            cm.activeAbilities = new List<Models.CharacterAbilityModel>();
            cm.characterClasses = new List<Models.CharacterClassModel>();
            cm.characterClasses.Add(new Models.CharacterClassModel() { className = "Adventurer", cp = 0, lvl = 1 });
            cm.characterClasses.Add(new Models.CharacterClassModel() { className = "Brawler", cp = 0, lvl = 1 });
            cm.currentQuest = new Models.CharacterQuestModel();
            cm.equipment = new Models.EquipmentModel() { accessory = "", armor = "", weapon = "" };
            cm.stats = new Models.StatsModel() { maxHP = 25, maxMP = 1, strength = 5, vitality = 5, intellect = 5, wisdom = 5, agility = 5 };
            returnValue.characters.Add(cm);
            cm = new Models.CharacterModel();
            cm.name = "Keenan Centers";
            cm.currentClass = "Adventurer";
            cm.lvl = 1;
            cm.activeAbilities = new List<Models.CharacterAbilityModel>();
            cm.characterClasses = new List<Models.CharacterClassModel>();
            cm.characterClasses.Add(new Models.CharacterClassModel() { className = "Adventurer", cp = 0, lvl = 1 });
            cm.characterClasses.Add(new Models.CharacterClassModel() { className = "Brawler", cp = 0, lvl = 1 });
            cm.currentQuest = new Models.CharacterQuestModel();
            cm.equipment = new Models.EquipmentModel() { accessory = "", armor = "", weapon = "" };
            cm.stats = new Models.StatsModel() { maxHP = 25, maxMP = 1, strength = 5, vitality = 5, intellect = 5, wisdom = 5, agility = 5 };
            returnValue.characters.Add(cm);
            returnValue.configuration = new List<Models.ConfigurationModel>();
            returnValue.gp = 0;
            returnValue.items = new List<Models.PlayerItemModel>();
            returnValue.parties = new List<Models.PartyModel>();
            returnValue.rootMap = "Ensemble Village";
            returnValue.rootX = 5;
            returnValue.rootY = 5;
            return returnValue;
        }

        public static void processEvent(MapEventModel currentEvent, PlayerModel player, ref List<ClientMessage> messageQueue)
        {
            switch (currentEvent.rewardType)
            {
                case MapDataClasses.ClientEvent.RewardType.Objective:
                    string returnValue = ObjectiveDirector.completeObjective(player, currentEvent.eventData.objective);
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
                    List<string> results = PlayerDataManager.givePartyXP(player, currentEvent.rewardValue);
                    foreach (string s in results)
                    {
                        messageQueue.Add(new ClientMessage()
                        {
                            message = s
                        });
                    }
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
                    List<string> cpResults = PlayerDataManager.givePartyCP(player, currentEvent.rewardValue);
                    foreach (string s in cpResults)
                    {
                        messageQueue.Add(new ClientMessage()
                        {
                            message = s
                        });
                    }
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

        public static List<Models.CharacterModel> getInactiveCharacters(PlayerModel pm)
        {
            bool canAdd = true;
            List<Models.CharacterModel> returnValues = new List<Models.CharacterModel>();
            foreach (Models.CharacterModel cm in pm.characters)
            {
                foreach (Models.PartyModel party in pm.parties)
                {
                    foreach (Models.PartyCharacterModel pcm in party.characters)
                    {
                        if (pcm.uniq == cm.uniq)
                        {
                            canAdd = false;
                        }
                    }
                }

                if (canAdd)
                {
                    returnValues.Add(cm);
                }
            }

            return returnValues;
        }

        public static List<Models.CharacterModel> getCurrentParty(PlayerModel pm, List<int> characterUniqs = null)
        {
            if (characterUniqs == null)
            {
                characterUniqs = new List<int>();
                List<Models.PartyCharacterModel> partyCharacters = getCurrentPartyPartyStats(pm);
                foreach (Models.PartyCharacterModel pcm in partyCharacters)
                {
                    characterUniqs.Add(pcm.characterUniq);
                }
            }

            List<Models.CharacterModel> returnValues = new List<Models.CharacterModel>();
            foreach(int characterUniq in characterUniqs) 
            {
                foreach (Models.CharacterModel cm in pm.characters)
                {
                    if (cm.uniq == characterUniq)
                    {
                        returnValues.Add(cm);
                    }
                }
            }
            return returnValues;
        }

        public static List<Models.PartyCharacterModel> getCurrentPartyPartyStats(PlayerModel pm) {
            List<Models.PartyCharacterModel> returnValues = new List<Models.PartyCharacterModel>();

            foreach(Models.PartyModel partyModels in pm.parties) 
            {
                if(partyModels.uniq == pm.activeParty)
                {
                    foreach(PlayerModels.Models.PartyCharacterModel pcm in partyModels.characters) 
                    {
                        returnValues.Add(pcm);
                    }
                }
            }

            return returnValues;
        }

        public static List<CombatDataModels.CombatCharacterModel> getCurrentPartyCombatStats(PlayerModel pm, List<int> characterUniqs = null)
        {
            if (characterUniqs == null)
            {
                characterUniqs = new List<int>();
                List<Models.PartyCharacterModel> partyCharacters = getCurrentPartyPartyStats(pm);
                foreach (Models.PartyCharacterModel pcm in partyCharacters)
                {
                    characterUniqs.Add(pcm.characterUniq);
                }
            }

            List<CombatDataModels.CombatCharacterModel> returnValues = new List<CombatDataModels.CombatCharacterModel>();


            foreach (int i in characterUniqs)
            {
                foreach (CombatDataModels.CombatCharacterModel cpm in pm.currentCombat.pcs)
                {
                    if (cpm.characterUniq == i)
                    {
                        returnValues.Add(cpm);
                    }
                }
            }

            return returnValues;
        }

        public static int getXPForLevel(int currentLevel)
        {
            int exponent = ((int)Math.Floor(((double)currentLevel / 10)) + 2);
            int xpForLevel = (int)Math.Pow(((Math.Pow((double)currentLevel, (double)2.0)) + (double)3.5), (double)exponent);
            return xpForLevel;
        }

        public static int getCPForLevel(int currentLevel)
        {
            if (currentLevel == 19)
            {
                return int.MaxValue;
            }
            else
            {
                return (currentLevel + 4) * currentLevel * ((currentLevel / 5) + 1);
            }
        }

        public static void addCharacter(PlayerModel pm)
        {
            Models.CharacterModel cm = new Models.CharacterModel();
            cm.name = "Alexander";
            cm.currentClass = "Adventurer";
            cm.lvl = 1;
            cm.activeAbilities = new List<Models.CharacterAbilityModel>();
            cm.characterClasses = new List<Models.CharacterClassModel>();
            cm.characterClasses.Add(new Models.CharacterClassModel() { className = "Adventurer", cp = 0, lvl = 1 });
            cm.currentQuest = new Models.CharacterQuestModel();
            cm.equipment = new Models.EquipmentModel() { accessory = "", armor = "", weapon = "" };
            cm.stats = new Models.StatsModel() { maxHP = 25, maxMP = 1, strength = 5, vitality = 5, intellect = 5, wisdom = 5, agility = 5 };
            StatCalculations.StatCalculator.updateCharacterStats(cm);
            pm.characters.Add(cm);
        }

        public static List<string> getClasses(PlayerModel pm)
        {
            List<string> classes = new List<string>();
            classes.Add("Adventurer");
            if(pm.isObjectiveCompleted(ObjectiveType.Brawler))
            {
                classes.Add("Brawler");
            }
            return classes;
        }

        public static List<string> givePartyXP(PlayerModel pm, int xp)
        {
            List<int> partyUniqs = new List<int>();
            PartyModel currentParty = pm.getActiveParty();
            List<string> results = new List<string>();
            foreach (PartyCharacterModel pcm in currentParty.characters)
            {
                results.AddRange(giveXPCP(pm, pcm.characterUniq, xp, 0));
            }

            return results;
        }

        public static List<string> givePartyCP(PlayerModel pm, int cp)
        {
            List<int> partyUniqs = new List<int>();
            PartyModel currentParty = pm.getActiveParty();
            List<string> results = new List<string>();
            foreach (PartyCharacterModel pcm in currentParty.characters)
            {
                results.AddRange(giveXPCP(pm, pcm.characterUniq, 0, cp));
            }

            return results;
        }

        public static void givePartyGP(PlayerModel pm, int gp)
        {
            pm.gp += gp;
        }

        public static List<string> giveXPCP(PlayerModel playerModel, int characterUniq, int xp, int cp)
        {
            List<string> results = new List<string>();

            foreach (CharacterModel cm in playerModel.characters)
            {
                if (cm.uniq == characterUniq)
                {
                    cm.xp += xp;
                    if (cm.xp >= PlayerModels.PlayerDataManager.getXPForLevel(cm.lvl))
                    {
                        results.Add(cm.name + " has gained a level!");
                        cm.lvl++;
                        PlayerModels.StatCalculations.StatCalculator.updateCharacterStats(cm);
                    }
                    foreach (CharacterClassModel ccm in cm.characterClasses)
                    {
                        if (ccm.className == cm.currentClass)
                        {
                            ccm.cp += cp;
                            if (ccm.cp >= PlayerModels.PlayerDataManager.getCPForLevel(ccm.lvl))
                            {
                                results.Add(cm.name + " has gained a class level!");
                                ccm.lvl++;
                                PlayerModels.StatCalculations.StatCalculator.updateCharacterStats(cm);

                                string newAbilityMessage = PlayerModels.StatCalculations.StatCalculator.getNewAbilityText(cm);
                                if (newAbilityMessage != string.Empty)
                                {
                                    results.Add(newAbilityMessage);
                                }
                            }
                        }
                    }
                }
            }

            return results;
        }
    }
}
