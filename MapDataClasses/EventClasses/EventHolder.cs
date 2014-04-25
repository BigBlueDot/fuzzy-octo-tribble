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
        public static Dictionary<int, IEventData> events = new Dictionary<int, IEventData>();

        static EventHolder()
        {
            List<Enemy> enemies = new List<Enemy>();
            enemies.Add(MapDataManager.getEnemy("Emergence Cavern", "Goblin"));
            events.Add(1, new CombatEventData(new Encounter()
            {
                enemies = enemies,
                message = "A single goblin blocks your path!"
            }));
        }

        public static IEventData getMapEvent(int uniq)
        {
            if(events.ContainsKey(uniq))
            {
                return events[uniq];
            }
            return new EventData(false, string.Empty, 0, EventDataType.Combat);
        }
    }
}
