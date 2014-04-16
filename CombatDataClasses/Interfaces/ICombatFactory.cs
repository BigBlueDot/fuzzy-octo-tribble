using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombatDataClasses.Interfaces
{
    public interface ICombatFactory
    {
        ICombat generateCombat();
        void setMap(string map);
    }
}
