using CombatDataClasses.AbilityProcessing.EnemyAbilityProcessing;
using CombatDataClasses.AbilityProcessing.ModificationsGeneration;
using CombatDataClasses.ClassProcessor;
using CombatDataClasses.Interfaces;
using MapDataClasses.MapDataClasses;
using PlayerModels.CombatDataModels;
using PlayerModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombatDataClasses.LiveImplementation
{
    public class Combat : ICombat
    {
        private FullCombatCharacter currentCharacter;
        private bool currentCharacterIsPC;
        private Dictionary<int, int> uniqBridge;
        private PlayerModels.PlayerModel playerModel;
        private List<IEffect> currentEffects;
        private Dictionary<int, FullCombatCharacter> pcs;
        private Dictionary<int, FullCombatCharacter> npcs;
        private List<CombatCharacterModel> combatCharacterModels; //These are stored so they can be updated
        private List<CombatCharacterModel> combatNPCModels;
        private CombatData combatData;
        private Action onGameOver;
        private Action onUpdate;
        private Action onCombatComplete;
        private string map;
        private int currentTime
        {
            get
            {
                if (playerModel.currentCombat == null)
                {
                    return 0;
                }
                return playerModel.currentCombat.currentTime;
            }
            set
            {
                if (playerModel.currentCombat != null)
                {
                    playerModel.currentCombat.currentTime = value;
                }
            }
        }

        public Combat(PlayerModels.PlayerModel playerModel, string map, int encounterSelection, Func<float> initiativeCalculator, Action onGameOver, Action onUpdate, Action onCombatComplete)
        {
            int currentUniq = 1;
            this.playerModel = playerModel;
            uniqBridge = new Dictionary<int, int>();
            pcs = new Dictionary<int, FullCombatCharacter>();
            npcs = new Dictionary<int, FullCombatCharacter>();
            currentEffects = new List<IEffect>();
            currentCharacter = new FullCombatCharacter();
            this.onGameOver = onGameOver;
            this.onUpdate = onUpdate;
            this.onCombatComplete = onCombatComplete;
            this.map = map;

            List<PlayerModels.Models.PartyCharacterModel> partyCharacterModels = PlayerModels.PlayerDataManager.getCurrentPartyPartyStats(playerModel);
            List<int> characterUniqs = new List<int>();
            foreach (PlayerModels.Models.PartyCharacterModel pcm in partyCharacterModels)
            {
                characterUniqs.Add(pcm.characterUniq);
            }
            List<PlayerModels.Models.CharacterModel> characterModels = PlayerModels.PlayerDataManager.getCurrentParty(playerModel, characterUniqs);
            bool hasPreviousCombatModels = false;
            if (playerModel.currentCombat == null)
            {
                combatCharacterModels = new List<PlayerModels.CombatDataModels.CombatCharacterModel>();
                combatNPCModels = new List<CombatCharacterModel>();
                playerModel.currentCombat = new CombatModel();
                playerModel.currentCombat.currentTime = 0;
                playerModel.currentCombat.pcs = combatCharacterModels;
                playerModel.currentCombat.npcs = combatNPCModels;
            }
            else
            {
                combatCharacterModels = playerModel.currentCombat.pcs;
                combatNPCModels = playerModel.currentCombat.npcs;
                hasPreviousCombatModels = true;
            }

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
                int nextAttackTime = 0;
                int classLevel = 0;
                int combatUniq = currentUniq;
                List<CombatModificationsModel> mods = new List<CombatModificationsModel>();
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

                        foreach (PlayerModels.Models.CharacterClassModel ccm in cm.characterClasses)
                        {
                            if (ccm.className == classType)
                            {
                                classLevel = ccm.lvl;
                            }
                        }
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

                nextAttackTime = GeneralProcessor.calculateNextAttackTime(0, initiativeCalculator(), agility);
                
                if (hasPreviousCombatModels)
                {
                    foreach (CombatCharacterModel ccm in combatCharacterModels)
                    {
                        if (ccm.characterUniq == characterUniq)
                        {
                            nextAttackTime = ccm.nextAttackTime;
                            hp = ccm.stats.hp;
                            mp = ccm.stats.mp;
                            combatUniq = ccm.combatUniq;
                        }
                    }
                }
                else {
                    combatCharacterModels.Add(new PlayerModels.CombatDataModels.CombatCharacterModel()
                    {
                        characterUniq = characterUniq,
                        stats = new PlayerModels.CombatDataModels.TemporaryCombatStatsModel() { hp = hp, mp = mp },
                        nextAttackTime = nextAttackTime,
                        combatUniq = combatUniq
                    });
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
                   classLevel = classLevel,
                   level = level,
                   strength = strength,
                   vitality = vitality,
                   intellect = intellect,
                   agility = agility,
                   wisdom = wisdom,
                   nextAttackTime = nextAttackTime,
                   mods = new List<CombatModificationsModel>()
                });

                combatCharacterModels[combatCharacterModels.Count - 1].mods = pcs[characterUniq].mods;

                currentUniq++;
            }

            if (hasPreviousCombatModels) //Combat was already initialized previously, just load the previous combatants
            {
                foreach (CombatCharacterModel ccm in combatNPCModels)
                {
                    int hp = ccm.stats.hp;
                    int mp = ccm.stats.mp;
                    int nextAttackTime = ccm.nextAttackTime;
                    List<CombatModificationsModel> mods = ccm.mods;

                    MapDataClasses.MapDataClasses.Enemy enemy = MapDataClasses.MapDataManager.getEnemy(map, ccm.classType);

                    if (!this.npcs.ContainsKey(currentUniq))
                    {
                        this.npcs.Add(currentUniq, new FullCombatCharacter()
                        {
                            name = enemy.name,
                            hp = hp,
                            maxHP = enemy.maxHP,
                            mp = mp,
                            maxMP = enemy.maxMP,
                            characterUniq = 0,
                            combatUniq = currentUniq,
                            className = enemy.type,
                            level = enemy.level,
                            strength = enemy.strength,
                            vitality = enemy.vitality,
                            agility = enemy.agility,
                            intellect = enemy.intellect,
                            wisdom = enemy.wisdom,
                            nextAttackTime = nextAttackTime,
                            mods = mods
                        });
                    }

                    currentUniq++;
                }
            }
            else //Generate a new encounter based on the map type
            {
                Encounter encounter = MapDataClasses.MapDataManager.getRandomEncounter(map, encounterSelection);
                currentEffects.Add(new Effect(EffectTypes.Message, 0, encounter.message, 0));
                combatNPCModels = new List<PlayerModels.CombatDataModels.CombatCharacterModel>();
                foreach (MapDataClasses.MapDataClasses.Enemy enemy in encounter.enemies)
                {
                    int nextAttackTime = GeneralProcessor.calculateNextAttackTime(0, initiativeCalculator(), enemy.agility);
                    int hp = enemy.maxHP;
                    int mp = enemy.maxMP;
                    int combatUniq = currentUniq;
                    List<CombatModificationsModel> mods = new List<CombatModificationsModel>();

                    this.npcs.Add(currentUniq, new FullCombatCharacter()
                    {
                        name = enemy.name,
                        hp = hp,
                        maxHP = enemy.maxHP,
                        mp = mp,
                        maxMP = enemy.maxMP,
                        characterUniq = 0,
                        combatUniq = currentUniq,
                        className = enemy.type,
                        level = enemy.level,
                        strength = enemy.strength,
                        vitality = enemy.vitality,
                        agility = enemy.agility,
                        intellect = enemy.intellect,
                        wisdom = enemy.wisdom,
                        nextAttackTime = nextAttackTime,
                        mods = mods
                    });


                    this.combatNPCModels.Add(new PlayerModels.CombatDataModels.CombatCharacterModel()
                    {
                        name = enemy.name,
                        stats = new PlayerModels.CombatDataModels.TemporaryCombatStatsModel()
                        {
                            hp = enemy.maxHP,
                            mp = enemy.maxMP
                        },
                        nextAttackTime = nextAttackTime,
                        combatUniq = currentUniq,
                        characterUniq = 0,
                        classType = enemy.type,
                        mods = this.npcs[currentUniq].mods
                    });

                    currentUniq++;
                }
            }

            playerModel.currentCombat.npcs = combatNPCModels;
            if (combatNPCModels.Count == 0)
            {
                throw new Exception("No NPCs were loaded :(");
            }

            calculateTurnOrder();
            combatData = new CombatData();

            if (!combatData.combatInitalized)
            {
                foreach (int key in pcs.Keys) //Check for initial code
                {
                    FullCombatCharacter fcc = pcs[key];
                    List<IEffect> newEffects = GeneralProcessor.initialExecute(fcc)(getAllPcsAsList(), getAllNpcsAsList(), combatData);
                    currentEffects.AddRange(newEffects);
                }

                combatData.combatInitalized = true;
            }
            calculateTurn(false);
        }
        
        public List<ICommand> getCommands()
        {
            List<ICommand> returnValue = new List<ICommand>();
            int currentCharacter = getCurrentPC();
            if (currentCharacter != 0)
            {
                returnValue.Add(new Command(false, new List<ICommand>(), false, 0, 0, "Attack", false, 0, true));
                returnValue.Add(new Command(true, ClassProcessor.AbilityDirector.getClassAbilities(pcs[uniqBridge[currentCharacter]].className, pcs[uniqBridge[currentCharacter]].classLevel), false, 0, 0, "Abilities", false, 0, false));
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
                List<IStatusDisplay> statuses = new List<IStatusDisplay>();
                foreach (CombatModificationsModel cmm in current.mods)
                {
                    statuses.Add(new StatusDisplay(Interfaces.Type.Text, cmm.name));
                }
                CharacterDisplay cd = new CharacterDisplay(current.name, current.hp, current.maxHP, current.mp, current.maxMP, statuses, current.combatUniq, current.turnOrder, current.className, current.level);
                cd.setGlanceStats(current.strength, current.vitality, current.intellect, current.wisdom, current.agility);
                pcDisplays.Add(cd);
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
                List<IStatusDisplay> statuses = new List<IStatusDisplay>();
                bool hasGlance = false;
                foreach (CombatModificationsModel cmm in current.mods)
                {
                    if (cmm.name == "Glance")
                    {
                        hasGlance = true;
                    }
                    statuses.Add(new StatusDisplay(Interfaces.Type.Text, cmm.name));
                }
                CharacterDisplay cd = new CharacterDisplay(current.name, current.hp, current.maxHP, current.mp, current.maxMP, statuses, current.combatUniq, current.turnOrder, current.className, current.level);
                if (hasGlance)
                {
                    cd.setGlanceStats(current.strength, current.vitality, current.intellect, current.wisdom, current.agility);
                }
                npcDisplays.Add(cd);
            }

            return new CombatStatus(fastestPCName, currentEffects, pcDisplays, npcDisplays);
        }

        public ICombatStatus executeCommand(SelectedCommand command)
        {
            FullCombatCharacter source = currentCharacter;
            List<FullCombatCharacter> targets = new List<FullCombatCharacter>();
            SelectedCommand testCommand = command;
            while (testCommand != null)
            {
                if (testCommand.targets != null)
                {
                    foreach (int uniq in testCommand.targets)
                    {
                        FullCombatCharacter target = getTarget(uniq);
                        targets.Add(target);
                    }
                }
                testCommand = testCommand.subCommand;
            }
            if (targets.Count == 0) //Pass in all enemies if no single target was selected
            {
                foreach (int key in npcs.Keys)
                {
                    targets.Add(npcs[key]);
                }
            }
            Func<FullCombatCharacter, List<FullCombatCharacter>, CombatData, List<IEffect>> cmdExecute = AbilityDirector.executeCommand(command);
            currentEffects = cmdExecute(source, targets, combatData);
            combatData.setFirstTurnOver(source.name);
            BasicModificationsGeneration.endTurnForUser(getAllPcsAsList(), currentCharacter.name);
            BasicModificationsGeneration.endTurnForUser(getAllNpcsAsList(), currentCharacter.name);
            currentEffects.Add(new Effect(EffectTypes.TurnEnded, 0, string.Empty, 0));
            checkCombatEnded();
            return getStatus();
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

        private void calculateTurnOrder()
        {
            List<int> usedUniqs = new List<int>();
            for (int i = 0; i < checkCharacterListDeath(npcs, true, true) + checkCharacterListDeath(pcs, true); i++)
            {
                int currentFastestUniq = 1;
                int fastestTime = int.MaxValue;
                bool isPC = false;
                foreach (int key in npcs.Keys)
                {
                    FullCombatCharacter npc = npcs[key];
                    if (!usedUniqs.Contains(npc.combatUniq) && npc.nextAttackTime < fastestTime)
                    {
                        currentFastestUniq = npc.combatUniq;
                        fastestTime = npc.nextAttackTime;
                        isPC = false;
                        if (i == 0)
                        {
                            currentCharacter = npc;
                            currentCharacterIsPC = false;
                        }
                    }
                }

                foreach (int key in pcs.Keys)
                {
                    FullCombatCharacter pc = pcs[key];
                    if (!usedUniqs.Contains(pc.combatUniq) && pc.nextAttackTime < fastestTime)
                    {
                        currentFastestUniq = pc.combatUniq;
                        fastestTime = pc.nextAttackTime;
                        isPC = true;
                        if (i == 0)
                        {
                            currentCharacter = pc;
                            currentCharacterIsPC = true;
                        }
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

            if (currentCharacterIsPC)
            {
                currentEffects.Add(new Effect(EffectTypes.ShowCommand, 0, string.Empty, 0));
            }
            else
            {
                List<IEffect> enemyEffects = BasicAbilityProcessing.getCommand(currentCharacter, getAllPcsAsList(), combatData);
                combatData.setFirstTurnOver(currentCharacter.name);
                BasicModificationsGeneration.endTurnForUser(getAllPcsAsList(), currentCharacter.name);
                BasicModificationsGeneration.endTurnForUser(getAllNpcsAsList(), currentCharacter.name);
                foreach (IEffect e in enemyEffects)
                {
                    currentEffects.Add(e);
                }
                currentEffects.Add(new Effect(EffectTypes.TurnEnded, 0, string.Empty, 0));
            }

            //Update Combat Data Models
            updateDataModels();
        }

        private void updateDataModels()
        {
            //Update the models list
            foreach (CombatCharacterModel ccm in combatCharacterModels)
            {
                foreach (int key in pcs.Keys)
                {
                    FullCombatCharacter fcc = pcs[key];
                    if (fcc.characterUniq == ccm.characterUniq)
                    {
                        ccm.nextAttackTime = fcc.nextAttackTime;
                        ccm.stats.hp = fcc.hp;
                        ccm.stats.mp = fcc.mp;
                        ccm.name = fcc.name;
                        ccm.classType = fcc.className;
                    }
                }
            }

            foreach (CombatCharacterModel ccm in combatNPCModels)
            {
                foreach (int key in npcs.Keys)
                {
                    FullCombatCharacter fcc = npcs[key];
                    if (fcc.characterUniq == ccm.characterUniq)
                    {
                        ccm.nextAttackTime = fcc.nextAttackTime;
                        ccm.stats.hp = fcc.hp;
                        ccm.stats.mp = fcc.mp;
                    }
                }
            }
            onUpdate();
        }

        private List<FullCombatCharacter> getAllPcsAsList()
        {
            List<FullCombatCharacter> characters = new List<FullCombatCharacter>();
            foreach (int key in pcs.Keys)
            {
                characters.Add(pcs[key]);
            }
            return characters;
        }

        private List<FullCombatCharacter> getAllNpcsAsList()
        {
            List<FullCombatCharacter> characters = new List<FullCombatCharacter>();
            foreach (int key in npcs.Keys)
            {
                characters.Add(npcs[key]);
            }
            return characters;
        }

        private FullCombatCharacter getTarget(int targetCombatUniq)
        {
            foreach (int key in pcs.Keys)
            {
                if (pcs[key].combatUniq == targetCombatUniq)
                {
                    return pcs[key];
                }
            }

            foreach (int key in npcs.Keys)
            {
                if (npcs[key].combatUniq == targetCombatUniq)
                {
                    return npcs[key];
                }
            }

            return null;
        }

        private int checkCharacterListDeath(Dictionary<int, FullCombatCharacter> characters, bool justGetCount, bool isNPC = false)
        {
            int livingCount = characters.Count;
            foreach (int key in characters.Keys)
            {
                if (!characters[key].defeated && characters[key].hp <= 0)
                {
                    characters[key].hp = 0;
                    characters[key].turnOrder = 0;
                    characters[key].nextAttackTime = int.MaxValue;
                    if (!justGetCount)
                    {
                        characters[key].defeated = true;
                        currentEffects.Add(new Effect(EffectTypes.DestroyCharacter, characters[key].combatUniq, string.Empty, 0));
                        if (isNPC)
                        {
                            //Calculate xp and cp
                            MapDataClasses.MapDataClasses.Enemy enemy = MapDataClasses.MapDataManager.getEnemy(map, characters[key].className);
                            getXPAndCP(enemy.xp, enemy.cp);
                        }
                    }
                    livingCount--;
                }
                else if (characters[key].defeated)
                {
                    livingCount--;
                }
            }

            return livingCount;
        }

        private void checkCombatEnded()
        {
            foreach (Effect e in currentEffects)
            {
                if (e.type == EffectTypes.CombatEnded)
                {
                    onCombatComplete();
                }
            }
        }

        private void getXPAndCP(int xp, int cp)
        {
            currentEffects.Add(new Effect(EffectTypes.Message, 0, "You received " + xp.ToString() + " XP and " + cp.ToString() + " CP.", 0));
            foreach (int key in pcs.Keys)
            {
                FullCombatCharacter fcc = pcs[key];
                foreach (CharacterModel cm in playerModel.characters)
                {
                    if (cm.uniq == fcc.characterUniq)
                    {
                        cm.xp += xp;
                        if (cm.xp >= PlayerModels.PlayerDataManager.getXPForLevel(cm.lvl))
                        {
                            currentEffects.Add(new Effect(EffectTypes.Message, 0, fcc.name + " has gained a level!", 0));
                            cm.lvl++;
                            PlayerModels.StatCalculations.StatCalculator.updateCharacterStats(cm);
                        }
                        foreach (CharacterClassModel ccm in cm.characterClasses)
                        {
                            if (ccm.className == cm.currentClass)
                            {
                                ccm.cp += cp;
                                if (ccm.cp >= PlayerModels.PlayerDataManager.getCPForLevel(ccm.lvl))
                                {
                                    currentEffects.Add(new Effect(EffectTypes.Message, 0, fcc.name + " has gained a class level!", 0));
                                    ccm.lvl++;
                                    PlayerModels.StatCalculations.StatCalculator.updateCharacterStats(cm);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void checkDeath()
        {
            int livingPcs = checkCharacterListDeath(pcs, false);
            int livingNpcs = checkCharacterListDeath(npcs, false, true);

            if (livingPcs == 0)
            {
                currentEffects.Remove(currentEffects[currentEffects.Count - 1]); //Remove the last effect, as it will be an end turn effect
                currentEffects.Add(new Effect(EffectTypes.GameOver, 0, "", 0));
                currentEffects.Add(new Effect(EffectTypes.Message, 0, "You have been defeated!", 0));
                currentEffects.Add(new Effect(EffectTypes.CombatEnded, 0, string.Empty, 0));
                removeEndOfTurnEffects();
                checkCombatEnded();
                onGameOver();
            }
            else if (livingNpcs == 0)
            {
                currentEffects.Add(new Effect(EffectTypes.Message, 0, "You have emerged victorious!", 0));
                currentEffects.Add(new Effect(EffectTypes.CombatEnded, 0, string.Empty, 0));
                removeEndOfTurnEffects();
                checkCombatEnded();
                onCombatComplete();
            }
        }

        private void removeEndOfTurnEffects()
        {
            List<IEffect> toRemove = new List<IEffect>();
            foreach (IEffect e in currentEffects)
            {
                if (e.type == EffectTypes.TurnEnded)
                {
                    toRemove.Add(e);
                }
            }

            foreach (IEffect e in toRemove)
            {
                currentEffects.Remove(e);
            }
        }


        public ICombatStatus nextTurn()
        {
            calculateTurnOrder();
            currentTime = currentCharacter.nextAttackTime;
            BasicModificationsGeneration.checkModifications(getAllPcsAsList(), currentTime);
            BasicModificationsGeneration.checkModifications(getAllNpcsAsList(), currentTime);
            calculateTurn(true);
            checkDeath();
            return getStatus();
        }
    }
}
