using CombatDataClasses.AbilityProcessing.ModificationsGeneration;
using CombatDataClasses.ClassProcessor;
using CombatDataClasses.Interfaces;
using CombatDataClasses.LiveImplementation;
using PlayerModels.CombatDataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombatDataClasses.AbilityProcessing
{
    public class AbilityInfo
    {
        public string name { get; set; }
        public bool ranged { get; set; }
        public float damageCoefficient { get; set; }
        public float damageMultiplier { get; set; }
        public float attackTimeCoefficient { get; set; }
        public DamageType damageType { get; set; }
        public int maxTargets { get; set; }
        public int requiredClassLevel { get; set; }
        public string message { get; set; }
        public int hits { get; set; }
        public string cooldown { get; set; }
        public int cooldownDuration { get; set; }
        public string oncePerRest { get; set; }
        public int mpCost { get; set; }
        public Func<FullCombatCharacter, List<FullCombatCharacter>, CombatData, List<IEffect>, AbilityInfo, ProcessResult> init { get; set; }
        public Func<FullCombatCharacter, List<FullCombatCharacter>, CombatData, List<IEffect>, AbilityInfo, ProcessResult> preExecute { get; set; }
        public Func<FullCombatCharacter, List<FullCombatCharacter>, CombatData, List<IEffect>, AbilityInfo, ProcessResult> postExecute { get; set; }
        public Func<FullCombatCharacter, List<FullCombatCharacter>, CombatData, List<IEffect>, AbilityInfo, ProcessResult> postCleanup { get; set; }

        public AbilityInfo()
        {
            damageCoefficient = 1.0f;
            attackTimeCoefficient = 1.0f;
            damageType = DamageType.None;
            hits = 1;
            maxTargets = 1;
            cooldownDuration = 60;
            oncePerRest = string.Empty;
            cooldown = string.Empty;
            mpCost = 0;
        }

        public Func<FullCombatCharacter, List<FullCombatCharacter>, CombatData, List<IEffect>> getCommand()
        {
            return ((FullCombatCharacter source, List<FullCombatCharacter> targets, CombatData combatData) =>
            {
                List<IEffect> effects = new List<IEffect>();
                if(processFunction(init, source, targets, combatData, effects) == ProcessResult.EndTurn)
                {
                    return effects;
                }
                PreProcess(source, targets, combatData, effects);
                if(processFunction(preExecute, source, targets, combatData, effects) == ProcessResult.EndTurn)
                {
                    return effects;
                }
                Process(source, targets, combatData, effects);
                if(processFunction(postExecute, source, targets, combatData, effects) == ProcessResult.EndTurn)
                {
                    return effects;
                }
                PostProcess(source, targets, combatData, effects);
                if (processFunction(postCleanup, source, targets, combatData, effects) == ProcessResult.EndTurn)
                {
                    return effects;
                }
                return effects;
            });
        }

        private ProcessResult processFunction(Func<FullCombatCharacter, List<FullCombatCharacter>, CombatData, List<IEffect>, AbilityInfo, ProcessResult> executingFunction,
            FullCombatCharacter source,
            List<FullCombatCharacter> targets,
            CombatData combatData,
            List<IEffect> effects)
        {
            if (executingFunction != null)
            {
                return executingFunction(source, targets, combatData, effects, this);
            }
            return ProcessResult.Normal;
        }

        private ProcessResult PreProcess(FullCombatCharacter source, List<FullCombatCharacter> target, CombatData combatData, List<IEffect> effects)
        {
            //Determine if it's a valid skill or on cooldown etc
            if (source.classLevel < requiredClassLevel || combatData.hasCooldown(source.name, cooldown) || target.Count > maxTargets)
            {
                return ProcessResult.EndTurn;
            }

            if (BasicModificationsGeneration.hasMod(source, "Ranged"))
            {
                damageCoefficient = damageCoefficient * .75f;

                if (!ranged)
                {
                    effects.Add(new Effect(EffectTypes.Message, 0, "Using a melee attack has removed the ranged advantage!", 0));
                    CombatCalculator.removeRanged(source);
                }
            }


            if (oncePerRest != string.Empty && source.usedAbilities.Contains(oncePerRest))
            {
                return ProcessResult.EndTurn;
            }

            return ProcessResult.Normal;
        }

        private ProcessResult Process(FullCombatCharacter source, List<FullCombatCharacter> target, CombatData combatData, List<IEffect> effects)
        {
            //Deal damage etc.
            foreach (FullCombatCharacter t in target)
            {
                int dmg = 0;
                if (damageType == DamageType.Physical)
                {
                    dmg = (int)((CombatCalculator.getNormalAttackValue(source) * damageCoefficient * damageMultiplier / t.vitality));

                    for (int i = 0; i < hits; i++)
                    {
                        if (t.inflictDamage(ref dmg) == FullCombatCharacter.HitEffect.Unbalance)
                        {
                            attackTimeCoefficient = attackTimeCoefficient * 2;
                        }
                    }

                    effects.Add(new Effect(EffectTypes.DealDamage, t.combatUniq, string.Empty, dmg));
                    string newMessage = message.Replace("{Name}", source.name)
                        .Replace("{Target}", t.name)
                        .Replace("{Damage}", dmg.ToString())
                        .Replace("{HitCount}", hits.ToString());
                    if (newMessage != string.Empty)
                    {
                        effects.Add(new Effect(EffectTypes.Message, 0, newMessage, 0));
                    }
                }
                else if (damageType == DamageType.Magical)
                {
                    dmg = (int)((CombatCalculator.getMagicAttackValue(source) * damageCoefficient * damageMultiplier / t.wisdom));

                    for (int i = 0; i < hits; i++)
                    {
                        t.inflictDamage(ref dmg);
                    }

                    effects.Add(new Effect(EffectTypes.DealDamage, t.combatUniq, string.Empty, dmg));
                    string newMessage = message.Replace("{Name}", source.name)
                        .Replace("{Target}", t.name)
                        .Replace("{Damage}", dmg.ToString())
                        .Replace("{HitCount}", hits.ToString());
                    if (newMessage != string.Empty)
                    {
                        effects.Add(new Effect(EffectTypes.Message, 0, newMessage, 0));
                    }
                }
            }

            return ProcessResult.Normal;
        }

        private ProcessResult PostProcess(FullCombatCharacter source, List<FullCombatCharacter> target, CombatData combatData, List<IEffect> effects)
        {
            //Set next attack time etc.
            if (cooldown != string.Empty)
            {
                combatData.cooldowns.Add(new CooldownModel
                {
                    character = source.name,
                    name = cooldown,
                    time = (source.nextAttackTime + cooldownDuration)
                });
            }

            GeneralProcessor.calculateNextAttackTime(source, attackTimeCoefficient, combatData);

            if (oncePerRest != string.Empty)
            {
                source.usedAbilities.Add(oncePerRest);
            }

            if (mpCost != 0)
            {
                source.mp = source.mp - mpCost;
            }

            return ProcessResult.Normal;
        }

        public enum DamageType
        {
            None,
            Physical,
            Magical
        }

        public enum ProcessResult
        {
            Normal,
            EndTurn
        }
    }
}
