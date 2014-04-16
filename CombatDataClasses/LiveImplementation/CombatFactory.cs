﻿using CombatDataClasses.Interfaces;
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
        private string map;
        private Func<int> randomNumberGenerator;
        private Func<float> initiativeCalculator;
        public CombatFactory(PlayerModels.PlayerModel playerModel, string map, Func<int> randomNumberGenerator, Func<float> initiativeCalculator)
        {
            this.playerModel = playerModel;
            this.map = map;
            this.randomNumberGenerator = randomNumberGenerator;
            this.initiativeCalculator = initiativeCalculator;
        }

        public ICombat generateCombat()
        {
            return new Combat(playerModel, map, randomNumberGenerator(), initiativeCalculator);
        }
    }
}
