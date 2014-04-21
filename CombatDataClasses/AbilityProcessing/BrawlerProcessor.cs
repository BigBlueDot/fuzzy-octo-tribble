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
