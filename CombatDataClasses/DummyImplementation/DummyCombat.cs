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
        public List<ICommand> getCommands()
        {
            //Get next player characters commands
            int characterUniq = 1;
            List<ICommand> returnValue = new List<ICommand>();

            returnValue.Add(new DummyCommand(new List<ICommand>(), false, 0, "Attack " + characterUniq.ToString(), false, 0));
            returnValue.Add(new DummyCommand(new List<ICommand>(), false, 0, "Guard " + characterUniq.ToString(), false, 0));
            returnValue.Add(new DummyCommand(new List<ICommand>(), false, 0, "Flee", false, 0));

            return returnValue;
        }

        public ICombatStatus getStatus()
        {
            List<IEffect> effectsList = new List<IEffect>();
            effectsList.Add(new DummyEffect(EffectTypes.Message, 0, "A horde of Goblins has appeared!", 0));
            return generateCombatStatus(effectsList);
        }

        public ICombatStatus executeCommand(SelectedCommand command)
        {
            //The non-dummy implementation will probably be handled in a separate class
            if (command.commandName == "Flee")
            {
                List<IEffect> effectsList = new List<IEffect>();
                effectsList.Add(new DummyEffect(EffectTypes.Message, 0, "You have escaped successfully!", 0));
                effectsList.Add(new DummyEffect(EffectTypes.CombatEnded, 0, string.Empty, 0));
                return generateCombatStatus(effectsList);
            }
            else {
                return getStatus();
            }
        }

        private ICombatStatus generateCombatStatus(List<IEffect> effectsList)
        {
            List<ICharacterDisplay> characterDisplays = new List<ICharacterDisplay>();
            characterDisplays.Add(new DummyCharacterDisplay("Scott Pilgrim", 30, 30, 2, 2, new List<IStatusDisplay>()));
            characterDisplays.Add(new DummyCharacterDisplay("Ada Lovelace", 35, 35, 3, 3, new List<IStatusDisplay>()));
            List<ICharacterDisplay> npcDisplays = new List<ICharacterDisplay>();
            npcDisplays.Add(new DummyCharacterDisplay("Goblin", 30, 30, 2, 2, new List<IStatusDisplay>()));
            npcDisplays.Add(new DummyCharacterDisplay("Boss Goblin", 50, 50, 2, 2, new List<IStatusDisplay>()));
            npcDisplays.Add(new DummyCharacterDisplay("Goblin", 30, 30, 2, 2, new List<IStatusDisplay>()));
            ICombatStatus combatStatus = new DummyCombatStatus("Scott Pilgrim", effectsList, characterDisplays, npcDisplays);
            return combatStatus;
        }
    }
}
