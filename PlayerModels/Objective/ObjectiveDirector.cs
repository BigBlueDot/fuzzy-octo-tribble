using MapDataClasses.EventClasses;
using PlayerModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerModels.Objective
{
    public class ObjectiveDirector
    {
        public static string getCompletedText(ObjectiveType type)
        {
            switch (type)
            {
                case ObjectiveType.EmergenceCavernAdditionalAdventurer:
                    return "You have unlocked an additional character!  You will be able to add them to your party the next time you enter a dungeon.";
                case ObjectiveType.EmergenceCavernB2:
                    return "You have unlocked Emergence Cavern B2.";
                case ObjectiveType.Potion:
                    return "The Potion item is now available at the Item Store.  Please note that item uses regenerate when exiting a dungeon.";
                default:
                    return string.Empty;
            }
        }

        public static string completeObjective(PlayerModel pm, ObjectiveType objective)
        {
            if (!pm.isObjectiveCompleted(objective))
            {
                if (objective == ObjectiveType.EmergenceCavernAdditionalAdventurer)
                {
                    PlayerDataManager.addCharacter(pm);
                }
                pm.objectives.Add(new PlayerObjectiveModel()
                {
                    type = objective
                });
                return getCompletedText(objective);
            }

            return string.Empty;
        }

        public static void markCompletedObjectives(PlayerModel pm)
        {
            MapDataClasses.MapModel mm = pm.getActiveParty().location;
            if (mm != null)
            {
                foreach (MapDataClasses.MapEventModel mem in mm.eventCollection.getAll())
                {
                    if (mem.rewardType == MapDataClasses.ClientEvent.RewardType.Objective)
                    {
                        if (pm.isObjectiveCompleted(mem.eventData.objective))
                        {
                            mem.rewardType = MapDataClasses.ClientEvent.RewardType.CompletedObjective;
                        }
                    }
                }
            }
        }
    }
}
