using CombatDataClasses.Interfaces;
using CombatDataClasses.LiveImplementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombatDataClasses.ClassProcessor
{
    public class GeneralProcessor
    {
        public static bool isProcessor(string name)
        {
            switch (name)
            {
                case "Attack":
                    return true;
                default:
                    return false;
            }
        }

        public static Func<FullCombatCharacter, FullCombatCharacter, List<IEffect>> executeCommand(string name)
        {
            switch (name)
            {
                case "Attack":
                    return ((FullCombatCharacter source, FullCombatCharacter target) =>
                    {
                        List<IEffect> effects = new List<IEffect>();
                        int dmg = (int)((source.strength * 5 / target.vitality));
                        effects.Add(new Effect(EffectTypes.DealDamage, target.combatUniq, string.Empty, dmg));
                        target.hp -= dmg;
                        return effects;
                    });
                default:
                    return ((FullCombatCharacter source, FullCombatCharacter target) =>
                    {
                        List<IEffect> effects = new List<IEffect>();
                        return effects;
                    });
            }
        }
    }
}
