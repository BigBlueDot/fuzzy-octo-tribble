using CombatDataClasses.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombatDataClasses.ClassProcessor
{
    public class ClassProcessor
    {
        public static List<ICommand> getClassAbilities(string className, int level)
        {
            switch (className)
            {
                case "Adventurer":
                    return AdventurerProcessor.getClassCommands(level);
                default:
                    return new List<ICommand>();
            }
        }
    }
}
