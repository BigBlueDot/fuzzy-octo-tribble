using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnemyDataClasses.Goblins
{
    public class Descriptions : IDescriptions
    {
        public bool isType(string name)
        {
            if (name == "Goblin" || name == "Boss Goblin")
            {
                return true;
            }
            return false;
        }

        public string getDescription(string type)
        {
            switch (type)
            {
                case "Goblin":
                    return "Goblins are generally nocturnal creatures.  Once a year, during the Festival of Light, there is a competition to see which Goblin can stay awake the longest in a well-lit room.";
                case "Boss Goblin":
                    return "The name \"Boss Goblin\" actually comes from a mistranslation of the Goblin word for \"Stylish.\"";
                default:
                    return string.Empty;
            }
        }
    }
}
