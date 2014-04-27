using MapDataClasses.EventClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapDataClasses
{
    public class MapEventModel
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int uniq { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int eventId { get; set; }
        public ClientEvent.RewardType rewardType { get; set; }
        public EventDataModel eventData { get; set; }
        public int rewardValue { get; set; }
    }
}
