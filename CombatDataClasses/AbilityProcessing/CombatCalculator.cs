using CombatDataClasses.LiveImplementation;
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
                attackValue += fcc.classLevel;
            }
            return attackValue;
        }
    }
}
