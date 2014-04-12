using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombatDataClasses.Interfaces
{
    public interface IStatusDisplay
    {
        Type type { get; }
        string value { get; }
    }

    public enum Type
    {
        Text
    }
}
