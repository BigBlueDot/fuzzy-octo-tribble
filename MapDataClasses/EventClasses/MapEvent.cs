using MapDataClasses.EventClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapDataClasses
{
    public class MapEvent
    {
        public int x { get; set; }
        public int y { get; set; }
        public int eventId { get; set; }
        public ClientEvent.RewardType rewardType { get; set; }
        public IEventData eventData { get; set; }
    }
}
