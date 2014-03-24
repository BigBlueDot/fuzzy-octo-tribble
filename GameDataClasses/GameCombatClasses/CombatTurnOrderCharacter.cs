using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDataClasses.GameCombatClasses
{
    public class CombatTurnOrderCharacter
    {
        public bool isPC { get; set; }
        public int characterUniq { get; set; }
        public int timeExecuted { get; set; }
    }
}
