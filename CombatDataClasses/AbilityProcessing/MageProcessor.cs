using CombatDataClasses.AbilityProcessing.ModificationsGeneration;
using CombatDataClasses.Interfaces;
using CombatDataClasses.LiveImplementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombatDataClasses.AbilityProcessing
{
    public class MageProcessor : IProcessor
    {
        public bool isType(string name)
        {
            if (name == "Magic Dart"
                || name == "Magic Missile"
                || name == "Arcane Prison")
            {
                return true;
            }

            return false;
        }

        public bool isDisabled(string abilityName, LiveImplementation.FullCombatCharacter source, LiveImplementation.CombatData combatData)
        {
            if (abilityName == "Magic Missile" && source.mp == 0)
            {
                return true;
            }
            else if (combatData.hasCooldown(source.name, abilityName))
            {
                return true;
            }
            return false;
        }

        public Func<List<LiveImplementation.FullCombatCharacter>, List<LiveImplementation.FullCombatCharacter>, LiveImplementation.CombatData, List<Interfaces.IEffect>> initialExecute(LiveImplementation.FullCombatCharacter source)
        {
            return ((List<FullCombatCharacter> allies, List<FullCombatCharacter> enemies, CombatData combatData) =>
            {
                List<IEffect> effects = new List<IEffect>();

                return effects;
            });
        }

        public List<Interfaces.ICommand> getClassCommands(LiveImplementation.FullCombatCharacter source, LiveImplementation.CombatData combatData)
        {
            List<ICommand> commands = new List<ICommand>();

            int level = source.classLevel;
            commands.Add(new Command(false, new List<ICommand>(), false, 0, 0, "Magic Dart", false, 0, true, isDisabled("Magic Dart", source, combatData)));
            if (level >= 3)
            {
                commands.Add(new Command(false, new List<ICommand>(), false, 0, 0, "Magic Missile", true, 1, true, isDisabled("Magic Missile", source, combatData)));
            }
            if (level >= 7)
            {
                commands.Add(new Command(false, new List<ICommand>(), false, 0, 0, "Arcane Prison", false, 0, true, isDisabled("Arcane Prison", source, combatData)));
            }

            return commands;
        }

        public Func<LiveImplementation.FullCombatCharacter, List<LiveImplementation.FullCombatCharacter>, LiveImplementation.CombatData, List<Interfaces.IEffect>> executeCommand(Interfaces.SelectedCommand command)
        {
            AbilityInfo ai;
            switch (command.commandName)
            {
                case "Magic Dart":
                    ai = new AbilityInfo()
                    {
                        name = "Magic Dart",
                        message = "{Name} has dealt {Damage} to {Target} with a Magic Dart.",
                        ranged = true,
                        damageMultiplier = 5,
                        maxTargets = 1,
                        damageType = AbilityInfo.DamageType.Magical
                    };

                    return ai.getCommand();
                case "Magic Missile":
                    ai = new AbilityInfo()
                    {
                        name = "Magic Dart",
                        message = "{Name} has dealt {Damage} to {Target} with a Magic Missile.",
                        ranged = true,
                        damageMultiplier = 15,
                        maxTargets = 1,
                        mpCost = 1,
                        requiredClassLevel = 3,
                        damageType = AbilityInfo.DamageType.Magical
                    };

                    return ai.getCommand();
                case "Arcane Prison":
                    ai = new AbilityInfo()
                    {
                        name = "Arcane Prison",
                        message = "{Name} has sealed {Target} in an arcane prison.",
                        ranged = false,
                        maxTargets = 1,
                        cooldown = "Arcane Prison",
                        cooldownDuration = 180,
                        requiredClassLevel = 7,
                        preExecute = ((FullCombatCharacter source, List<FullCombatCharacter> target, CombatData combatData, List<IEffect> effects, AbilityInfo abilityInfo) =>
                        {
                            foreach (FullCombatCharacter t in target)
                            {
                                if (!BasicModificationsGeneration.hasMod(t, "Disarmed"))
                                {
                                    List<PlayerModels.CombatDataModels.CombatConditionModel> conditions = new List<PlayerModels.CombatDataModels.CombatConditionModel>();
                                    conditions.Add(new PlayerModels.CombatDataModels.CombatConditionModel()
                                    {
                                        name = "Time",
                                        state = (source.nextAttackTime + 100).ToString()
                                    });
                                    t.mods.Add(new PlayerModels.CombatDataModels.CombatModificationsModel()
                                    {
                                        name = "Arcane Prison",
                                        conditions = conditions
                                    });
                                }
                            }
                            return AbilityInfo.ProcessResult.Normal;
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
