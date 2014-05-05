using PlayerModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerModels.StatCalculations
{
    public class ClassStatCalculator : IStatCalculator
    {
        private List<Action<CharacterModel>> levelIncreases;
        private List<AbilityDescription> abilities;
        private List<string> statIncreases;
        private string className;

        public ClassStatCalculator(List<Action<CharacterModel>> levelIncreases, List<AbilityDescription> abilities, List<string> statIncreases, string className)
        {
            this.levelIncreases = levelIncreases;
            this.abilities = abilities;
            this.statIncreases = statIncreases;
            this.className = className;
        }

        public void getStats(CharacterModel cm)
        {
            foreach (CharacterClassModel ccm in cm.characterClasses)
            {
                if (ccm.className == className)
                {
                    for (int i = 0; i < ccm.lvl && i < levelIncreases.Count; i++)
                    {
                        levelIncreases[i](cm);
                    }
                }
            }
        }

        public IEnumerable<AbilityDescription> getAbilities(CharacterModel cm)
        {
            List<AbilityDescription> returnValue = new List<AbilityDescription>();

            foreach (CharacterClassModel ccm in cm.characterClasses)
            {
                if (ccm.className == className)
                {
                    for (int i = 0; i < ccm.lvl && i / 2 < abilities.Count; i += 2)
                    {
                        returnValue.Add(this.abilities[i / 2]);
                    }
                }
            }

            return returnValue;
        }

        public string getNewAbilityMessage(CharacterModel cm)
        {
            foreach (CharacterClassModel ccm in cm.characterClasses)
            {
                if (ccm.className == className)
                {
                    if (ccm.lvl % 2 == 1 && abilities.Count < ((ccm.lvl - 1) / 2))
                    {
                        return cm.name + " has learned " + abilities[(ccm.lvl - 1) / 2].name + ".  " + abilities[(ccm.lvl - 1) / 2].description;
                    }
                    else
                    {
                        return statIncreases[(ccm.lvl / 2) - 1]; //Return new message for stat increases
                    }
                }
            }

            return string.Empty;
        }
    }
}
