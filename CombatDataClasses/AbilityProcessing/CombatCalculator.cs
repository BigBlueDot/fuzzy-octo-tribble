using CombatDataClasses.LiveImplementation;
using PlayerModels.CombatDataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombatDataClasses.AbilityProcessing
{
    public class CombatCalculator
    {
        public static int getNormalAttackValue(FullCombatCharacter fcc)
        {
            int attackValue = fcc.strength;
            if (fcc.className == "Brawler")
            {
                //Add bonus points for fist speciality
                attackValue += ((int)(fcc.classLevel / 5) + 1);
            }

            if (ModificationsGeneration.BasicModificationsGeneration.hasMod(fcc, "Disarmed"))
            {
                attackValue = (int)(attackValue * .75);
            }

            return attackValue;
        }

        public static int getMagicAttackValue(FullCombatCharacter fcc)
        {
            int attackValue = fcc.intellect;

            return attackValue;
        }

        public static void removeRanged(FullCombatCharacter fcc)
        {
            CombatModificationsModel toRemove = null;
            foreach(CombatModificationsModel mod in fcc.mods)
            {
                if (mod.name == "Ranged")
                {
                    toRemove = mod;
                }
            }

            if (toRemove != null)
            {
                fcc.mods.Remove(toRemove);
            }
        }
    }
}
