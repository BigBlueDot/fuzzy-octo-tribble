using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapDataClasses.EventClasses
{
    public class MapEventCollectionModel
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int uniq { get; set; }
        public List<MapEventModel> events { get; set; }

        public MapEventCollectionModel()
        {
            events = new List<MapEventModel>();
        }

        public void addEvent(MapEventModel e)
        {
            e.eventId = events.Count;
            events.Add(e);
        }

        public MapEventModel getEvent(int uniq)
        {
            if (events.Count > uniq)
            {
                return events[uniq];
            }
            else
            {
                throw new Exception("No event with that identifier exists.");
            }
        }

        public List<MapEventModel> getAll()
        {
            return events;
        }

        public void removeEvent(MapEventModel e)
        {
            List<MapEventModel> toRemove = new List<MapEventModel>();
            List<Coordinate> toCheck = new List<Coordinate>();
            foreach (MapEventModel mem in events)
            {
                if (mem.uniq == e.uniq)
                {
                    toRemove.Add(mem);
                    toCheck.Add(new Coordinate() { x = mem.x, y = mem.y });
                }
            }

            while (toCheck.Count != 0)
            {
                Coordinate current = toCheck[0];
                toCheck.RemoveAt(0);

                foreach (MapEventModel mem in events)
                {
                    if (toRemove.Contains(mem))
                    {
                        continue;
                    }
                    var xDiff = Math.Abs(mem.x - current.x);
                    var yDiff = Math.Abs(mem.y - current.y);
                    if (xDiff + yDiff == 1) //If it is exactly one space away
                    {
                        toRemove.Add(mem);
                        toCheck.Add(new Coordinate() { x = mem.x, y = mem.y });
                    }
                }
            }

            foreach(MapEventModel mem in toRemove)
            {
                events.Remove(mem);
            }
        }

        public class Coordinate
        {
            public int x { get; set; }
            public int y { get; set; }
        }
    }
}
