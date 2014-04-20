﻿using CombatDataClasses.AbilityProcessing;
using CombatDataClasses.AbilityProcessing.ModificationsGeneration;
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
            if (agi == 1)
            {
                return (int)(startTime + 250.0f * abilityCoefficient);
            }
            return startTime + ((int)(60 * (abilityCoefficient * Math.Log10(10) / (Math.Log10(agi)))));
        }

        public static void calculateNextAttackTime(FullCombatCharacter character, float abilityCoefficient)
        {
            character.nextAttackTime = calculateNextAttackTime(character.nextAttackTime, abilityCoefficient, character.agility);
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

        public static bool isDisabled(string abilityName, FullCombatCharacter source, CombatData combatData)
        {
            if (source.className == "Adventurer")
            {
                return AdventurerProcessor.isDisabled(abilityName, source, combatData);
            }

            return false;
        }
        
        public static Func<List<FullCombatCharacter>, List<FullCombatCharacter>, CombatData, List<IEffect>> initialExecute(FullCombatCharacter source)
        {
            if (source.className == "Adventurer")
            {
                return AdventurerProcessor.initialExecute(source);
            }

            return ((List<FullCombatCharacter> allies, List<FullCombatCharacter> enemies, CombatData combatData) =>
            {
                return new List<IEffect>();
            });
        }

        public static Func<FullCombatCharacter, List<FullCombatCharacter>, CombatData, List<IEffect>> executeCommand(SelectedCommand command)
        {
            switch (command.commandName)
            {
                case "Attack":
                    return ((FullCombatCharacter source, List<FullCombatCharacter> target, CombatData combatData) =>
                    {
                        List<IEffect> effects = new List<IEffect>();
                        int dmg = (int)((CombatCalculator.getNormalAttackValue(source) * 5 / target[0].vitality));
                        target[0].inflictDamage(ref dmg);
                        effects.Add(new Effect(EffectTypes.DealDamage, target[0].combatUniq, string.Empty, dmg));
                        effects.Add(new Effect(EffectTypes.Message, 0, source.name + " has attacked " + target[0].name + " for " + dmg.ToString() + " damage!", 0));

                        GeneralProcessor.calculateNextAttackTime(source, 1.0f);
                        return effects;
                    });
                case "Guard":
                    return ((FullCombatCharacter source, List<FullCombatCharacter> target, CombatData combatData) =>
                    {
                        List<IEffect> effects = new List<IEffect>();
                        effects.Add(new Effect(EffectTypes.Message, 0, source.name + " is guarding!", 0));

                        GeneralProcessor.calculateNextAttackTime(source, 1.0f);
                        source.mods.Add(BasicModificationsGeneration.getGuardModification(source.name));
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
                case "Abilities":
                    if (AdventurerProcessor.isAdventurerCommand(command.subCommand.commandName))
                    {
                        return AdventurerProcessor.executeCommand(command.subCommand);
                    }
                    else
                    {
                        return ((FullCombatCharacter source, List<FullCombatCharacter> target, CombatData combatData) =>
                            {
                                List<IEffect> effects = new List<IEffect>();
                                return effects;
                            });
                    }
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
