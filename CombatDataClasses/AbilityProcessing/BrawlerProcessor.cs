using CombatDataClasses.AbilityProcessing.ModificationsGeneration;
using CombatDataClasses.ClassProcessor;
using CombatDataClasses.Interfaces;
using CombatDataClasses.LiveImplementation;
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
                commands.Add(new Command(false, new List<ICommand>(), false, 0, 0, "Adrenaline", false, 0, true, isDisabled("Adrenaline", source, combatData)));
            }

            return commands;
        }

        public Func<LiveImplementation.FullCombatCharacter, List<LiveImplementation.FullCombatCharacter>, LiveImplementation.CombatData, List<Interfaces.IEffect>> executeCommand(Interfaces.SelectedCommand command)
        {
            switch (command.commandName)
            {
                case "Disarming Blow":
                    return ((FullCombatCharacter source, List<FullCombatCharacter> target, CombatData combatData) =>
                        {
                            if (source.classLevel < 3 || combatData.hasCooldown(source.name, "Disarming Blow"))
                            {
                                return new List<IEffect>();
                            }
                            List<IEffect> effects = new List<IEffect>();
                            float coefficient = 1.0f;
                            foreach (FullCombatCharacter t in target)
                            {
                                int dmg = (int)((CombatCalculator.getNormalAttackValue(source) * 5 / t.vitality));
                                if (t.inflictDamage(ref dmg) == FullCombatCharacter.HitEffect.Unbalance)
                                {
                                    coefficient = coefficient * 2;
                                }
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
                                combatData.cooldowns.Add(new CombatData.Cooldown()
                                {
                                    character = source.name,
                                    name = "Disarming Blow",
                                    time = (source.nextAttackTime + 120)
                                });
                                
                                effects.Add(new Effect(EffectTypes.DealDamage, t.combatUniq, string.Empty, dmg));
                                effects.Add(new Effect(EffectTypes.Message, 0, source.name + " has dealt " + dmg + " damage to " + t.name + " with a disarming blow.", 0));

                            }
                            GeneralProcessor.calculateNextAttackTime(source, coefficient);
                            return effects;
                        });
                case "One-Two Punch":
                    return ((FullCombatCharacter source, List<FullCombatCharacter> target, CombatData combatData) =>
                    {
                        if (source.classLevel < 7 || combatData.hasCooldown(source.name, "One-Two Punch"))
                        {
                            return new List<IEffect>();
                        }
                        List<IEffect> effects = new List<IEffect>();
                        float coefficient = 1.0f;
                        foreach (FullCombatCharacter t in target)
                        {
                            int dmg = (int)((CombatCalculator.getNormalAttackValue(source) * 5 * .75f / t.vitality));
                            if (t.inflictDamage(ref dmg) == FullCombatCharacter.HitEffect.Unbalance)
                            {
                                coefficient = coefficient * 2;
                            }
                            if (t.inflictDamage(ref dmg) == FullCombatCharacter.HitEffect.Unbalance)
                            {
                                coefficient = coefficient * 2;
                            }
                            combatData.cooldowns.Add(new CombatData.Cooldown()
                            {
                                character = source.name,
                                name = "One-Two Punch",
                                time = (source.nextAttackTime + 120)
                            });

                            effects.Add(new Effect(EffectTypes.DealDamage, t.combatUniq, string.Empty, dmg));
                            effects.Add(new Effect(EffectTypes.DealDamage, t.combatUniq, string.Empty, dmg));
                            effects.Add(new Effect(EffectTypes.Message, 0, source.name + " has dealt 2x" + dmg + " damage to " + t.name + " with a One-Two Punch.", 0));

                        }
                        GeneralProcessor.calculateNextAttackTime(source, coefficient);
                        return effects;
                    });
                case "Sweep":
                    return ((FullCombatCharacter source, List<FullCombatCharacter> targets, CombatData combatData) =>
                    {
                        if (source.classLevel < 9 || combatData.hasCooldown(source.name, "Sweep"))
                        {
                            return new List<IEffect>();
                        }
                        List<IEffect> effects = new List<IEffect>();
                        float coefficient = 1.5f;
                        foreach (FullCombatCharacter t in targets)
                        {
                            int dmg = (int)((CombatCalculator.getNormalAttackValue(source) * 5 * .5f / t.vitality));
                            if (t.inflictDamage(ref dmg) == FullCombatCharacter.HitEffect.Unbalance)
                            {
                                coefficient = coefficient * 2;
                            }

                            effects.Add(new Effect(EffectTypes.DealDamage, t.combatUniq, string.Empty, dmg));
                        }
                        combatData.cooldowns.Add(new CombatData.Cooldown()
                        {
                            character = source.name,
                            name = "Sweep",
                            time = (source.nextAttackTime + 180)
                        });
                        effects.Add(new Effect(EffectTypes.Message, 0, source.name + " dealt damage to all enemies with a sweeping blow!", 0));
                        GeneralProcessor.calculateNextAttackTime(source, coefficient);
                        return effects;
                    });
                case "Preemptive Strike":
                    return ((FullCombatCharacter source, List<FullCombatCharacter> targets, CombatData combatData) =>
                    {
                        if (source.classLevel < 13 || isDisabled("Preemptive Strike", source, combatData))
                        {
                            return new List<IEffect>();
                        }
                        List<IEffect> effects = new List<IEffect>();
                        float coefficient = 1.0f;
                        FullCombatCharacter currentTarget = targets[0];
                        foreach (FullCombatCharacter t in targets)
                        {
                            if (currentTarget.nextAttackTime > t.nextAttackTime)
                            {
                                currentTarget = t;
                            }
                        }
                        int dmg = (int)((CombatCalculator.getNormalAttackValue(source) * 5 * 1.5f / currentTarget.vitality));
                        if (currentTarget.inflictDamage(ref dmg) == FullCombatCharacter.HitEffect.Unbalance)
                        {
                            coefficient = coefficient * 2;
                        }

                        effects.Add(new Effect(EffectTypes.DealDamage, currentTarget.combatUniq, string.Empty, dmg));
                        combatData.cooldowns.Add(new CombatData.Cooldown()
                        {
                            character = source.name,
                            name = "Preemptive Strike",
                            time = (source.nextAttackTime + 120)
                        });
                        effects.Add(new Effect(EffectTypes.Message, 0, source.name + " dealt " + dmg + " damage to " + currentTarget.name + " with a preemptive strike!", 0));
                        GeneralProcessor.calculateNextAttackTime(source, coefficient);
                        return effects;
                    });
                case "Vicious Blow":
                    return ((FullCombatCharacter source, List<FullCombatCharacter> targets, CombatData combatData) =>
                    {
                        if (source.classLevel < 15 || isDisabled("Vicious Blow", source, combatData))
                        {
                            return new List<IEffect>();
                        }
                        List<IEffect> effects = new List<IEffect>();
                        float coefficient = 1.0f;
                        foreach (FullCombatCharacter t in targets)
                        {
                            if (t.hp < t.maxHP / 2)
                            {
                                int dmg = (int)((CombatCalculator.getNormalAttackValue(source) * 5 * 1.5f / t.vitality));
                                if (t.inflictDamage(ref dmg) == FullCombatCharacter.HitEffect.Unbalance)
                                {
                                    coefficient = coefficient * 2;
                                }
                                effects.Add(new Effect(EffectTypes.DealDamage, t.combatUniq, string.Empty, dmg));
                                effects.Add(new Effect(EffectTypes.Message, 0, source.name + " dealt " + dmg + " damage to " + t.name + " with a vicious blow!", 0));
                            }
                            else
                            {
                                effects.Add(new Effect(EffectTypes.Message, 0, source.name + "'s Vicious Blow Missed!", 0));
                            }
                        }

                        combatData.cooldowns.Add(new CombatData.Cooldown()
                        {
                            character = source.name,
                            name = "Vicious Blow",
                            time = (source.nextAttackTime + 180)
                        });
                        
                        GeneralProcessor.calculateNextAttackTime(source, coefficient);
                        return effects;
                    });
                case "Adrenaline":
                    return ((FullCombatCharacter source, List<FullCombatCharacter> targets, CombatData combatData) =>
                    {
                        if (source.classLevel < 17 || isDisabled("Adrenaline", source, combatData))
                        {
                            return new List<IEffect>();
                        }
                        List<IEffect> effects = new List<IEffect>();
                        float coefficient = 1.0f;
                        source.usedAbilities.Add("Adrenaline");

                        source.mods.Add(BasicModificationsGeneration.getAdrenalineModification((source.nextAttackTime + 60).ToString()));

                        GeneralProcessor.calculateNextAttackTime(source, coefficient);
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
