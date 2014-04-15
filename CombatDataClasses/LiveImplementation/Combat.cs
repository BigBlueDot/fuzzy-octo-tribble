using CombatDataClasses.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombatDataClasses.LiveImplementation
{
    public class Combat : ICombat
    {
        private int currentCharacterUniq;
        private Dictionary<int, CharacterDisplay> pcs;
        private Dictionary<int, int> uniqBridge;
        private List<CharacterDisplay> npcs;
        private PlayerModels.PlayerModel playerModel;
        private List<IEffect> currentEffects;

        public Combat(PlayerModels.PlayerModel playerModel)
        {
            int currentUniq = 1;
            this.playerModel = playerModel;
            uniqBridge = new Dictionary<int, int>();
            pcs = new Dictionary<int, CharacterDisplay>();
            npcs = new List<CharacterDisplay>();

            List<PlayerModels.Models.PartyCharacterModel> partyCharacterModels = PlayerModels.PlayerDataManager.getCurrentPartyPartyStats(playerModel);
            List<int> characterUniqs = new List<int>();
            foreach (PlayerModels.Models.PartyCharacterModel pcm in partyCharacterModels)
            {
                characterUniqs.Add(pcm.characterUniq);
            }
            List<PlayerModels.Models.CharacterModel> characterModels = PlayerModels.PlayerDataManager.getCurrentParty(playerModel, characterUniqs);
            List<PlayerModels.CombatDataModels.CombatPCModel> combatCharacterModels = new List<PlayerModels.CombatDataModels.CombatPCModel>();

            foreach (int characterUniq in characterUniqs)
            {
                string name = string.Empty;
                int hp = 0;
                int maxHP = 0;
                int mp = 0;
                int maxMP = 0;
                int turnOrder = 0;
                uniqBridge.Add(currentUniq, characterUniq);

                foreach (PlayerModels.Models.CharacterModel cm in characterModels)
                {
                    name = cm.name;
                    maxHP = cm.stats.maxHP;
                    maxMP = cm.stats.maxMP;
                }

                foreach (PlayerModels.Models.PartyCharacterModel pcm in partyCharacterModels)
                {
                    hp = pcm.hp;
                    mp = pcm.mp;
                }

                turnOrder = currentUniq;

                pcs.Add(characterUniq, new CharacterDisplay(name, hp, maxHP, mp, maxMP, new List<IStatusDisplay>(), currentUniq, turnOrder));
                combatCharacterModels.Add(new PlayerModels.CombatDataModels.CombatPCModel()
                {
                    characterUniq = characterUniq,
                    mods = new List<PlayerModels.CombatDataModels.CombatModificationsModel>(),
                    stats = new PlayerModels.CombatDataModels.TemporaryCombatStatsModel() { hp = hp, mp = mp },
                });
                currentUniq++;

                currentEffects = new List<IEffect>();
                currentEffects.Add(new Effect(EffectTypes.Message, 0, "You have entered combat.", 0));
            }

            playerModel.currentCombat = new PlayerModels.CombatDataModels.CombatModel();
            playerModel.currentCombat.pcs = combatCharacterModels;
            playerModel.currentCombat.currentTime = 0;

            //for(int i=0; i<pcs.Count; i++) {
            //    PlayerModels.CombatDataModels.CombatPCModel currentCombatModel = cpcs[i];
            //    PlayerModels.Models.CharacterModel currentModel = pcs[i];
            //    CharacterDisplay cd = new CharacterDisplay(currentModel.name, currentCombatModel.stats.hp, 
            //        currentModel.stats.maxHP, currentCombatModel.stats.mp, 
            //        currentModel.stats.maxMP, new List<IStatusDisplay>(), 
            //        currentUniq, 1);
            //    this.pcs.Add(cd);
            //    currentUniq++;
            //}
            this.npcs.Add(new CharacterDisplay("Goblin", 30, 30, 2, 2, new List<IStatusDisplay>(), currentUniq++, 4));
            this.npcs.Add(new CharacterDisplay("Boss Goblin", 50, 50, 2, 2, new List<IStatusDisplay>(), currentUniq++, 2));
            this.npcs.Add(new CharacterDisplay("Goblin", 0, 30, 2, 2, new List<IStatusDisplay>(), currentUniq++, 5));
        }
        
        public List<ICommand> getCommands()
        {
            List<ICommand> returnValue = new List<ICommand>();
            returnValue.Add(new Command(false, new List<ICommand>(), false, 0, 0, "Attack", false, 0, true));
            returnValue.Add(new Command(false, new List<ICommand>(), false, 0, 0, "Guard", false, 0, false));
            returnValue.Add(new Command(false, new List<ICommand>(), false, 0, 0, "Flee", false, 0, false));
            return returnValue;
        }

        public ICombatStatus getStatus()
        {
            List<ICharacterDisplay> pcDisplays = new List<ICharacterDisplay>();
            foreach(int key in pcs.Keys) {
                pcDisplays.Add(pcs[key]);
            }

            return new CombatStatus("INCOMPLETE", currentEffects, pcDisplays, npcs);
        }

        public ICombatStatus executeCommand(SelectedCommand command)
        {
            throw new NotImplementedException();
        }
    }
}
