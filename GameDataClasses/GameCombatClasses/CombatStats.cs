using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDataClasses.GameCombatClasses
{
    public class CombatStats
    {
        //Trey: this can probably be removed once all the code is done.  I think it will end up being redundant
        public int hp { get; set; }
        public int maxHP { get; set; }
        public int mp { get; set; }
        public int maxMP { get; set; }
        public int strength { get; set; }
        public int vitality { get; set; }
        public int intellect { get; set; }
        public int wisdom { get; set; }
        public int agility { get; set; }
    }
}
