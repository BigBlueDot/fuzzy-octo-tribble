using CombatDataClasses.Interfaces;
using CombatDataClasses.LiveImplementation;
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
            if (name == "Glance")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static List<ICommand> getClassCommands(int level)
        {
            List<ICommand> commands = new List<ICommand>();

            commands.Add(new Command(false, new List<ICommand>(), false, 0, 0, "Glance", false, 0, true));

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
