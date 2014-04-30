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
            events.Add(1, EventDataModel.getCombatEvent(new Encounter()
            {
                enemies = enemies,
                message = "A single goblin blocks your path!",
            }, ObjectiveType.EmergenceCavernAdditionalAdventurer));

            enemies = new List<Enemy>();
            enemies.Add(MapDataManager.getEnemy("Emergence Cavern", "Goblin"));
            enemies.Add(MapDataManager.getEnemy("Emergence Cavern", "Goblin"));
            enemies.Add(MapDataManager.getEnemy("Emergence Cavern", "Goblin"));
            events.Add(2, EventDataModel.getCombatEvent(new Encounter()
            {
                enemies = enemies,
                message = "A horde of angry Goblins attacks!"
            }, ObjectiveType.EmergenceCavernB2));

            enemies = new List<Enemy>();
            enemies.Add(MapDataManager.getEnemy("Emergence Cavern", "Goblin"));
            List<Encounter> encounters = new List<Encounter>();
            encounters.Add(new Encounter()
            {
                enemies = enemies,
                message = "A single goblin blocks your path!",
            });
            enemies = new List<Enemy>();
            enemies.Add(MapDataManager.getEnemy("Emergence Cavern", "Goblin"));
            encounters.Add(new Encounter()
            {
                enemies = enemies,
                message = "Another goblin has appeared!",
            });
            events.Add(3, EventDataModel.getMultiCombatEvent(encounters, ObjectiveType.Brawler));
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
