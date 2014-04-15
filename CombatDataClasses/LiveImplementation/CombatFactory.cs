using CombatDataClasses.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombatDataClasses.LiveImplementation
{
    public class CombatFactory : ICombatFactory
    {
        private PlayerModels.PlayerModel playerModel;
        public CombatFactory(PlayerModels.PlayerModel playerModel)
        {
            this.playerModel = playerModel;
        }

        public ICombat generateCombat()
        {
            return new Combat(playerModel);
        }
    }
}
