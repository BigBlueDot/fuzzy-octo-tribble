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
        public static List<ICommand> getClassCommands(int level)
        {
            List<ICommand> commands = new List<ICommand>();

            commands.Add(new Command(false, new List<ICommand>(), false, 0, 0, "Glance", false, 0, true));

            return commands;
        }
    }
}
