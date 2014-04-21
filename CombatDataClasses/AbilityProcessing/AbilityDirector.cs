using CombatDataClasses.Interfaces;
using CombatDataClasses.LiveImplementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombatDataClasses.ClassProcessor
{
    public class AbilityDirector
    {
        public static List<ICommand> getClassAbilities(FullCombatCharacter source, CombatData combatData)
        {
            return GeneralProcessor.getClassCommands(source, combatData);
        }

        public static Func<FullCombatCharacter, List<FullCombatCharacter>, CombatData, List<IEffect>> executeCommand(SelectedCommand command)
        {
            //In the future this will look through all of the available processors to find the right one to use
            return GeneralProcessor.executeCommand(command);
        }
    }
}
