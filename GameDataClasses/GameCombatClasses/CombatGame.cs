using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDataClasses.GameCombatClasses
{
    public class CombatGame
    {
        public List<CombatPC> pcs { get; set; }
        public List<CombatNPC> npcs { get; set; }
        public CombatTurnOrder turnOrders { get; set; }
        public int currentTime { get; set; }

        public CombatGame()
        {
            pcs = new List<CombatPC>();
            npcs = new List<CombatNPC>();
        }
    }
}
