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
            List<ICommand> returnValue = new List<ICommand>();
            List<ICommand> magicList = new List<ICommand>();
            magicList.Add(new DummyCommand(false, new List<ICommand>(), false, 0, 0, "Fireball", true, 1, true));
            magicList.Add(new DummyCommand(false, new List<ICommand>(), false, 0, 0, "Heal", true, 1, true));

            returnValue.Add(new DummyCommand(false, new List<ICommand>(), false, 0, 0, "Attack", false, 0, true));
            returnValue.Add(new DummyCommand(true, magicList, false, 0, 0, "Magic", false, 0, false));
            returnValue.Add(new DummyCommand(false, new List<ICommand>(), false, 0, 0, "Guard", false, 0, false));
            returnValue.Add(new DummyCommand(false, new List<ICommand>(), false, 0, 0, "Flee", false, 0, false));
            returnValue.Add(new DummyCommand(false, new List<ICommand>(), false, 0, 0, "Heal", false, 0, false));
            returnValue.Add(new DummyCommand(false, new List<ICommand>(), true, 2, 3, "Destroy", false, 0, false));
            returnValue.Add(new DummyCommand(false, new List<ICommand>(), false, 0, 0, "Game Over", false, 0, false));

            return returnValue;
        }

        public ICombatStatus getStatus()
        {
            List<IEffect> effectsList = new List<IEffect>();
            effectsList.Add(new DummyEffect(EffectTypes.Message, 0, "A horde of Goblins has appeared!", 0));
            return generateCombatStatus(effectsList, 0);
        }

        public ICombatStatus executeCommand(SelectedCommand command)
        {
            //The non-dummy implementation will probably be handled in a separate class
            int target = 1;
            if (command.targets != null && command.targets.Count != 0)
            {
                target = command.targets[0];
            }
            if (command.commandName == "Flee")
            {
                List<IEffect> effectsList = new List<IEffect>();
                effectsList.Add(new DummyEffect(EffectTypes.Message, 0, "You have escaped successfully!", 0));
                effectsList.Add(new DummyEffect(EffectTypes.CombatEnded, 0, string.Empty, 0));
                return generateCombatStatus(effectsList, 0);
            }
            else if (command.commandName == "Attack")
            {
                List<IEffect> effectsList = new List<IEffect>();
                effectsList.Add(new DummyEffect(EffectTypes.DealDamage, target, string.Empty, 5));
                effectsList.Add(new DummyEffect(EffectTypes.Message, 0, "Damage has been dealt!", 0));
                return generateCombatStatus(effectsList, 5);
            }
            else if (command.commandName == "Magic")
            {
                target = command.subCommand.targets[0];
                if (command.subCommand.commandName == "Fireball")
                {
                    List<IEffect> effectsList = new List<IEffect>();
                    effectsList.Add(new DummyEffect(EffectTypes.DealDamage, target, string.Empty, 5));
                    effectsList.Add(new DummyEffect(EffectTypes.Message, 0, "A gigantic fireball damages the target!", 0));
                    return generateCombatStatus(effectsList, 5);
                }
                else if (command.subCommand.commandName == "Heal")
                {
                    List<IEffect> effectsList = new List<IEffect>();
                    effectsList.Add(new DummyEffect(EffectTypes.HealDamage, target, string.Empty, 5));
                    effectsList.Add(new DummyEffect(EffectTypes.Message, 0, "A holy light heals a character.", 0));
                    return generateCombatStatus(effectsList, 5);
                }
            }
            else if (command.commandName == "Heal")
            {
                List<IEffect> effectsList = new List<IEffect>();
                effectsList.Add(new DummyEffect(EffectTypes.HealDamage, 1, string.Empty, 5));
                effectsList.Add(new DummyEffect(EffectTypes.Message, 0, "Healed!", 0));
                return generateCombatStatus(effectsList, 5);
            }
            else if (command.commandName == "Guard")
            {
                List<IEffect> effectsList = new List<IEffect>();
                effectsList.Add(new DummyEffect(EffectTypes.Message, 0, "You are guarding.", 0));
                return generateCombatStatus(effectsList, 5);
            }
            else if (command.commandName == "Destroy")
            {
                List<IEffect> effectsList = new List<IEffect>();
                effectsList.Add(new DummyEffect(EffectTypes.DestroyCharacter, target, string.Empty, 5));
                return generateCombatStatus(effectsList, 5);
            }
            else if (command.commandName == "Game Over")
            {
                List<IEffect> effectsList = new List<IEffect>();
                effectsList.Add(new DummyEffect(EffectTypes.GameOver, 1, string.Empty, 5));
                return generateCombatStatus(effectsList, 5);
            }
            return getStatus();
        }

        private ICombatStatus generateCombatStatus(List<IEffect> effectsList, int healthDecrease)
        {
            List<ICharacterDisplay> characterDisplays = new List<ICharacterDisplay>();
            characterDisplays.Add(new DummyCharacterDisplay("Scott Pilgrim", 30 - healthDecrease, 30, 2, 2, new List<IStatusDisplay>(), 1, 1));
            characterDisplays.Add(new DummyCharacterDisplay("Ada Lovelace", 35 - healthDecrease, 35, 3, 3, new List<IStatusDisplay>(), 2, 3));
            List<ICharacterDisplay> npcDisplays = new List<ICharacterDisplay>();
            npcDisplays.Add(new DummyCharacterDisplay("Goblin", 30 - healthDecrease, 30, 2, 2, new List<IStatusDisplay>(), 3, 4));
            npcDisplays.Add(new DummyCharacterDisplay("Boss Goblin", 50 - healthDecrease, 50, 2, 2, new List<IStatusDisplay>(), 4, 2));
            npcDisplays.Add(new DummyCharacterDisplay("Goblin", 0, 30, 2, 2, new List<IStatusDisplay>(), 5, 5));
            ICombatStatus combatStatus = new DummyCombatStatus("Scott Pilgrim", effectsList, characterDisplays, npcDisplays);
            return combatStatus;
        }


        public ICombatStatus nextTurn()
        {
            throw new NotImplementedException();
        }
    }
}
