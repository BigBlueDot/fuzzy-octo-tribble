using CombatDataClasses.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombatDataClasses.LiveImplementation
{
    public class CombatStatus : ICombatStatus
    {
        public CombatStatus(string currentCharacter, IEnumerable<IEffect> effects, IEnumerable<ICharacterDisplay> characterDisplays, IEnumerable<ICharacterDisplay> npcDisplays)
        {
            _currentCharacter = currentCharacter;
            _effects = effects;
            _characterDisplays = characterDisplays;
            _npcDisplays = npcDisplays;
        }

        private string _currentCharacter;
        public string currentCharacter
        {
            get
            {
                return _currentCharacter;
            }
        }

        private IEnumerable<IEffect> _effects;
        public IEnumerable<IEffect> effects
        {
            get
            {
                return _effects;
            }
        }

        private IEnumerable<ICharacterDisplay> _characterDisplays;
        public IEnumerable<ICharacterDisplay> characterDisplays
        {
            get
            {
                return _characterDisplays;
            }
        }

        private IEnumerable<ICharacterDisplay> _npcDisplays;
        public IEnumerable<ICharacterDisplay> npcDisplays
        {
            get
            {
                return _npcDisplays;
            }
        }
    }
}
