using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombatDataClasses.Interfaces
{
    public interface ICharacterDisplay
    {
        string name { get; }
        int hp { get;  }
        int maxHP { get;  }
        int mp { get;  }
        int maxMP { get; }
        int uniq { get; }
        IEnumerable<IStatusDisplay> statuses { get; }
    }
}
