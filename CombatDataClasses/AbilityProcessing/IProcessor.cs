using CombatDataClasses.Interfaces;
using CombatDataClasses.LiveImplementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombatDataClasses.AbilityProcessing
{
    public interface IProcessor
    {
        bool isType(string name);
        bool isDisabled(string abilityName, FullCombatCharacter source, CombatData combatData);
        Func<List<FullCombatCharacter>, List<FullCombatCharacter>, CombatData, List<IEffect>> initialExecute(FullCombatCharacter source);
        List<ICommand> getClassCommands(FullCombatCharacter source, CombatData combatData);
        Func<FullCombatCharacter, List<FullCombatCharacter>, CombatData, List<IEffect>> executeCommand(SelectedCommand command);

    }
}
