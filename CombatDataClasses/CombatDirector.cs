using CombatDataClasses.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombatDataClasses
{
    public class CombatDirector
    {
        ICombatFactory factory;

        public CombatDirector()
        {
            //Load dummy controls for now
            factory = new DummyImplementation.DummyCombatFactory();
        }

        public ICombat getCombat()
        {
            return factory.generateCombat();
        }
    }
}
