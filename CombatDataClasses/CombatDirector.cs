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

        public CombatDirector(PlayerModels.PlayerModel playerModel, string initialMap, Func<int> randomNumberGenerator)
        {
            factory = new LiveImplementation.CombatFactory(playerModel, initialMap, randomNumberGenerator);
        }

        //TODO:  Add a constructor that takes in pointers to current party, loads enemies etc.

        public ICombat getCombat()
        {
            return factory.generateCombat();
        }

        public void setMap(string map)
        {

        }
    }
}
