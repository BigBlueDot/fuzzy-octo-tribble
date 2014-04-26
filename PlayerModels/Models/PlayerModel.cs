using PlayerModels.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerModels
{
    public class PlayerModel
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int uniq { get; set; }
        public string rootMap { get; set; }
        public int rootX { get; set; }
        public int rootY { get; set; }
        public List<PlayerItemModel> items { get; set; }
        public int gp { get; set; }
        public List<CharacterModel> characters { get; set; }
        public List<PartyModel> parties { get; set; }
        public int activeParty { get; set; }
        public List<CharacterUnlockedClassModel> unlockedClasses { get; set; }
        public List<CharacterCompletedQuestModel> completedQuests { get; set; }
        public List<CharacterBattleCommandModel> battleCommands { get; set; }
        public List<CharacterPurchaseableModel> purchaseables { get; set; }
        public List<ConfigurationModel> configuration { get; set; }
        public List<CharacterCommandModel> commands { get; set; }
        public PlayerModels.CombatDataModels.CombatModel currentCombat { get; set; }

        public void copyTo(PlayerModel pm)
        {
            pm.rootMap = rootMap;
            pm.rootX = rootX;
            pm.rootY = rootY;
            pm.items = items;
            pm.gp = gp;
            pm.characters = characters;
            pm.parties = parties;
            pm.activeParty = activeParty;
            pm.unlockedClasses = unlockedClasses;
            pm.completedQuests = completedQuests;
            pm.battleCommands = battleCommands;
            pm.purchaseables = purchaseables;
            pm.configuration = configuration;
            pm.commands = commands;
        }

        public PartyModel getActiveParty()
        {
            if (activeParty != 0)
            {
                foreach (PartyModel pm in parties)
                {
                    if (pm.uniq == activeParty)
                    {
                        return pm;
                    }
                }
            }

            return new PartyModel();
        }
    }

    public class CharacterUnlockedClassModel
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int uniq { get; set; }
        public string name { get; set; }
    }

    public class CharacterCompletedQuestModel
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int uniq { get; set; }
        public string name { get; set; }
    }

    public class CharacterPurchaseableModel
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int uniq { get; set; }
        public string name { get; set; }
    }

    public class CharacterCommandModel
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int uniq { get; set; }
        public string name { get; set; }
    }

    public class CharacterBattleCommandModel
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int uniq { get; set; }
        public string name { get; set; }
    }
}
