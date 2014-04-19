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

        public int currentFleeCount { get; set; }
        public List<string> firstTurnOver { get; set; }
        public bool combatInitalized { get; set; }
    }
}
