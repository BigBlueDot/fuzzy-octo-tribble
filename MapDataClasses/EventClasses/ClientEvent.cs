using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapDataClasses
{
    public class ClientEvent
    {
        public int x { get; set; }
        public int y { get; set; }
        public RewardType rewardType { get; set; }

        public enum RewardType
        {
            Objective,
            CompletedObjective,
            Gold,
            XP,
            CP,
            Item
        }
    }

}
