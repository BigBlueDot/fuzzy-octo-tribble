using PlayerModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerModels.StatCalculations
{
    public class MageStatCalculator : IStatCalculator
    {
        private static List<Action<CharacterModel>> levelIncreases;
        private static List<AbilityDescription> abilities;
        private static List<string> statIncreases;
        private static string className;

        static MageStatCalculator()
        {
            className = "Mage";
            levelIncreases = new List<Action<CharacterModel>>();

            for (int i = 1; i <= 19; i++)
            {
                var currentNumber = i;

                levelIncreases.Add((CharacterModel cm) =>
                    {
                        cm.stats.maxHP++;
                        if (currentNumber == 2)
                        {
                            cm.stats.intellect++;
                        }
                        if (currentNumber == 4)
                        {
                            cm.stats.vitality++;
                        }
                        if (currentNumber == 6)
                        {
                            cm.stats.intellect++;
                        }
                        if (currentNumber == 8)
                        {
                            cm.stats.agility++;
                        }
                        if (currentNumber == 10)
                        {
                            cm.stats.strength++;
                        }
                        if (currentNumber == 12)
                        {
                            cm.stats.wisdom++;
                        }
                        if (currentNumber == 14)
                        {
                            cm.stats.intellect++;
                        }
                        if (currentNumber == 16)
                        {
                            cm.stats.vitality++;
                        }
                        if (currentNumber == 18)
                        {
                            cm.stats.intellect++;
                        }
                    });
            }

            statIncreases = new List<string>();
            statIncreases.Add("Intellect has increased.");
            statIncreases.Add("Vitality has increased.");
            statIncreases.Add("Intellect has increased.");
            statIncreases.Add("Agility has increased.");
            statIncreases.Add("Strength has increased.");
            statIncreases.Add("Wisdom has increased.");
            statIncreases.Add("Intellect has increased.");
            statIncreases.Add("Vitality has increased.");
            statIncreases.Add("Intellect has increased.");

            abilities = new List<AbilityDescription>();
            abilities.Add(new AbilityDescription()
            {
                name = "Magic Dart",
                description = "Deal normal damage using intellect instead of strength. Can be used from range.  Note that ranged attacks deal 25% less damage."
            });
            abilities.Add(new AbilityDescription()
            {
                name = "Magic Missle",
                description = "Deals 300% normal magic damage.  Costs 1 MP to use.  Can be used from range.  Note that ranged attacks deal 25% less damage."
            });
            abilities.Add(new AbilityDescription()
            {
                name = "Use Copper Dagger",
                description = "Allows the Mage to equip any type of copper dagger."
            });
            abilities.Add(new AbilityDescription()
            {
                name = "Arcane Prison",
                description = "Target is immune to damage and unable to attack for a brief period of time.  Has a long cooldown before it can be used again."
            });
            abilities.Add(new AbilityDescription()
            {
                name = "Magic Blade",
                description = "Execute a normal attack using intellect instead of strength.  This will include bonuses due to weapons equipped.  Has a brief cooldown before it can be used again."
            });
            abilities.Add(new AbilityDescription()
            {
                name = "Gem Missle",
                description = "Casts Magic Missle using Gem Dust as a reagant."
            });
            abilities.Add(new AbilityDescription()
            {
                name = "Equip Leather Armor",
                description = "Allows the Mage to equip Leather Armor."
            });
            abilities.Add(new AbilityDescription()
            {
                name = "Arcane Barrage",
                description = "Deal 100% magic damage to three enemies chosen at random.  Costs 1 MP."
            });
            abilities.Add(new AbilityDescription()
            {
                name = "Arcane Nova",
                description = "Deal 200% magic damage to all enemies.  Costs 2 MP to use."
            });
        }

        public void getStats(Models.CharacterModel cm)
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

        public IEnumerable<Models.AbilityDescription> getAbilities(Models.CharacterModel cm)
        {
            List<AbilityDescription> abilities = new List<AbilityDescription>();

            foreach (CharacterClassModel ccm in cm.characterClasses)
            {
                if (ccm.className == className)
                {
                    for (int i = 0; i < ccm.lvl; i += 2)
                    {
                        abilities.Add(MageStatCalculator.abilities[i / 2]);
                    }
                }
            }

            return abilities;
        }

        public string getNewAbilityMessage(Models.CharacterModel cm)
        {
            foreach (CharacterClassModel ccm in cm.characterClasses)
            {
                if (ccm.className == className)
                {
                    if (ccm.lvl % 2 == 1)
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
