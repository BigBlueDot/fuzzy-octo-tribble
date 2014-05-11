using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnemyDataClasses
{
    public class DescriptionDirector
    {
        public static List<IDescriptions> descriptions;

        static DescriptionDirector()
        {
            descriptions = new List<IDescriptions>();
            descriptions.Add(new Goblins.Descriptions());
            descriptions.Add(new Slimes.Descriptions());
        }

        public static string getDescription(string type)
        {
            foreach (IDescriptions d in descriptions)
            {
                if (d.isType(type))
                {
                    return d.getDescription(type);
                }
            }

            return string.Empty;
        }
    }
}
