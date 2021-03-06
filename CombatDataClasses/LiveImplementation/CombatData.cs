﻿using PlayerModels.CombatDataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombatDataClasses.LiveImplementation
{
    public class CombatData
    {
        private CombatDataModel combatData { get; set; }
        public bool combatInitalized
        {
            get
            {
                return combatData.combatInitalized;
            }
            set
            {
                combatData.combatInitalized = value;
            }
        }
        public int currentFleeCount
        {
            get
            {
                return combatData.currentFleeCount;
            }
            set
            {
                combatData.currentFleeCount = value;
            }
        }

        public List<string> firstTurnOver
        {
            get
            {
                List<string> returnValues = new List<string>();
                foreach (CombatDataModel.TurnOverModel tom in combatData.firstTurnOver)
                {
                    returnValues.Add(tom.name);
                }
                return returnValues;
            }
        }
        public List<CooldownModel> cooldowns
        {
            get
            {
                return combatData.cooldowns;
            }
            set
            {
                combatData.cooldowns = value;
            }
        }
        public CombatEndType combatEndType
        {
            get;
            set;
        }
        public bool canFlee
        {
            get
            {
                return combatData.canFlee;
            }
            set
            {
                combatData.canFlee = value;
            }
        }

        public PlayerModels.CombatDataModels.CombatDataModel.DoubleSelectionState doubleSelectionState
        {
            get
            {
                return combatData.doubleSelectionState;
            }
            set
            {
                combatData.doubleSelectionState = value;
            }
        }

        public int doubleSelectionDelay
        {
            get
            {
                return combatData.doubleSelectionDelay;
            }
            set
            {
                combatData.doubleSelectionDelay = value;
            }
        }

        public CombatData(CombatModel combat, bool canFlee)
        {
            if (combat.combatData == null)
            {
                combat.combatData = this.combatData = new CombatDataModel();
                combat.combatData.canFlee = canFlee;

                Init();
            }
            
            this.combatData = combat.combatData;
            this.doubleSelectionState = PlayerModels.CombatDataModels.CombatDataModel.DoubleSelectionState.None;

            if (this.combatData.firstTurnOver == null)
            {
                this.combatData.firstTurnOver = new List<CombatDataModel.TurnOverModel>();
            }
        }

        private void Init()
        {
            this.combatData.currentFleeCount = 0;
            this.combatData.firstTurnOver = new List<CombatDataModel.TurnOverModel>();
            this.combatData.combatInitalized = false;
            this.combatData.cooldowns = new List<CooldownModel>();
        }

        public void setFirstTurnOver(string name)
        {
            if (!this.firstTurnOver.Contains(name))
            {
                combatData.firstTurnOver.Add(new CombatDataModel.TurnOverModel() { name = name });
            }
        }

        public bool isFirstTurn(string name)
        {
            return !this.firstTurnOver.Contains(name);
        }

        public void removeCooldowns(int time)
        {
            List<CooldownModel> toRemove = new List<CooldownModel>();
            foreach (CooldownModel cd in combatData.cooldowns)
            {
                if (cd.time <= time)
                {
                    toRemove.Add(cd);
                }
            }

            foreach (CooldownModel cd in toRemove)
            {
                combatData.cooldowns.Remove(cd);
            }
        }

        public bool hasCooldown(string characterName, string attack)
        {
            if (attack == string.Empty)
            {
                return false;
            }

            foreach (CooldownModel cd in combatData.cooldowns)
            {
                if (cd.character == characterName && cd.name == attack)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
