using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombatDataClasses.Interfaces
{
    public class SelectedCommand
    {
        public string commandName { get; set; }
        public bool hasSubCommand { get; set; }
        public SelectedCommand subCommand { get; set; }
        public List<int> targets { get; set; }
    }
}
