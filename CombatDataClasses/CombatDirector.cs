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
        private ICombatFactory factory;

        public CombatDirector()
        {
            //Default to dummy factory
            factory = new DummyImplementation.DummyCombatFactory();
        }

        //TODO:  Add a constructor that takes in pointers to current party, loads enemies etc.

        public ICombat getCombat()
        {
            return factory.generateCombat();
        }
    }
}
