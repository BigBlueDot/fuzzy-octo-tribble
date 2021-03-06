﻿using System;
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
        int turnOrder { get; }
        string type { get; }
        int level { get; }
        bool showSpecificStats { get; }
        int strength { get; }
        int vitality { get; }
        int intellect { get; }
        int wisdom { get; }
        int agility { get; }
        IEnumerable<IStatusDisplay> statuses { get; }
        string description { get; }
    }
}
