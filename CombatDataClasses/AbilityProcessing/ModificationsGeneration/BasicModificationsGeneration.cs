using CombatDataClasses.LiveImplementation;
using PlayerModels.CombatDataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombatDataClasses.AbilityProcessing.ModificationsGeneration
{
    public class BasicModificationsGeneration
    {
        public static void checkModifications(List<FullCombatCharacter> characters, int time)
        {
            foreach (FullCombatCharacter fcc in characters)
            {
                List<CombatModificationsModel> toRemove = new List<CombatModificationsModel>();
                foreach (CombatModificationsModel cmm in fcc.mods)
                {
                    foreach (CombatConditionModel ccm in cmm.conditions)
                    {
                        if (ccm.name == "Time")
                        {
                            if (Convert.ToInt32(ccm.state) <= time)
                            {
                                toRemove.Add(cmm);
                            }
                        }
                    }
                }

                foreach (CombatModificationsModel cmm in toRemove)
                {
                    fcc.mods.Remove(cmm);
                }
            }
        }

        public static void endTurnForUser(List<FullCombatCharacter> characters, string characterEnding)
        {
            foreach (FullCombatCharacter fcc in characters)
            {
                List<CombatModificationsModel> toRemove = new List<CombatModificationsModel>();
                foreach (CombatModificationsModel cmm in fcc.mods)
                {
                    foreach (CombatConditionModel ccm in cmm.conditions)
                    {
                        if (ccm.name == "TurnEnding")
                        {
                            if (ccm.state == characterEnding)
                            {
                                toRemove.Add(cmm);
                            }
                        }
                    }
                }

                foreach (CombatModificationsModel cmm in toRemove)
                {
                    fcc.mods.Remove(cmm);
                }
            }
        }

        public static CombatModificationsModel getGuardModification(string characterName)
        {
            CombatModificationsModel cmm = new CombatModificationsModel();
            cmm.name = "Guard";
            cmm.conditions = new List<CombatConditionModel>();
            cmm.conditions.Add(new CombatConditionModel() { name = "TurnEnding", state = characterName });
            return cmm;
        }

        public static bool hasMod(FullCombatCharacter fcc, string modName)
        {
            foreach (CombatModificationsModel cmm in fcc.mods)
            {
                if (cmm.name == modName)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
