using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDataClasses.GameCombatClasses
{
    public class CombatNPC
    {
        public CombatStats stats { get; set; }
        public List<CombatModifier> modifiers { get; set; }
        public CombatNPCStats npcStats { get; set; }
        public int nextAttackTime { get; set; } 

        public CombatNPC()
        {
            modifiers = new List<CombatModifier>();
        }

        public void getAction()
        {
            //This is probably where the AI code will go

        }
    }
}
