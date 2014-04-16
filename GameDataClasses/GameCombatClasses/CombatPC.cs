using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDataClasses.GameCombatClasses
{
    public class CombatPC
    {
        public CombatStats stats { get; set; }
        public List<CombatModifier> modifiers { get; set; }
        public int characterUniq { get; set; }
        public int nextAttackTime { get; set; } 

        public CombatPC()
        {
            modifiers = new List<CombatModifier>();
        }

        public void getCommands()
        {
            //Get the commands for this character somehow
        }
    }
}
