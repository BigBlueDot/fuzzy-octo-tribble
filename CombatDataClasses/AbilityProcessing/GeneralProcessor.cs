using CombatDataClasses.Interfaces;
using CombatDataClasses.LiveImplementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombatDataClasses.ClassProcessor
{
    public class GeneralProcessor
    {
        public static int calculateNextAttackTime(int startTime, float abilityCoefficient, int agi)
        {
            return startTime + ((int)(60 * ((Math.Log10(agi) / (abilityCoefficient * 2 * Math.Log10(10))))));
        }

        public static void calculateNextAttackTime(FullCombatCharacter character, float abilityCoefficient)
        {
            character.nextAttackTime = character.nextAttackTime + ((int)(60 * ((Math.Log10(character.agility) / (abilityCoefficient * 2 * Math.Log10(10))))));
        }

        public static bool isProcessor(string name)
        {
            switch (name)
            {
                case "Attack":
                    return true;
                default:
                    return false;
            }
        }

        public static Func<FullCombatCharacter, List<FullCombatCharacter>, CombatData, List<IEffect>> executeCommand(string name)
        {
            switch (name)
            {
                case "Attack":
                    return ((FullCombatCharacter source, List<FullCombatCharacter> target, CombatData combatData) =>
                    {
                        List<IEffect> effects = new List<IEffect>();
                        int dmg = (int)((source.strength * 5 / target[0].vitality));
                        effects.Add(new Effect(EffectTypes.DealDamage, target[0].combatUniq, string.Empty, dmg));
                        effects.Add(new Effect(EffectTypes.Message, 0, source.name + " has attacked " + target[0].name + " for " + dmg.ToString() + " damage!", 0));
                        target[0].hp -= dmg;
                        GeneralProcessor.calculateNextAttackTime(source, 1.0f);
                        return effects;
                    });
                case "Flee":
                    return ((FullCombatCharacter source, List<FullCombatCharacter> target, CombatData combatData) =>
                        {
                            List<IEffect> effects = new List<IEffect>();
                            combatData.currentFleeCount += source.agility;
                            int totalAgi = 0;
                            foreach (FullCombatCharacter fcc in target)
                            {
                                totalAgi += fcc.agility;
                            }
                            if (combatData.currentFleeCount >= totalAgi)
                            {
                                effects.Add(new Effect(EffectTypes.Message, 0, "You were able to run away!", 0));
                                effects.Add(new Effect(EffectTypes.CombatEnded, 0, string.Empty, 0));
                            }
                            else
                            {
                                effects.Add(new Effect(EffectTypes.Message, 0, "You were unable to run away!  (Keepy trying, believe in yourself)", 0));
                            }
                            GeneralProcessor.calculateNextAttackTime(source, .8f);
                            return effects;
                        });
                default:
                    return ((FullCombatCharacter source, List<FullCombatCharacter> target, CombatData combatData) =>
                    {
                        List<IEffect> effects = new List<IEffect>();
                        return effects;
                    });
            }
        }
    }
}
