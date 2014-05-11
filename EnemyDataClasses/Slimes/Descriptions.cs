using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnemyDataClasses.Slimes
{
    public class Descriptions : IDescriptions
    {
        public bool isType(string name)
        {
            if (name == "Blue Slime")
            {
                return true;
            }
            return false;
        }

        public string getDescription(string type)
        {
            switch (type)
            {
                case "Blue Slime":
                    return "Blue Slimes come from a far away land.  They are not very fast.";
                default:
                    return string.Empty;
            }
        }
    }
}
