using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombatDataClasses.Interfaces
{
    public interface ICommand
    {
        bool hasChildCommands { get; }
        List<ICommand> childCommands { get; }
        bool limitedUsage { get; }
        int uses { get; }
        int totalUses { get; }
        string name { get;  }
        bool mpNeeded { get;  }
        int mpCost { get;  }
        bool hasTarget { get; }
    }
}
