using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombatDataClasses.Interfaces
{
    public interface ICombatStatus
    {
        string currentCharacter { get; }
        IEnumerable<IEffect> effects { get; }
        IEnumerable<ICharacterDisplay> characterDisplays { get; }
        IEnumerable<ICharacterDisplay> npcDisplays { get; }
        string location { get; }
    }
}
