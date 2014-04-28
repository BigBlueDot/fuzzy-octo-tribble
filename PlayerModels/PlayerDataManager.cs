using MapDataClasses.EventClasses;
using PlayerModels.Models;
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
            returnValue.battleCommands = new List<PlayerModels.CharacterBattleCommandModel>();
            returnValue.battleCommands.Add(new PlayerModels.CharacterBattleCommandModel() { name = "Attack" });
            returnValue.battleCommands.Add(new PlayerModels.CharacterBattleCommandModel() { name = "Ability" });
            returnValue.battleCommands.Add(new PlayerModels.CharacterBattleCommandModel() { name = "Guard" });
            returnValue.battleCommands.Add(new PlayerModels.CharacterBattleCommandModel() { name = "Use Item" });
            returnValue.battleCommands.Add(new PlayerModels.CharacterBattleCommandModel() { name = "Flee" });
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
            returnValue.commands = new List<CharacterCommandModel>();
            returnValue.completedQuests = new List<CharacterCompletedQuestModel>();
            returnValue.configuration = new List<Models.ConfigurationModel>();
            returnValue.gp = 0;
            returnValue.items = new List<Models.PlayerItemModel>();
            returnValue.parties = new List<Models.PartyModel>();
            returnValue.purchaseables = new List<CharacterPurchaseableModel>();
            returnValue.purchaseables.Add(new CharacterPurchaseableModel() { name = "Tutorial Quest Shop" });
            returnValue.purchaseables.Add(new CharacterPurchaseableModel() { name = "Tutorial Rest" });
            returnValue.unlockedClasses = new List<CharacterUnlockedClassModel>();
            returnValue.unlockedClasses.Add(new CharacterUnlockedClassModel() { name = "Adventurer" });
            returnValue.unlockedClasses.Add(new CharacterUnlockedClassModel() { name = "Brawler" });
            returnValue.rootMap = "Ensemble Village";
            returnValue.rootX = 5;
            returnValue.rootY = 5;
            return returnValue;
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
            pm.characters.Add(cm);
        }

        public static void givePartyXP(PlayerModel pm, int xp)
        {
            List<int> partyUniqs = new List<int>();
            PartyModel currentParty = pm.getActiveParty();
            foreach (PartyCharacterModel pcm in currentParty.characters)
            {
                partyUniqs.Add(pcm.characterUniq);
            }

            foreach (int uniq in partyUniqs)
            {
                foreach (CharacterModel cm in pm.characters)
                {
                    if(cm.uniq == uniq)
                    {
                        cm.xp += xp;
                    }
                }
            }
        }

        public static void givePartyCP(PlayerModel pm, int cp)
        {
            List<int> partyUniqs = new List<int>();
            PartyModel currentParty = pm.getActiveParty();
            foreach (PartyCharacterModel pcm in currentParty.characters)
            {
                partyUniqs.Add(pcm.characterUniq);
            }

            foreach (int uniq in partyUniqs)
            {
                foreach (CharacterModel cm in pm.characters)
                {
                    if (cm.uniq == uniq)
                    {
                        cm.addCp(cp);
                    }
                }
            }
        }

        public static void givePartyGP(PlayerModel pm, int gp)
        {
            pm.gp += gp;
        }
    }
}
