using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombatDataClasses.Interfaces
{
    public interface ICommand
    {
        List<ICommand> childCommands { get; }
        bool limitedUsage { get; }
        int uses { get; }
        string name { get;  }
        bool mpNeeded { get;  }
        int mpCost { get;  }
    }
}
