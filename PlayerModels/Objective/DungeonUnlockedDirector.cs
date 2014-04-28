using MapDataClasses.EventClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerModels.Objective
{
    public class DungeonUnlockedDirector
    {
        public static bool isDungeonUnlocked(string name, PlayerModel pm)
        {
            switch (name)
            {
                case "Emergence Cavern B2":
                    return pm.isObjectiveCompleted(ObjectiveType.EmergenceCavernB2);
            }

            return true;
        }
    }
}
