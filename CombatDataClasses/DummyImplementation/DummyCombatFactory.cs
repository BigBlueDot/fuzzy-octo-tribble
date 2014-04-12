using CombatDataClasses.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombatDataClasses.DummyImplementation
{
    public class DummyCombatFactory : ICombatFactory
    {
        public ICombat generateCombat()
        {
            ICombat returnValue = new DummyCombat();

            return returnValue;
        }
    }
}
