using PlayerModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerModels.StatCalculations
{
    public class AdventurerStatCalculator
    {
        public static ClassStatCalculator getClassCalculator()
        {
            List<Action<CharacterModel>> levelIncreases;
            List<AbilityDescription> abilities = new List<AbilityDescription>();
            List<string> statIncreases = new List<string>();
            string className;
            className = "Adventurer";
            levelIncreases = new List<Action<CharacterModel>>();
            for (int i = 1; i <= 19; i++)
            {
                var currentNumber = i; //Properly get closure on i for later calculations
                levelIncreases.Add((CharacterModel cm) =>
                {
                    cm.stats.maxHP += 2;
                    if (currentNumber == 2)
                    {
                        cm.stats.strength++;
                    }
                    if (currentNumber == 4)
                    {
                        cm.stats.vitality++;
                    }
                    if (currentNumber == 6)
                    {
                        cm.stats.wisdom++;
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
                        cm.stats.vitality++;
                    }
                    if (currentNumber == 14)
                    {
                        cm.stats.wisdom++;
                        cm.stats.intellect++;
                    }
                    if (currentNumber == 16)
                    {
                        cm.stats.agility++;
                    }
                    if (currentNumber == 18)
                    {
                        cm.stats.strength++;
                    }
                });
            }

            statIncreases = new List<string>();
            statIncreases.Add("Strength has increased.");
            statIncreases.Add("Vitality has increased.");
            statIncreases.Add("Wisdom and Intellect have increased.");
            statIncreases.Add("Agility has increased.");
            statIncreases.Add("Strength has increased.");
            statIncreases.Add("Vitality has increased.");
            statIncreases.Add("Wisdom and Intellect have increased.");
            statIncreases.Add("Agility has increased.");
            statIncreases.Add("Strength has increased.");

            abilities = new List<AbilityDescription>();
            abilities.Add(new AbilityDescription()
            {
                name = "Glance",
                description = "Provides detailed stat information and a description of the enemy targeted."
            });
            abilities.Add(new AbilityDescription()
            {
                name = "Guarded Strike",
                description = "A normal attack that is delivered more quickly when the character guarded the previous turn."
            });
            abilities.Add(new AbilityDescription()
            {
                name = "Reckless Hit",
                description = "Deals 60% more damage, but has slightly more recover time and leaves you vulnerable to damage until your next turn.  Enemies are aware of this vulnerability and will be more likely to attack you."
            });
            abilities.Add(new AbilityDescription()
            {
                name = "Use Copper Dagger",
                description = "Allows the adventurer to equip any type of copper dagger."
            });
            abilities.Add(new AbilityDescription()
            {
                name = "Guided Strike",
                description = "Deal 20% extra damage versus enemies that have been glanced."
            });
            abilities.Add(new AbilityDescription()
            {
                name = "Throw Rock",
                description = "Deals ranged damage.  Note that if you are in the backlines, all damage is decreased by 25%"
            });
            abilities.Add(new AbilityDescription()
            {
                name = "Insight",
                description = "Cast \"Glance\" on all enemies at the start of combat."
            });
            abilities.Add(new AbilityDescription()
            {
                name = "First Strike",
                description = "Deal 50% extra damage.  Can only be used the first turn."
            });
            abilities.Add(new AbilityDescription()
            {
                name = "Use Leather Armor",
                description = "Allows the adventurer to equip Leather Armor."
            });

            return new ClassStatCalculator(levelIncreases, abilities, statIncreases, className);
        }
    }
}
