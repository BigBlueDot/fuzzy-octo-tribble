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
        private Dictionary<int, int> uniqBridge;
        private PlayerModels.PlayerModel playerModel;
        private List<IEffect> currentEffects;
        private Dictionary<int, FullCombatCharacter> pcs;
        private Dictionary<int, FullCombatCharacter> npcs;
        private List<PlayerModels.CombatDataModels.CombatCharacterModel> combatCharacterModels; //These are stored so they can be updated
        private List<PlayerModels.CombatDataModels.CombatCharacterModel> combatNPCModels;

        public Combat(PlayerModels.PlayerModel playerModel, string map, int encounterSelection, Func<float> initiativeCalculator)
        {
            int currentUniq = 1;
            this.playerModel = playerModel;
            uniqBridge = new Dictionary<int, int>();
            pcs = new Dictionary<int, FullCombatCharacter>();
            npcs = new Dictionary<int, FullCombatCharacter>();

            List<PlayerModels.Models.PartyCharacterModel> partyCharacterModels = PlayerModels.PlayerDataManager.getCurrentPartyPartyStats(playerModel);
            List<int> characterUniqs = new List<int>();
            foreach (PlayerModels.Models.PartyCharacterModel pcm in partyCharacterModels)
            {
                characterUniqs.Add(pcm.characterUniq);
            }
            List<PlayerModels.Models.CharacterModel> characterModels = PlayerModels.PlayerDataManager.getCurrentParty(playerModel, characterUniqs);
            combatCharacterModels = new List<PlayerModels.CombatDataModels.CombatCharacterModel>();

            foreach (int characterUniq in characterUniqs)
            {
                string name = string.Empty;
                int hp = 0;
                int maxHP = 0;
                int mp = 0;
                int maxMP = 0;
                int turnOrder = 0;
                string classType = string.Empty;
                int level = 0;
                int strength = 0;
                int vitality = 0;
                int agility = 0;
                int intellect = 0;
                int wisdom = 0;
                uniqBridge.Add(currentUniq, characterUniq);

                foreach (PlayerModels.Models.CharacterModel cm in characterModels)
                {
                    if (cm.uniq == characterUniq)
                    {
                        name = cm.name;
                        maxHP = cm.stats.maxHP;
                        maxMP = cm.stats.maxMP;
                        classType = cm.currentClass;
                        level = cm.lvl;
                        strength = cm.stats.strength;
                        vitality = cm.stats.vitality;
                        agility = cm.stats.agility;
                        intellect = cm.stats.intellect;
                        wisdom = cm.stats.wisdom;
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

                pcs.Add(characterUniq, new FullCombatCharacter() {
                   name = name,
                   maxHP = maxHP,
                   hp =  hp,
                   maxMP = maxMP,
                   mp = mp,
                   characterUniq = characterUniq,
                   combatUniq = currentUniq,
                   className = classType,
                   level = level,
                   strength = strength,
                   vitality = vitality,
                   intellect = intellect,
                   agility = agility,
                   wisdom = wisdom
                });
                combatCharacterModels.Add(new PlayerModels.CombatDataModels.CombatCharacterModel()
                {
                    characterUniq = characterUniq,
                    mods = new List<PlayerModels.CombatDataModels.CombatModificationsModel>(),
                    stats = new PlayerModels.CombatDataModels.TemporaryCombatStatsModel() { hp = hp, mp = mp },
                    nextAttackTime = calculateNextAttackTime(0, initiativeCalculator(), agility),
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
            combatNPCModels = new List<PlayerModels.CombatDataModels.CombatCharacterModel>();
            foreach (MapDataClasses.MapDataClasses.Enemy enemy in encounter.enemies)
            {
                this.npcs.Add(currentUniq, new FullCombatCharacter(){
                    name = enemy.name,
                    hp = enemy.maxHP,
                    maxHP = enemy.maxHP,
                    mp = enemy.maxMP,
                    maxMP = enemy.maxMP,
                    characterUniq = 0,
                    combatUniq = currentUniq,
                    className = enemy.type,
                    level = enemy.level,
                    strength = enemy.strength,
                    vitality = enemy.vitality,
                    agility = enemy.agility,
                    intellect = enemy.intellect,
                    wisdom = enemy.wisdom
                });
                   
                this.combatNPCModels.Add(new PlayerModels.CombatDataModels.CombatCharacterModel()
                {
                    name = enemy.name,
                    stats = new PlayerModels.CombatDataModels.TemporaryCombatStatsModel()
                    {
                        hp = enemy.maxHP,
                        mp = enemy.maxMP
                    },
                    nextAttackTime = calculateNextAttackTime(0, initiativeCalculator(), enemy.agility),
                    combatUniq = currentUniq,
                    characterUniq = 0
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
            int currentCharacter = getCurrentPC();
            if (currentCharacter != 0)
            {
                returnValue.Add(new Command(false, new List<ICommand>(), false, 0, 0, "Attack", false, 0, true));
                returnValue.Add(new Command(true, ClassProcessor.ClassProcessor.getClassAbilities(pcs[uniqBridge[currentCharacter]].className, pcs[uniqBridge[currentCharacter]].level), false, 0, 0, "Abilities", false, 0, false));
                returnValue.Add(new Command(false, new List<ICommand>(), false, 0, 0, "Guard", false, 0, false));
                returnValue.Add(new Command(false, new List<ICommand>(), false, 0, 0, "Flee", false, 0, false));
            }
            return returnValue;
        }

        public ICombatStatus getStatus()
        {
            List<ICharacterDisplay> pcDisplays = new List<ICharacterDisplay>();
            string fastestPCName = string.Empty;
            int fastestPCTime = int.MaxValue;
            foreach(int key in pcs.Keys) {
                FullCombatCharacter current = pcs[key];
                pcDisplays.Add(new CharacterDisplay(current.name, current.hp, current.maxHP, current.mp, current.maxMP, new List<IStatusDisplay>(), current.combatUniq, current.turnOrder,current.className, current.level));
                if (pcs[key].turnOrder < fastestPCTime)
                {
                    fastestPCTime = pcs[key].turnOrder;
                    fastestPCName = pcs[key].name;
                }
            }
            List<ICharacterDisplay> npcDisplays = new List<ICharacterDisplay>();
            foreach (int key in npcs.Keys)
            {
                FullCombatCharacter current = npcs[key];
                npcDisplays.Add(new CharacterDisplay(current.name, current.hp, current.maxHP, current.mp, current.maxMP, new List<IStatusDisplay>(), current.combatUniq, current.turnOrder, current.className, current.level));
            }

            return new CombatStatus(fastestPCName, currentEffects, pcDisplays, npcDisplays);
        }

        public ICombatStatus executeCommand(SelectedCommand command)
        {
            throw new NotImplementedException();
        }

        private int getCurrentPC()
        {
            int combatUniq = 0;
            int fastestPCTime = int.MaxValue;
            foreach (int key in pcs.Keys)
            {
                if (pcs[key].turnOrder < fastestPCTime)
                {
                    fastestPCTime = pcs[key].turnOrder;
                    combatUniq = pcs[key].combatUniq;
                }
            }

            return combatUniq;
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
                foreach (PlayerModels.CombatDataModels.CombatCharacterModel npc in combatNPCModels)
                {
                    if (!usedUniqs.Contains(npc.combatUniq) && npc.nextAttackTime < fastestTime)
                    {
                        currentFastestUniq = npc.combatUniq;
                        fastestTime = npc.nextAttackTime;
                        isPC = false;
                    }
                }

                foreach (PlayerModels.CombatDataModels.CombatCharacterModel pc in combatCharacterModels)
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
                    pcs[uniqBridge[currentFastestUniq]].turnOrder = i + 1;
                }
                else
                {
                    npcs[currentFastestUniq].turnOrder = i + 1;
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
