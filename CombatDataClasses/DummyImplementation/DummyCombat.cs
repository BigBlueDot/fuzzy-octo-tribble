using CombatDataClasses.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombatDataClasses.DummyImplementation
{
    public class DummyCombat : ICombat
    {
        public List<ICommand> getCommands(int characterUniq)
        {
            List<ICommand> returnValue = new List<ICommand>();

            returnValue.Add(new DummyCommand(new List<ICommand>(), false, 0, "Attack " + characterUniq.ToString(), false, 0));
            returnValue.Add(new DummyCommand(new List<ICommand>(), false, 0, "Guard " + characterUniq.ToString(), false, 0));
            returnValue.Add(new DummyCommand(new List<ICommand>(), false, 0, "Flee " + characterUniq.ToString(), false, 0));

            return returnValue;
        }

        public ICombatStatus getStatus()
        {
            List<IEffect> effectsList = new List<IEffect>();
            effectsList.Add(new DummyEffect(EffectTypes.Message, 0, "This is a combat message", 0));
            List<ICharacterDisplay> characterDisplays = new List<ICharacterDisplay>();
            characterDisplays.Add(new DummyCharacterDisplay("Scott Pilgrim", 30, 30, 2, 2, new List<IStatusDisplay>()));
            List<ICharacterDisplay> npcDisplays = new List<ICharacterDisplay>();
            npcDisplays.Add(new DummyCharacterDisplay("Goblin", 30, 30, 2, 2, new List<IStatusDisplay>()));
            ICombatStatus combatStatus = new DummyCombatStatus("Scott Pilgrim", effectsList, characterDisplays, npcDisplays);
            return combatStatus;
        }
    }
}
