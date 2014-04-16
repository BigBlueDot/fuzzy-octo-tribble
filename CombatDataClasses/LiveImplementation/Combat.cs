using CombatDataClasses.Interfaces;
using MapDataClasses.MapDataClasses;
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
        private Dictionary<int, CharacterDisplay> npcs;
        private List<PlayerModels.CombatDataModels.CombatNPCModel> combatNPCModels;
        private PlayerModels.PlayerModel playerModel;
        private List<IEffect> currentEffects;
        private List<PlayerModels.CombatDataModels.CombatPCModel> combatCharacterModels;

        public Combat(PlayerModels.PlayerModel playerModel, string map, int encounterSelection, Func<float> initiativeCalculator)
        {
            int currentUniq = 1;
            this.playerModel = playerModel;
            uniqBridge = new Dictionary<int, int>();
            pcs = new Dictionary<int, CharacterDisplay>();
            npcs = new Dictionary<int, CharacterDisplay>();

            List<PlayerModels.Models.PartyCharacterModel> partyCharacterModels = PlayerModels.PlayerDataManager.getCurrentPartyPartyStats(playerModel);
            List<int> characterUniqs = new List<int>();
            foreach (PlayerModels.Models.PartyCharacterModel pcm in partyCharacterModels)
            {
                characterUniqs.Add(pcm.characterUniq);
            }
            List<PlayerModels.Models.CharacterModel> characterModels = PlayerModels.PlayerDataManager.getCurrentParty(playerModel, characterUniqs);
            combatCharacterModels = new List<PlayerModels.CombatDataModels.CombatPCModel>();

            foreach (int characterUniq in characterUniqs)
            {
                string name = string.Empty;
                int hp = 0;
                int maxHP = 0;
                int mp = 0;
                int maxMP = 0;
                int turnOrder = 0;
                int agi = 0;
                string classType = string.Empty;
                int level = 0;
                uniqBridge.Add(currentUniq, characterUniq);

                foreach (PlayerModels.Models.CharacterModel cm in characterModels)
                {
                    if (cm.uniq == characterUniq)
                    {
                        name = cm.name;
                        maxHP = cm.stats.maxHP;
                        maxMP = cm.stats.maxMP;
                        agi = cm.stats.agility;
                        classType = cm.currentClass;
                        level = cm.lvl;
                    }
                }

                foreach (PlayerModels.Models.PartyCharacterModel pcm in partyCharacterModels)
                {
                    if (pcm.characterUniq == characterUniq)
                    {
                        hp = pcm.hp;
                        mp = pcm.mp;
                    }
                }

                pcs.Add(characterUniq, new CharacterDisplay(name, hp, maxHP, mp, maxMP, new List<IStatusDisplay>(), currentUniq, turnOrder, classType, level));
                combatCharacterModels.Add(new PlayerModels.CombatDataModels.CombatPCModel()
                {
                    characterUniq = characterUniq,
                    mods = new List<PlayerModels.CombatDataModels.CombatModificationsModel>(),
                    stats = new PlayerModels.CombatDataModels.TemporaryCombatStatsModel() { hp = hp, mp = mp },
                    nextAttackTime = calculateNextAttackTime(0, initiativeCalculator(), agi),
                    combatUniq = currentUniq
                });
                currentUniq++;
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

            Encounter encounter = MapDataClasses.MapDataManager.getRandomEncounter(map, encounterSelection);
            combatNPCModels = new List<PlayerModels.CombatDataModels.CombatNPCModel>();
            foreach (MapDataClasses.MapDataClasses.Enemy enemy in encounter.enemies)
            {
                this.npcs.Add(currentUniq, new CharacterDisplay(enemy.name, enemy.maxHP, enemy.maxHP, enemy.maxMP, enemy.maxMP, new List<IStatusDisplay>(), currentUniq, currentUniq, enemy.type, enemy.level));
                this.combatNPCModels.Add(new PlayerModels.CombatDataModels.CombatNPCModel()
                {
                    enemyName = enemy.name,
                    stats = new PlayerModels.CombatDataModels.TemporaryCombatStatsModel()
                    {
                        hp = enemy.maxHP,
                        mp = enemy.maxMP
                    },
                    nextAttackTime = calculateNextAttackTime(0, initiativeCalculator(), enemy.agility),
                    combatUniq = currentUniq
                });

                currentUniq++;
            }
            playerModel.currentCombat.npcs = combatNPCModels;

            currentEffects = new List<IEffect>();
            currentEffects.Add(new Effect(EffectTypes.Message, 0, encounter.message, 0));

            calculateTurnOrder();
            calculateTurn(false);
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
            string fastestPCName = string.Empty;
            int fastestPCTime = int.MaxValue;
            foreach(int key in pcs.Keys) {
                pcDisplays.Add(pcs[key]);
                if (pcs[key].turnOrder < fastestPCTime)
                {
                    fastestPCTime = pcs[key].turnOrder;
                    fastestPCName = pcs[key].name;
                }
            }
            List<ICharacterDisplay> npcDisplays = new List<ICharacterDisplay>();
            foreach (int key in npcs.Keys)
            {
                npcDisplays.Add(npcs[key]);
            }

            return new CombatStatus(fastestPCName, currentEffects, pcDisplays, npcDisplays);
        }

        public ICombatStatus executeCommand(SelectedCommand command)
        {
            throw new NotImplementedException();
        }

        private int calculateNextAttackTime(int startTime, float abilityCoefficient, int agi)
        {
            return startTime + ((int)(60 * ((Math.Log10(agi) / (abilityCoefficient * 2 * Math.Log10(10))))));
        }

        private void calculateTurnOrder()
        {
            List<int> usedUniqs = new List<int>();
            for (int i = 0; i < npcs.Count + pcs.Count; i++)
            {
                int currentFastestUniq = 1;
                int fastestTime = int.MaxValue;
                bool isPC = false;
                foreach (PlayerModels.CombatDataModels.CombatNPCModel npc in combatNPCModels)
                {
                    if (!usedUniqs.Contains(npc.combatUniq) && npc.nextAttackTime < fastestTime)
                    {
                        currentFastestUniq = npc.combatUniq;
                        fastestTime = npc.nextAttackTime;
                        isPC = false;
                    }
                }

                foreach (PlayerModels.CombatDataModels.CombatPCModel pc in combatCharacterModels)
                {
                    if (!usedUniqs.Contains(pc.combatUniq) && pc.nextAttackTime < fastestTime)
                    {
                        currentFastestUniq = pc.combatUniq;
                        fastestTime = pc.nextAttackTime;
                        isPC = true;
                    }
                }

                usedUniqs.Add(currentFastestUniq);
                if (isPC)
                {
                    pcs[uniqBridge[currentFastestUniq]].setTurnOrder(i + 1);
                }
                else
                {
                    npcs[currentFastestUniq].setTurnOrder(i + 1);
                }
            }
        }

        private void calculateTurn(bool killPreviousTurn)
        {
            //This is where all the stuff is calculated until the player's next move
            if (killPreviousTurn)
            {
                currentEffects.Clear();
            }

            currentEffects.Add(new Effect(EffectTypes.ShowCommand, 0, string.Empty, 0));
        }
    }
}
