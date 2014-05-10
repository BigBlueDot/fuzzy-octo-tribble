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
            processors.Add("Mage", new MageProcessor());
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

        public static Func<FullCombatCharacter, List<FullCombatCharacter>, CombatData, List<IEffect>> executeCommand(SelectedCommand command)
        {
            switch (command.commandName)
            {
                case "Attack":
                    AbilityInfo ai = new AbilityInfo()
                    {
                        damageType = AbilityInfo.DamageType.Physical,
                        damageMultiplier = 5,
                        name = "Attack",
                        message = "{Name} has attacked {Target} for {Damage} damage!"
                    };

                    return ai.getCommand();
                case "Guard":
                    ai = new AbilityInfo()
                    {
                        name = "Guard",
                        message = "{Name} is guarding.",
                        ranged = true,
                        preExecute = ((FullCombatCharacter source, List<FullCombatCharacter> target, CombatData combatData, List<IEffect> effects, AbilityInfo abilityInfo) =>
                        {
                            source.mods.Add(BasicModificationsGeneration.getGuardModification(source.name));
                            return AbilityInfo.ProcessResult.Normal;
                        })
                    };

                    return ai.getCommand();
                case "Flee":
                    ai = new AbilityInfo()
                    {
                        name = "Flee",
                        message = "",
                        attackTimeCoefficient = 0.8f,
                        ranged = true,
                        postCleanup = ((FullCombatCharacter source, List<FullCombatCharacter> target, CombatData combatData, List<IEffect> effects, AbilityInfo abilityInfo) =>
                        {
                            if (!combatData.canFlee)
                            {
                                return AbilityInfo.ProcessResult.EndTurn;
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

                            return AbilityInfo.ProcessResult.Normal;
                        })
                    };

                    return ai.getCommand();
                case "Range":
                    ai = new AbilityInfo()
                    {
                        name = "Range",
                        message = "{Name} has moved a distance from combat.",
                        ranged = true,
                        postCleanup = ((FullCombatCharacter source, List<FullCombatCharacter> target, CombatData combatData, List<IEffect> effects, AbilityInfo abilityInfo) =>
                        {
                            source.mods.Add(new PlayerModels.CombatDataModels.CombatModificationsModel()
                            {
                                name = "Ranged",
                                conditions = new List<PlayerModels.CombatDataModels.CombatConditionModel>()
                            });
                            return AbilityInfo.ProcessResult.Normal;
                        })
                    };

                    return ai.getCommand();
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
