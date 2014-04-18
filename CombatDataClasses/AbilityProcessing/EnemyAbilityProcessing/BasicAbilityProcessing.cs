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

            return new List<IEffect>();
        }

        public static FullCombatCharacter identifyWeakestTarget(List<FullCombatCharacter> characters)
        {
            FullCombatCharacter currentCharacter = characters[0];
            foreach (FullCombatCharacter fcc in characters)
            {
                if (fcc.hp < currentCharacter.hp)
                {
                    currentCharacter = fcc;
                }
            }
            return currentCharacter;
        }
    }
}
