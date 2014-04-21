using PlayerModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerModels.StatCalculations
{
    public class GeneralStatCalculator
    {
        private static List<Action<CharacterModel>> levelIncreases;

        static GeneralStatCalculator()
        {
            levelIncreases = new List<Action<CharacterModel>>();
            for (int i = 1; i <= 15; i++)
            {
                levelIncreases.Add((CharacterModel cm) =>
                {
                    cm.stats.maxHP += 5;
                    cm.stats.strength += 1;
                    cm.stats.vitality += 1;
                    cm.stats.wisdom += 1;
                    cm.stats.intellect += 1;
                    cm.stats.agility += 1;
                });
            }
        }

        public static void getStats(CharacterModel cm)
        {
            for (int i = 1; i < cm.lvl && i < levelIncreases.Count; i++)
            {
                levelIncreases[i](cm);
            }
        }
    }
}
