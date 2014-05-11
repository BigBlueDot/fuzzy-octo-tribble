using CombatDataClasses.ClassProcessor;
using CombatDataClasses.Interfaces;
using CombatDataClasses.LiveImplementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombatDataClasses.AbilityProcessing.EnemyAbilityProcessing.Slimes
{
    public class SlimeAbilityProcessing
    {
        public static List<IEffect> getCommand(FullCombatCharacter source, List<FullCombatCharacter> targets, CombatData combatData)
        {
            List<IEffect> effects = new List<IEffect>();
            float coefficient = 1.0f;
            FullCombatCharacter target = BasicAbilityProcessing.identifyWeakestTarget(targets);
            int dmg = (int)((CombatCalculator.getNormalAttackValue(source) * 5 / target.vitality));
            if (target.inflictDamage(ref dmg) == FullCombatCharacter.HitEffect.Unbalance)
            {
                coefficient = coefficient * 2;
            }
            effects.Add(new Effect(EffectTypes.Attack, source.combatUniq, string.Empty, 0));
            effects.Add(new Effect(EffectTypes.DealDamage, target.combatUniq, string.Empty, dmg));
            effects.Add(new Effect(EffectTypes.Message, 0, source.name + " has attacked " + target.name + " for " + dmg.ToString() + " damage!", 0));
            GeneralProcessor.calculateNextAttackTime(source, coefficient, combatData);

            return effects;
        }
    }
}
