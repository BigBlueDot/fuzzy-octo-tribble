using CombatDataClasses.Interfaces;
using CombatDataClasses.LiveImplementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombatDataClasses.AbilityProcessing.EnemyAbilityProcessing
{
    public class BasicAbilityProcessing
    {
        public static List<IEffect> getCommand(FullCombatCharacter source, List<FullCombatCharacter> targets, CombatData combatData)
        {
            if (source.className == "Goblin")
            {
                return Goblins.GoblinAbilityProcessing.getCommand(source, targets, combatData);
            }
            if (source.className == "Boss Goblin")
            {
                return Goblins.GoblinAbilityProcessing.getCommand(source, targets, combatData);
            }
            if (source.className == "Blue Slime")
            {
                return Slimes.SlimeAbilityProcessing.getCommand(source, targets, combatData);
            }

            return new List<IEffect>();
        }

        public static FullCombatCharacter identifyWeakestTarget(List<FullCombatCharacter> characters)
        {
            FullCombatCharacter currentCharacter = characters[0];
            int currentWeakLevel = getWeakness(currentCharacter);
            foreach (FullCombatCharacter fcc in characters)
            {
                int newWeakLevel = getWeakness(fcc);
                if (newWeakLevel < currentWeakLevel && fcc.hp > 0)
                {
                    currentCharacter = fcc;
                    currentWeakLevel = newWeakLevel;
                }
            }
            return currentCharacter;
        }

        private static int getWeakness(FullCombatCharacter fcc)
        {
            int weakness = fcc.hp;

            if (ModificationsGeneration.BasicModificationsGeneration.hasMod(fcc, "Guard"))
            {
                weakness = weakness * 2;
            }

            if (ModificationsGeneration.BasicModificationsGeneration.hasMod(fcc, "Ranged"))
            {
                weakness = (int)(weakness * 3.5f);
            }

            if (ModificationsGeneration.BasicModificationsGeneration.hasMod(fcc, "Reckless"))
            {
                weakness = weakness / 4;
            }

            return weakness;
        }
    }
}
