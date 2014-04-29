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
        private string map;
        private Func<int> randomNumberGenerator;
        private Func<float> initiativeCalculator;
        private Action onGameOver;
        private Action onUpdate;
        private Action<CombatEndType> onCombatComplete;
        public CombatFactory(PlayerModels.PlayerModel playerModel, string map, Func<int> randomNumberGenerator, Func<float> initiativeCalculator, Action onGameOver, Action onUpdate, Action<CombatEndType> onCombatComplete)
        {
            this.playerModel = playerModel;
            this.map = map;
            this.randomNumberGenerator = randomNumberGenerator;
            this.initiativeCalculator = initiativeCalculator;
            this.onGameOver = onGameOver;
            this.onUpdate = onUpdate;
            this.onCombatComplete = onCombatComplete;
        }

        public void setMap(string map)
        {
            this.map = map;
        }

        public ICombat generateCombat()
        {
            return new Combat(playerModel, map, randomNumberGenerator(), initiativeCalculator, onGameOver, onUpdate, onCombatComplete);
        }
    }
}
