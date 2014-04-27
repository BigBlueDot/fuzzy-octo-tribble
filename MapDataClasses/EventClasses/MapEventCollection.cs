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
            MapEventModel toRemove = null;
            foreach (MapEventModel mem in events)
            {
                if (mem.uniq == e.uniq)
                {
                    toRemove = mem;
                }
            }

            if (toRemove != null)
            {
                events.Remove(toRemove);
            }
        }
    }
}
