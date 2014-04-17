using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnemyDataClasses.Goblins
{
    public class NameGenerator
    {
        private static int currentCount = 0;
        private static List<string> names = new List<string>();

        static NameGenerator()
        {
            names.Add("Gobel");
            names.Add("Hector");
            names.Add("Gobbelin");
            names.Add("Cowlair");
            names.Add("Pillo");
            names.Add("Gobling");
            names.Add("CobGob Jr.");
            names.Add("Malli");
            names.Add("Dimitri");
            names.Add("Goberyn");
            names.Add("Scrapper");
            names.Add("\"Ratface\"");
            names.Add("Cowcadden");
            names.Add("Gobeline");
            names.Add("CobGob");
            names.Add("Tyke");
            names.Add("Coblyn");
            names.Add("Unlucky One");
            names.Add("Xavier");
            names.Add("Hijack");
            names.Add("Dokkaebi");
            names.Add("Knocker");
            names.Add("Goblyn");
            names.Add("Stubbs");
        }

        public static string getGoblinName()
        {
            currentCount++;
            if (currentCount >= names.Count)
            {
                currentCount = 0;
            }
            return names[currentCount];
        }
    }
}
