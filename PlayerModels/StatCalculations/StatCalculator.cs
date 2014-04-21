using PlayerModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerModels.StatCalculations
{
    public class StatCalculator
    {
        private static Dictionary<string, IStatCalculator> classCalculators;

        static StatCalculator()
        {
            classCalculators = new Dictionary<string, IStatCalculator>();
            classCalculators.Add("Adventurer", new AdventurerStatCalculator());
            classCalculators.Add("Brawler", new BrawlerStatCalculator());
        }

        public static void updateCharacterStats(CharacterModel cm)
        {
            cm.stats.maxHP = 25;
            cm.stats.maxMP = 1;
            cm.stats.strength = 5;
            cm.stats.vitality = 5;
            cm.stats.intellect = 5;
            cm.stats.wisdom = 5;
            cm.stats.agility = 5;

            GeneralStatCalculator.getStats(cm);

            if (classCalculators.ContainsKey(cm.currentClass))
            {
                classCalculators[cm.currentClass].getStats(cm);
            }

            if (classCalculators.ContainsKey(cm.currentClass))
            {
                cm.abilities = classCalculators[cm.currentClass].getAbilities(cm).ToList();
            }
        }

        public static string getNewAbilityText(CharacterModel cm)
        {
            if (classCalculators.ContainsKey(cm.currentClass))
            {
                return classCalculators[cm.currentClass].getNewAbilityMessage(cm);
            }

            return string.Empty;
        }
    }
}
