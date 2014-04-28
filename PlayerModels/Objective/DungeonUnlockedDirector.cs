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
        public static bool isDungeonUnlocked(string name, List<ObjectiveType> objectives)
        {
            switch (name)
            {
                case "Emergence Cavern B2":
                    return objectives.Contains(ObjectiveType.EmergenceCavernB2);
            }

            return true;
        }
    }
}
