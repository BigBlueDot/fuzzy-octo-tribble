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
    public class AdventurerProcessor
    {
        public static bool isAdventurerCommand(string name)
        {
            if (name == "Glance" || name == "Guarded Strike" || name == "Reckless Hit" || name == "Guided Strike")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static Func<List<FullCombatCharacter>, List<FullCombatCharacter>, CombatData, List<IEffect>> initialExecute(FullCombatCharacter source)
        {
            return ((List<FullCombatCharacter> allies, List<FullCombatCharacter> enemies, CombatData combatData) =>
            {
                List<IEffect> effects = new List<IEffect>();
                if (source.className == "Adventurer" && source.classLevel >= 7)
                {
                    foreach (FullCombatCharacter fcc in enemies)
                    {
                        CombatModificationsModel cmm = new CombatModificationsModel();
                        cmm.name = "Glance";
                        cmm.conditions = new List<CombatConditionModel>();
                        fcc.mods.Add(cmm);
                    }
                    effects.Add(new Effect(EffectTypes.Message, 0, "All enemies have been glanced by " + source.name + "'s Insight ability!", 0));
                }
                return effects;
            });
        }

        public static List<ICommand> getClassCommands(int level)
        {
            List<ICommand> commands = new List<ICommand>();

            commands.Add(new Command(false, new List<ICommand>(), false, 0, 0, "Glance", false, 0, true));
            if (level >= 2)
            {
                commands.Add(new Command(false, new List<ICommand>(), false, 0, 0, "Guarded Strike", false, 0, true));
            }
            if (level >= 3)
            {
                commands.Add(new Command(false, new List<ICommand>(), false, 0, 0, "Reckless Hit", false, 0, true));
            }
            if (level >= 4)
            {
                commands.Add(new Command(false, new List<ICommand>(), false, 0, 0, "Guided Strike", false, 0, true));
            }

            return commands;
        }

        public static Func<FullCombatCharacter, List<FullCombatCharacter>, CombatData, List<IEffect>> executeCommand(SelectedCommand command)
        {
            switch (command.commandName)
            {
                case "Glance":
                    return ((FullCombatCharacter source, List<FullCombatCharacter> target, CombatData combatData) =>
                        {
                            List<IEffect> effects = new List<IEffect>();
                            foreach (FullCombatCharacter t in target)
                            {
                                t.mods.Add(new PlayerModels.CombatDataModels.CombatModificationsModel()
                                {
                                    name="Glance",
                                    conditions = new List<PlayerModels.CombatDataModels.CombatConditionModel>()
                                });
                                effects.Add(new Effect(EffectTypes.Message, 0, t.name + " has been glanced!", 0));
                            }
                            GeneralProcessor.calculateNextAttackTime(source, .5f);
                            return effects;
                        });
                case "Guarded Strike":
                    return ((FullCombatCharacter source, List<FullCombatCharacter> targets, CombatData combatData) =>
                        {
                            if (source.classLevel < 2) //Verify that they have the level to use this skill
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
                                t.inflictDamage(ref dmg);
                                effects.Add(new Effect(EffectTypes.DealDamage, t.combatUniq, string.Empty, dmg));
                                effects.Add(new Effect(EffectTypes.Message, 0, preMessage + source.name + " has dealt " + dmg + " damage to " + t.name + ".", 0));
                            }
                            GeneralProcessor.calculateNextAttackTime(source, coefficient);
                            return effects;
                        });
                case "Reckless Hit":
                    return ((FullCombatCharacter source, List<FullCombatCharacter> targets, CombatData combatData) =>
                    {
                        if (source.classLevel < 3)
                        {
                            return new List<IEffect>();
                        }
                        List<IEffect> effects = new List<IEffect>();
                        foreach(FullCombatCharacter t in targets){
                            int dmg = (int)((CombatCalculator.getNormalAttackValue(source) * 8 / t.vitality));
                            t.inflictDamage(ref dmg);
                            effects.Add(new Effect(EffectTypes.DealDamage, t.combatUniq, string.Empty, dmg));
                            effects.Add(new Effect(EffectTypes.Message, 0, source.name + " has dealt " + dmg + " damage to " + t.name + " with a reckless attack.", 0));
                        }
                        GeneralProcessor.calculateNextAttackTime(source, 1.2f);
                        source.mods.Add(BasicModificationsGeneration.getRecklessModification(source.name));
                        return effects;
                    });
                case "Guided Strike":
                    return ((FullCombatCharacter source, List<FullCombatCharacter> targets, CombatData combatData) =>
                        {
                            if (source.classLevel < 4)
                            {
                                return new List<IEffect>();
                            }
                            List<IEffect> effects = new List<IEffect>();
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
                                t.inflictDamage(ref dmg);
                                effects.Add(new Effect(EffectTypes.DealDamage, t.combatUniq, string.Empty, dmg));
                                effects.Add(new Effect(EffectTypes.Message, 0,preMessage + source.name + " has dealt " + dmg + " damage to " + t.name + " with a guided strike.", 0));
                            }
                            GeneralProcessor.calculateNextAttackTime(source, 1.0f);
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
