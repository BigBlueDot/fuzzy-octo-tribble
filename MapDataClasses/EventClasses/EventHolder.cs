﻿using MapDataClasses.MapDataClasses;
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
            events.Add(1, new CombatEventDataModel(new Encounter()
            {
                enemies = enemies,
                message = "A single goblin blocks your path!"
            }));
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
