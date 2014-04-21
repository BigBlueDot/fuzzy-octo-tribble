using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombatDataClasses.LiveImplementation
{
    public class CombatData
    {
        public CombatData()
        {
            Init();
        }

        private void Init()
        {
            currentFleeCount = 0;
            firstTurnOver = new List<string>();
            combatInitalized = false;
            cooldowns = new List<Cooldown>();
        }

        public void setFirstTurnOver(string name)
        {
            if (!firstTurnOver.Contains(name))
            {
                firstTurnOver.Add(name);
            }
        }

        public bool isFirstTurn(string name)
        {
            return !firstTurnOver.Contains(name);
        }

        public void removeCooldowns(int time)
        {
            List<Cooldown> toRemove = new List<Cooldown>();
            foreach (Cooldown cd in cooldowns)
            {
                if (cd.time <= time)
                {
                    toRemove.Add(cd);
                }
            }

            foreach (Cooldown cd in toRemove)
            {
                cooldowns.Remove(cd);
            }
        }

        public bool hasCooldown(string characterName, string attack)
        {
            foreach (Cooldown cd in cooldowns)
            {
                if (cd.character == characterName && cd.name == attack)
                {
                    return true;
                }
            }

            return false;
        }

        public int currentFleeCount { get; set; }
        public List<string> firstTurnOver { get; set; }
        public bool combatInitalized { get; set; }
        public List<Cooldown> cooldowns { get; set; }

        public class Cooldown
        {
            public string character { get; set; }
            public string name { get; set; }
            public int time { get; set; }
        }
    }
}
