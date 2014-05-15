using MapDataClasses.MapDataClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapDataClasses.EventClasses
{
    public class EventHolder
    {
        public static Dictionary<int, EventDataModel> events = new Dictionary<int, EventDataModel>();

        static EventHolder()
        {
            List<Enemy> enemies = new List<Enemy>();
            enemies.Add(MapDataManager.getEnemy("Emergence Cavern", "Goblin"));
            enemies.Add(MapDataManager.getEnemy("Emergence Cavern", "Goblin"));
            events.Add(1, EventDataModel.getCombatEvent(new Encounter(isEvent:true)
            {
                enemies = enemies,
                message = "A horde of Goblins attacks!",
            }, ObjectiveType.EmergenceCavernAdditionalAdventurer));

            enemies = new List<Enemy>();
            enemies.Add(MapDataManager.getEnemy("Emergence Cavern", "Blue Slime"));
            enemies.Add(MapDataManager.getEnemy("Emergence Cavern", "Blue Slime"));
            events.Add(2, EventDataModel.getCombatEvent(new Encounter(isEvent:true)
            {
                enemies = enemies,
                message = "Twin Slimes appear!"
            }, ObjectiveType.EmergenceCavernB2));

            enemies = new List<Enemy>();
            enemies.Add(MapDataManager.getEnemy("Emergence Cavern", "Boss Goblin"));
            events.Add(3, EventDataModel.getCombatEvent(new Encounter(isEvent: true)
            {
                enemies = enemies,
                message = "A well dressed Goblin appears!"
            }, ObjectiveType.EmergenceCavernB2));
        }

        public static EventDataModel getMapEvent(int uniq)
        {
            if(events.ContainsKey(uniq))
            {
                return events[uniq];
            }
            return new EventDataModel(false, string.Empty, 0, EventDataType.Combat);
        }
    }
}
