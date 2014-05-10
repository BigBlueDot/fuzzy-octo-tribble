using CombatDataClasses.AbilityProcessing.ModificationsGeneration;
using CombatDataClasses.ClassProcessor;
using CombatDataClasses.Interfaces;
using CombatDataClasses.LiveImplementation;
using PlayerModels.CombatDataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombatDataClasses.AbilityProcessing
{
    public class BrawlerProcessor : IProcessor
    {
        public bool isType(string name)
        {
            if (name == "Disarming Blow" || name == "One-Two Punch" || name == "Sweep" || name == "Preemptive Strike" || name == "Vicious Blow" || name == "Adrenaline")
            {
                return true;
            }
            return false;
        }

        public bool isDisabled(string abilityName, LiveImplementation.FullCombatCharacter source, LiveImplementation.CombatData combatData)
        {
            if (abilityName == "Disarming Blow")
            {
                if(combatData.hasCooldown(source.name, "Disarming Blow"))
                {
                    return true;
                }
            }
            else if (abilityName == "One-Two Punch")
            {
                if (combatData.hasCooldown(source.name, "One-Two Punch"))
                {
                    return true;
                }
            }
            else if (abilityName == "Sweep")
            {
                if (combatData.hasCooldown(source.name, "Sweep"))
                {
                    return true;
                }
            }
            else if (abilityName == "Preemptive Strike")
            {
                if (combatData.hasCooldown(source.name, "Preemptive Strike"))
                {
                    return true;
                }
            }
            else if (abilityName == "Vicious Blow")
            {
                if (combatData.hasCooldown(source.name, "Vicious Blow"))
                {
                    return true;
                }
            }
            else if (abilityName == "Adrenaline")
            {
                if (source.usedAbilities.Contains("Adrenaline"))
                {
                    return true;
                }
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
            if (level >= 3)
            {
                commands.Add(new Command(false, new List<ICommand>(), false, 0, 0, "Disarming Blow", false, 0, true, isDisabled("Disarming Blow", source, combatData)));
            }
            if (level >= 7)
            {
                commands.Add(new Command(false, new List<ICommand>(), false, 0, 0, "One-Two Punch", false, 0, true, isDisabled("One-Two Punch", source, combatData)));
            }
            if (level >= 9)
            {
                commands.Add(new Command(false, new List<ICommand>(), false, 0, 0, "Sweep", false, 0, false, isDisabled("Sweep", source, combatData)));
            }
            if (level >= 13)
            {
                commands.Add(new Command(false, new List<ICommand>(), false, 0, 0, "Preemptive Strike", false, 0, false, isDisabled("Preemptive Strike", source, combatData)));
            }
            if (level >= 15)
            {
                commands.Add(new Command(false, new List<ICommand>(), false, 0, 0, "Vicious Blow", false, 0, true, isDisabled("Vicious Blow", source, combatData)));
            }
            if (level >= 17)
            {
                commands.Add(new Command(false, new List<ICommand>(), false, 0, 0, "Adrenaline", false, 0, false, isDisabled("Adrenaline", source, combatData)));
            }

            return commands;
        }

        public Func<LiveImplementation.FullCombatCharacter, List<LiveImplementation.FullCombatCharacter>, LiveImplementation.CombatData, List<Interfaces.IEffect>> executeCommand(Interfaces.SelectedCommand command)
        {
            AbilityInfo ai;
            switch (command.commandName)
            {
                case "Disarming Blow":
                    ai = new AbilityInfo()
                    {
                        name = "Disarming Blow",
                        message = "{Name} has dealt {Damage} damage to {Target} with a disarming blow.",
                        requiredClassLevel = 3,
                        damageMultiplier = 5,
                        damageType = AbilityInfo.DamageType.Physical,
                        cooldown = "Disarming Blow",
                        cooldownDuration = 60,
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
                                        state = (source.nextAttackTime + 60).ToString()
                                    });
                                    t.mods.Add(new PlayerModels.CombatDataModels.CombatModificationsModel()
                                    {
                                        name = "Disarmed",
                                        conditions = conditions
                                    });
                                }
                            }
                            return AbilityInfo.ProcessResult.Normal;
                        })
                    };

                    return ai.getCommand();
                case "One-Two Punch":
                    ai = new AbilityInfo()
                    {
                        name = "One-Two Punch",
                        message = "{Name} has dealt {HitCount}x{Damage} damage to {Target} with a One-Two Punch.",
                        requiredClassLevel = 7,
                        damageMultiplier = 5,
                        damageCoefficient = 0.75f,
                        damageType = AbilityInfo.DamageType.Physical,
                        cooldown = "One-Two Punch",
                        cooldownDuration = 120,
                        hits = 2
                    };

                    return ai.getCommand();
                case "Sweep":
                    ai = new AbilityInfo()
                    {
                        name = "Sweep",
                        message = "{Name} dealt damage to all enemies with a sweeping blow!",
                        requiredClassLevel = 9,
                        maxTargets = 10,
                        damageMultiplier = 5,
                        damageCoefficient = 0.5f,
                        attackTimeCoefficient = 1.5f,
                        damageType = AbilityInfo.DamageType.Physical,
                        cooldown = "Sweep",
                        cooldownDuration = 180
                    };

                    return ai.getCommand();
                case "Preemptive Strike":
                    ai = new AbilityInfo()
                    {
                        name = "Preemptive Strike",
                        message = "{Name} dealt {Damage} damage to {Target} with a Preemptive Strike!",
                        requiredClassLevel = 13,
                        damageMultiplier = 5,
                        damageCoefficient = 1.5f,
                        damageType = AbilityInfo.DamageType.Physical,
                        cooldown = "Preemptive Strike",
                        cooldownDuration = 120,
                        preExecute = ((FullCombatCharacter source, List<FullCombatCharacter> target, CombatData combatData, List<IEffect> effects, AbilityInfo abilityInfo) =>
                        {
                            FullCombatCharacter currentTarget = target[0];
                            foreach (FullCombatCharacter t in target)
                            {
                                if (currentTarget.nextAttackTime > t.nextAttackTime)
                                {
                                    currentTarget = t;
                                }
                            }

                            target.Clear();
                            target.Add(currentTarget);

                            return AbilityInfo.ProcessResult.Normal;
                        })
                    };

                    return ai.getCommand();
                case "Vicious Blow":
                    ai = new AbilityInfo()
                    {
                        name = "Vicious Blow",
                        message = "{Name} dealt {Damage} damage to {Target} with a Vicious Blow!",
                        requiredClassLevel = 15,
                        damageMultiplier = 5,
                        damageCoefficient = 1.5f,
                        damageType = AbilityInfo.DamageType.Physical,
                        cooldown = "Vicious Blow",
                        cooldownDuration = 180,
                        preExecute = ((FullCombatCharacter source, List<FullCombatCharacter> target, CombatData combatData, List<IEffect> effects, AbilityInfo abilityInfo) =>
                        {
                            FullCombatCharacter currentTarget = target[0];

                            if (currentTarget.hp * 2 > currentTarget.maxHP)
                            {
                                abilityInfo.message = "{Name}'s vicious blow missed {Target}!";
                                abilityInfo.damageCoefficient = 0.0f;
                            }

                            return AbilityInfo.ProcessResult.Normal;
                        })
                    };

                    return ai.getCommand();
                case "Adrenaline":
                    ai = new AbilityInfo()
                    {
                        name = "Adrenaline",
                        message = "Adrenaline pumps through {Name}!",
                        requiredClassLevel = 17,
                        oncePerRest = "Adrenaline",
                        preExecute = ((FullCombatCharacter source, List<FullCombatCharacter> target, CombatData combatData, List<IEffect> effects, AbilityInfo abilityInfo) =>
                        {
                            source.mods.Add(BasicModificationsGeneration.getAdrenalineModification((source.nextAttackTime + 60).ToString()));

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
