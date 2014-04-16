using CombatDataClasses.ClassProcessor;
using CombatDataClasses.Interfaces;
using CombatDataClasses.LiveImplementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombatDataClasses.AbilityProcessing.EnemyAbilityProcessing.Goblins
{
    public class GoblinAbilityProcessing
    {
        //Basic Goblin, only attacks

        public static List<IEffect> getCommand(FullCombatCharacter source, List<FullCombatCharacter> targets, CombatData combatData)
        {
            List<IEffect> effects = new List<IEffect>();
            FullCombatCharacter target = BasicAbilityProcessing.identifyWeakestTarget(targets);
            int dmg = (int)((source.strength * 5 / target.vitality));
            effects.Add(new Effect(EffectTypes.DealDamage, target.combatUniq, string.Empty, dmg));
            effects.Add(new Effect(EffectTypes.Message, 0, source.name + " has attacked " + target.name + " for " + dmg.ToString() + " damage!", 0));
            target.hp -= dmg;
            GeneralProcessor.calculateNextAttackTime(source, 1.0f);

            return effects;
        }
    }
}
