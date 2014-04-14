using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombatDataClasses.Interfaces
{
    public interface ICombat
    {
        List<ICommand> getCommands();
        ICombatStatus getStatus();
        ICombatStatus executeCommand(SelectedCommand command);
    }
}
