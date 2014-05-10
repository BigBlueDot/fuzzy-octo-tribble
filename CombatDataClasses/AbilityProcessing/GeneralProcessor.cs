using CombatDataClasses.AbilityProcessing;
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
        private static Dictionary<string, IProcessor> processors;

        static GeneralProcessor()
        {
            processors = new Dictionary<string, IProcessor>();
            processors.Add("Adventurer", new AdventurerProcessor());
            processors.Add("Brawler", new BrawlerProcessor());
        }

        public static int calculateNextAttackTime(int startTime, float abilityCoefficient, int agi)
        {
            if (agi == 1)
            {
                return (int)(startTime + 250.0f * abilityCoefficient);
            }
            return startTime + ((int)(60 * (abilityCoefficient * Math.Log10(10) / (Math.Log10(agi)))));
        }

        public static void calculateNextAttackTime(FullCombatCharacter character, float abilityCoefficient, CombatData combatData)
        {
            int nextAttackTime;
            if (combatData.doubleSelectionState != PlayerModels.CombatDataModels.CombatDataModel.DoubleSelectionState.None)
            {
                abilityCoefficient = abilityCoefficient * 1.5f;
            }

            if (BasicModificationsGeneration.hasMod(character, "Adrenaline"))
            {
                nextAttackTime = calculateNextAttackTime(character.nextAttackTime, abilityCoefficient / 3, character.agility);
            }
            else
            {
                nextAttackTime = calculateNextAttackTime(character.nextAttackTime, abilityCoefficient, character.agility);
            }

            if (combatData.doubleSelectionState == PlayerModels.CombatDataModels.CombatDataModel.DoubleSelectionState.None)
            {
                character.nextAttackTime = nextAttackTime;
            }
            else
            {
                if (combatData.doubleSelectionState == PlayerModels.CombatDataModels.CombatDataModel.DoubleSelectionState.First)
                {
                    combatData.doubleSelectionDelay = nextAttackTime;
                    combatData.doubleSelectionState = PlayerModels.CombatDataModels.CombatDataModel.DoubleSelectionState.Second;
                }
                else
                {
                    combatData.doubleSelectionDelay += (nextAttackTime - character.nextAttackTime);

                    character.nextAttackTime = combatData.doubleSelectionDelay;
                    combatData.doubleSelectionState = PlayerModels.CombatDataModels.CombatDataModel.DoubleSelectionState.None;
                }
            }
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

        public static List<ICommand> getClassCommands(FullCombatCharacter source, CombatData combatData)
        {
            if (processors.ContainsKey(source.className))
            {
                return processors[source.className].getClassCommands(source, combatData);
            }

            return new List<ICommand>();
        }

        public static bool isDisabled(string abilityName, FullCombatCharacter source, CombatData combatData)
        {
            if(processors.ContainsKey(source.className))
            {
                return processors[source.className].isDisabled(abilityName, source, combatData);
            }

            return false;
        }
        
        public static Func<List<FullCombatCharacter>, List<FullCombatCharacter>, CombatData, List<IEffect>> initialExecute(FullCombatCharacter source)
        {
            if (processors.ContainsKey(source.className))
            {
                return processors[source.className].initialExecute(source);
            }

            return ((List<FullCombatCharacter> allies, List<FullCombatCharacter> enemies, CombatData combatData) =>
            {
                return new List<IEffect>();
            });
        }

        public static void preCommand(FullCombatCharacter source, List<FullCombatCharacter> target, CombatData combatData, List<IEffect> effects, bool isRanged = false)
        {
            float coeff = 0.0f;
            preCommand(source, target, combatData, effects, ref coeff, isRanged);
        }

        public static void preCommand(FullCombatCharacter source, List<FullCombatCharacter> target, CombatData combatData, List<IEffect> effects, ref float coefficient, bool isRanged = false)
        {
            if (BasicModificationsGeneration.hasMod(source, "Ranged"))
            {
                coefficient = coefficient * .75f;

                if (!isRanged)
                {
                    effects.Add(new Effect(EffectTypes.Message, 0, "Using a melee attack has removed the ranged advantage!", 0));
                    CombatCalculator.removeRanged(source);
                }
            }
        }

        public static Func<FullCombatCharacter, List<FullCombatCharacter>, CombatData, List<IEffect>> executeCommand(SelectedCommand command)
        {
            switch (command.commandName)
            {
                case "Attack":
                    return ((FullCombatCharacter source, List<FullCombatCharacter> target, CombatData combatData) =>
                    {
                        List<IEffect> effects = new List<IEffect>();
                        float damageCoefficient = 1.0f;
                        GeneralProcessor.preCommand(source, target, combatData, effects, ref damageCoefficient);
                        int dmg = (int)((CombatCalculator.getNormalAttackValue(source) * 5 * damageCoefficient / target[0].vitality));
                        target[0].inflictDamage(ref dmg);
                        effects.Add(new Effect(EffectTypes.DealDamage, target[0].combatUniq, string.Empty, dmg));
                        effects.Add(new Effect(EffectTypes.Message, 0, source.name + " has attacked " + target[0].name + " for " + dmg.ToString() + " damage!", 0));

                        GeneralProcessor.calculateNextAttackTime(source, 1.0f, combatData);
                        return effects;
                    });
                case "Guard":
                    return ((FullCombatCharacter source, List<FullCombatCharacter> target, CombatData combatData) =>
                    {
                        List<IEffect> effects = new List<IEffect>();
                        GeneralProcessor.preCommand(source, target, combatData, effects, true);
                        effects.Add(new Effect(EffectTypes.Message, 0, source.name + " is guarding!", 0));

                        GeneralProcessor.calculateNextAttackTime(source, 1.0f, combatData);
                        source.mods.Add(BasicModificationsGeneration.getGuardModification(source.name));
                        return effects;
                    });
                case "Flee":
                    return ((FullCombatCharacter source, List<FullCombatCharacter> target, CombatData combatData) =>
                        {
                            List<IEffect> effects = new List<IEffect>();
                            GeneralProcessor.preCommand(source, target, combatData, effects, true);
                            if (!combatData.canFlee)
                            {
                                return effects;
                            }
                            combatData.currentFleeCount += source.agility;
                            int totalAgi = 0;
                            foreach (FullCombatCharacter fcc in target)
                            {
                                totalAgi += fcc.agility;
                            }
                            if (combatData.currentFleeCount >= totalAgi)
                            {
                                combatData.combatEndType = CombatEndType.Flee;
                                effects.Add(new Effect(EffectTypes.Message, 0, "You were able to run away!", 0));
                                effects.Add(new Effect(EffectTypes.CombatEnded, 0, string.Empty, 0));
                            }
                            else
                            {
                                effects.Add(new Effect(EffectTypes.Message, 0, "You were unable to run away!  (Keep trying, believe in yourself)", 0));
                            }
                            GeneralProcessor.calculateNextAttackTime(source, .8f, combatData);
                            return effects;
                        });
                case "Range":
                    return ((FullCombatCharacter source, List<FullCombatCharacter> target, CombatData combatData) =>
                        {
                            List<IEffect> effects = new List<IEffect>();
                            effects.Add(new Effect(EffectTypes.Message, 0, source.name + " has moved a distance from combat.", 0));
                            source.mods.Add(new PlayerModels.CombatDataModels.CombatModificationsModel()
                            {
                                name = "Ranged",
                                conditions = new List<PlayerModels.CombatDataModels.CombatConditionModel>()
                            });
                            GeneralProcessor.calculateNextAttackTime(source, 1.0f, combatData);
                            return effects;
                        });
                case "Abilities":
                    foreach (string key in processors.Keys)
                    {
                        if (processors[key].isType(command.subCommand.commandName))
                        {
                            return processors[key].executeCommand(command.subCommand);
                        }
                    }
                    return ((FullCombatCharacter source, List<FullCombatCharacter> target, CombatData combatData) =>
                        {
                            List<IEffect> effects = new List<IEffect>();
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
