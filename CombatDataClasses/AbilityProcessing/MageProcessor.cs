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
            if (name == "Magic Dart")
            {
                return true;
            }

            return false;
        }

        public bool isDisabled(string abilityName, LiveImplementation.FullCombatCharacter source, LiveImplementation.CombatData combatData)
        {
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
