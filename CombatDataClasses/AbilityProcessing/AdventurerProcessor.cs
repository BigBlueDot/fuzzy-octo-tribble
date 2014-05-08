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
            switch (command.commandName)
            {
                case "Glance":
                    return ((FullCombatCharacter source, List<FullCombatCharacter> target, CombatData combatData) =>
                        {
                            List<IEffect> effects = new List<IEffect>();
                            foreach (FullCombatCharacter t in target)
                            {
                                if (!BasicModificationsGeneration.hasMod(t, "Glance"))
                                {
                                    t.mods.Add(new PlayerModels.CombatDataModels.CombatModificationsModel()
                                    {
                                        name = "Glance",
                                        conditions = new List<PlayerModels.CombatDataModels.CombatConditionModel>()
                                    });
                                    effects.Add(new Effect(EffectTypes.Message, 0, t.name + " has been glanced!", 0));
                                }
                            }
                            GeneralProcessor.calculateNextAttackTime(source, .5f, combatData);
                            return effects;
                        });
                case "Guarded Strike":
                    return ((FullCombatCharacter source, List<FullCombatCharacter> targets, CombatData combatData) =>
                        {
                            if (source.classLevel < 3) //Verify that they have the level to use this skill
                            {
                                return new List<IEffect>();
                            }
                            List<IEffect> effects = new List<IEffect>();
                            float coefficient = 1.0f;
                            string preMessage = string.Empty;
                            if (BasicModificationsGeneration.hasMod(source, "Guard"))
                            {
                                coefficient = .75f;
                                preMessage = source.name + " attacks quicker due to guarding!  ";
                            }
                            foreach (FullCombatCharacter t in targets)
                            {
                                int dmg = (int)((CombatCalculator.getNormalAttackValue(source) * 5 / t.vitality));
                                if (t.inflictDamage(ref dmg) == FullCombatCharacter.HitEffect.Unbalance)
                                {
                                    coefficient = coefficient * 2;
                                }
                                effects.Add(new Effect(EffectTypes.DealDamage, t.combatUniq, string.Empty, dmg));
                                effects.Add(new Effect(EffectTypes.Message, 0, preMessage + source.name + " has dealt " + dmg + " damage to " + t.name + ".", 0));
                            }
                            GeneralProcessor.calculateNextAttackTime(source, coefficient, combatData);
                            return effects;
                        });
                case "Reckless Hit":
                    return ((FullCombatCharacter source, List<FullCombatCharacter> targets, CombatData combatData) =>
                    {
                        if (source.classLevel < 5)
                        {
                            return new List<IEffect>();
                        }
                        float coefficient = 1.2f;
                        List<IEffect> effects = new List<IEffect>();
                        foreach(FullCombatCharacter t in targets){
                            int dmg = (int)((CombatCalculator.getNormalAttackValue(source) * 8 / t.vitality));
                            if (t.inflictDamage(ref dmg) == FullCombatCharacter.HitEffect.Unbalance)
                            {
                                coefficient = coefficient * 2;
                            }
                            effects.Add(new Effect(EffectTypes.DealDamage, t.combatUniq, string.Empty, dmg));
                            effects.Add(new Effect(EffectTypes.Message, 0, source.name + " has dealt " + dmg + " damage to " + t.name + " with a reckless attack.", 0));
                        }
                        GeneralProcessor.calculateNextAttackTime(source, coefficient, combatData);
                        source.mods.Add(BasicModificationsGeneration.getRecklessModification(source.name));
                        return effects;
                    });
                case "Guided Strike":
                    return ((FullCombatCharacter source, List<FullCombatCharacter> targets, CombatData combatData) =>
                        {
                            if (source.classLevel < 7)
                            {
                                return new List<IEffect>();
                            }
                            List<IEffect> effects = new List<IEffect>();
                            float coefficient = 1.0f;
                            foreach (FullCombatCharacter t in targets)
                            {
                                float dmgMod = .8f;
                                string preMessage = string.Empty;
                                if (BasicModificationsGeneration.hasMod(t, "Glance"))
                                {
                                    dmgMod = 1.2f;
                                    preMessage = source.name + " is locked on!  ";
                                }
                                int dmg = (int)((CombatCalculator.getNormalAttackValue(source) * dmgMod * 5 / t.vitality));
                                if (t.inflictDamage(ref dmg) == FullCombatCharacter.HitEffect.Unbalance)
                                {
                                    coefficient = coefficient * 2;
                                }
                                effects.Add(new Effect(EffectTypes.DealDamage, t.combatUniq, string.Empty, dmg));
                                effects.Add(new Effect(EffectTypes.Message, 0,preMessage + source.name + " has dealt " + dmg + " damage to " + t.name + " with a guided strike.", 0));
                            }
                            GeneralProcessor.calculateNextAttackTime(source, coefficient, combatData);
                            return effects;
                        });
                case "First Strike":
                    return ((FullCombatCharacter source, List<FullCombatCharacter> targets, CombatData combatData) =>
                    {
                        if (source.classLevel < 15)
                        {
                            return new List<IEffect>();
                        }
                        List<IEffect> effects = new List<IEffect>();
                        float coefficient = 1.0f;
                        if (combatData.isFirstTurn(source.name))
                        {
                            foreach (FullCombatCharacter t in targets)
                            {
                                int dmg = (int)((CombatCalculator.getNormalAttackValue(source) * 1.5 * 5 / t.vitality));
                                if (t.inflictDamage(ref dmg) == FullCombatCharacter.HitEffect.Unbalance)
                                {
                                    coefficient = coefficient * 2;
                                }
                                effects.Add(new Effect(EffectTypes.DealDamage, t.combatUniq, string.Empty, dmg));
                                effects.Add(new Effect(EffectTypes.Message, 0, source.name + " is well rested.  " + source.name + " has dealt " + dmg + " damage to " + t.name + " with a guided strike.", 0));
                            }
                        }
                        GeneralProcessor.calculateNextAttackTime(source, coefficient, combatData);
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
