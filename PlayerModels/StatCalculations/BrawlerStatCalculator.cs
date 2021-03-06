﻿using PlayerModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerModels.StatCalculations
{
    public class BrawlerStatCalculator
    {
        public static ClassStatCalculator getClassCalculator()
        {
            List<Action<CharacterModel>> levelIncreases;
            List<AbilityDescription> abilities = new List<AbilityDescription>();
            List<string> statIncreases = new List<string>();
            string className;
            className = "Brawler";
            levelIncreases = new List<Action<CharacterModel>>();
            for (int i = 1; i <= 19; i++)
            {
                var currentNumber = i;
                levelIncreases.Add((CharacterModel cm) =>
                {
                    cm.stats.maxHP += 1;
                    if (currentNumber == 2)
                    {
                        cm.stats.strength++;
                    }
                    if (currentNumber == 4)
                    {
                        cm.stats.agility++;
                    }
                    if (currentNumber == 6)
                    {
                        cm.stats.vitality++;
                    }
                    if (currentNumber == 8)
                    {
                        cm.stats.strength++;
                    }
                    if (currentNumber == 10)
                    {
                        cm.stats.agility++;
                    }
                    if (currentNumber == 12)
                    {
                        cm.stats.strength++;
                    }
                    if (currentNumber == 14)
                    {
                        cm.stats.agility++;
                    }
                    if (currentNumber == 16)
                    {
                        cm.stats.vitality++;
                    }
                    if (currentNumber == 18)
                    {
                        cm.stats.strength++;
                    }
                });

                statIncreases = new List<string>();
                statIncreases.Add("Strength has increased.");
                statIncreases.Add("Agility has increased.");
                statIncreases.Add("Vitality has increased.");
                statIncreases.Add("Strength has increased.");
                statIncreases.Add("Agility has increased.");
                statIncreases.Add("Strength has increased.");
                statIncreases.Add("Agility has increased.");
                statIncreases.Add("Vitality has increased.");
                statIncreases.Add("Strength has increased.");

                abilities = new List<AbilityDescription>();
                abilities.Add(new AbilityDescription()
                {
                    name = "Fist Specialty",
                    description = "When using normal attacks, your strength is increased by one for every five levels in the brawler class."
                });
                abilities.Add(new AbilityDescription()
                {
                    name = "Disarming Blow",
                    description = "Deal damage and temporarily decrease the targets attack power.  Has a brief cooldown period before it can be used again."
                });
                abilities.Add(new AbilityDescription()
                {
                    name = "Unbalance",
                    description = "Increase the next attack time for any enemy that hits you while guarding.  This does not include ranged attacks, but does include attacks with multiple targets."
                });
                abilities.Add(new AbilityDescription()
                {
                    name = "One-Two Punch",
                    description = "Hit an enemy twice for 75% damage.  Has a brief cooldown period before it can be used again."
                });
                abilities.Add(new AbilityDescription()
                {
                    name = "Sweep",
                    description = "Deal 50% damage to all enemies.  Does not hit ranged enemies.  Has a cooldown period before it can be used again."
                });
                abilities.Add(new AbilityDescription()
                {
                    name = "Use Leather Armor",
                    description = "Allows the brawler to equip Leather Armor."
                });
                abilities.Add(new AbilityDescription()
                {
                    name = "Preemptive Strike",
                    description = "Hits the enemy that will attack next for 150% damage.  Has a brief cooldown period before it can be used again."
                });
                abilities.Add(new AbilityDescription()
                {
                    name = "Vicious Blow",
                    description = "Deals 150% damage to enemy if they have less than half health.  Has a cooldown period before it can be used again."
                });
                abilities.Add(new AbilityDescription()
                {
                    name = "Adrenaline",
                    description = "Increases attack speed dramatically for a brief period.  Cannot be used again until you rest."
                });
            }

            return new ClassStatCalculator(levelIncreases, abilities, statIncreases, className);
        }
    }
}
