using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapDataClasses.EventClasses
{
    public class MapEventCollection
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int uniq { get; set; }
        private List<MapEvent> events { get; set; }

        public MapEventCollection()
        {
            events = new List<MapEvent>();
        }

        public void addEvent(MapEvent e)
        {
            e.eventId = events.Count;
            events.Add(e);
        }

        public MapEvent getEvent(int uniq)
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

        public List<MapEvent> getAll()
        {
            return events;
        }
    }
}
