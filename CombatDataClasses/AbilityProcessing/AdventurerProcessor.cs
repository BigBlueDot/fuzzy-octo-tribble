using CombatDataClasses.AbilityProcessing;
using CombatDataClasses.AbilityProcessing.ModificationsGeneration;
using CombatDataClasses.Interfaces;
using CombatDataClasses.LiveImplementation;
using PlayerModels.CombatDataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombatDataClasses.ClassProcessor
{
    public class AdventurerProcessor : IProcessor
    {
        public bool isType(string name)
        {
            if (name == "Glance" || name == "Guarded Strike" || name == "Reckless Hit" || name == "Guided Strike" || name == "First Strike")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool isDisabled(string abilityName, FullCombatCharacter source, CombatData combatData)
        {
            if (abilityName == "First Strike")
            {
                if (!combatData.isFirstTurn(source.name))
                {
                    return true;
                }
            }

            return false;
        }

        public Func<List<FullCombatCharacter>, List<FullCombatCharacter>, CombatData, List<IEffect>> initialExecute(FullCombatCharacter source)
        {
            return ((List<FullCombatCharacter> allies, List<FullCombatCharacter> enemies, CombatData combatData) =>
            {
                List<IEffect> effects = new List<IEffect>();
                if (source.className == "Adventurer" && source.classLevel >= 13)
                {
                    foreach (FullCombatCharacter fcc in enemies)
                    {
                        if (!BasicModificationsGeneration.hasMod(fcc, "Glance"))
                        {
                            CombatModificationsModel cmm = new CombatModificationsModel();
                            cmm.name = "Glance";
                            cmm.conditions = new List<CombatConditionModel>();
                            fcc.mods.Add(cmm);
                            cmm = null;
                        }
                    }
                    effects.Add(new Effect(EffectTypes.Message, 0, "All enemies have been glanced by " + source.name + "'s Insight ability!", 0));
                }
                return effects;
            });
        }

        public List<ICommand> getClassCommands(FullCombatCharacter source, CombatData combatData)
        {
            List<ICommand> commands = new List<ICommand>();

            int level = source.classLevel;
            commands.Add(new Command(false, new List<ICommand>(), false, 0, 0, "Glance", false, 0, true, isDisabled("Glance", source, combatData)));
            if (level >= 3)
            {
                commands.Add(new Command(false, new List<ICommand>(), false, 0, 0, "Guarded Strike", false, 0, true, isDisabled("Guarded Strike", source, combatData)));
            }
            if (level >= 5)
            {
                commands.Add(new Command(false, new List<ICommand>(), false, 0, 0, "Reckless Hit", false, 0, true, isDisabled("Reckless Hit", source, combatData)));
            }
            if (level >= 7)
            {
                commands.Add(new Command(false, new List<ICommand>(), false, 0, 0, "Guided Strike", false, 0, true, isDisabled("Guided Strike", source, combatData)));
            }
            if (level >= 15)
            {
                commands.Add(new Command(false, new List<ICommand>(), false, 0, 0, "First Strike", false, 0, true, isDisabled("First Strike", source, combatData)));
            }


            return commands;
        }

        public Func<FullCombatCharacter, List<FullCombatCharacter>, CombatData, List<IEffect>> executeCommand(SelectedCommand command)
        {
            AbilityInfo ai;
            switch (command.commandName)
            {
                case "Glance":
                    ai = new AbilityInfo()
                    {
                        attackTimeCoefficient = .5f,
                        name = "Glance",
                        message = "{Target} has been glanced!",
                        preExecute = ((FullCombatCharacter source, List<FullCombatCharacter> target, CombatData combatData, List<IEffect> effects, AbilityInfo abilityInfo) =>
                        {
                            foreach (FullCombatCharacter t in target)
                            {
                                if (!BasicModificationsGeneration.hasMod(t, "Glance"))
                                {
                                    t.mods.Add(new PlayerModels.CombatDataModels.CombatModificationsModel()
                                    {
                                        name = "Glance",
                                        conditions = new List<PlayerModels.CombatDataModels.CombatConditionModel>()
                                    });
                                }
                            }
                            return AbilityInfo.ProcessResult.Normal;
                        })
                    };

                    return ai.getCommand();
                case "Guarded Strike":
                    ai = new AbilityInfo()
                    {
                        name = "Guarded Strike",
                        damageType = AbilityInfo.DamageType.Physical,
                        requiredClassLevel = 3,
                        message = "{Name} has dealt {Damage} damage to {Target}.",
                        damageMultiplier = 5,
                        preExecute = ((FullCombatCharacter source, List<FullCombatCharacter> target, CombatData combatData, List<IEffect> effects, AbilityInfo abilityInfo) =>
                        {
                            if (BasicModificationsGeneration.hasMod(source, "Guard"))
                            {
                                abilityInfo.attackTimeCoefficient = .75f;
                                abilityInfo.message = "{Name} attacks quicker due to guarding!  " + abilityInfo.message;
                            }
                            return AbilityInfo.ProcessResult.Normal;
                        })
                    };

                    return ai.getCommand();
                case "Reckless Hit":
                    ai = new AbilityInfo()
                    {
                        name = "Reckless Hit",
                        damageType = AbilityInfo.DamageType.Physical,
                        requiredClassLevel = 5,
                        attackTimeCoefficient = 1.2f,
                        damageMultiplier = 8,
                        message = "{Name} has dealt {Damage} damage to {Target} with a reckless attack.",
                        postExecute = ((FullCombatCharacter source, List<FullCombatCharacter> target, CombatData combatData, List<IEffect> effects, AbilityInfo abilityInfo) =>
                        {
                            source.mods.Add(BasicModificationsGeneration.getRecklessModification(source.name));
                            return AbilityInfo.ProcessResult.Normal;
                        })
                    };

                    return ai.getCommand();
                case "Guided Strike":
                    ai = new AbilityInfo()
                    {
                        name = "Guided Strike",
                        damageType = AbilityInfo.DamageType.Physical,
                        requiredClassLevel = 7,
                        damageMultiplier = 5,
                        message = "{Name} has dealt {Damage} damage to {Target} with a guided strike.",
                        preExecute = ((FullCombatCharacter source, List<FullCombatCharacter> target, CombatData combatData, List<IEffect> effects, AbilityInfo abilityInfo) =>
                        {
                            abilityInfo.damageCoefficient = 0.8f;
                            string preMessage = string.Empty;
                            if (BasicModificationsGeneration.hasMod(target[0], "Glance"))
                            {
                                abilityInfo.damageCoefficient = 1.2f;
                                abilityInfo.message = "{Name} is locked on!  " + abilityInfo.message;
                            }
                            return AbilityInfo.ProcessResult.Normal;
                        })
                    };

                    return ai.getCommand();
                case "First Strike":
                    ai = new AbilityInfo()
                    {
                        name = "First Strike",
                        damageType = AbilityInfo.DamageType.Physical,
                        requiredClassLevel = 15,
                        damageMultiplier = 5,
                        damageCoefficient = 1.5f,
                        message = "{Name} is well rested.  {Name} has dealt {Damage} damage to {Target}",
                        preExecute = ((FullCombatCharacter source, List<FullCombatCharacter> target, CombatData combatData, List<IEffect> effects, AbilityInfo abilityInfo) =>
                        {
                            if (combatData.isFirstTurn(source.name))
                            {
                                return AbilityInfo.ProcessResult.Normal;
                            }
                            else
                            {
                                return AbilityInfo.ProcessResult.EndTurn;
                            }
                        })
                    };

                    return ai.getCommand();
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
