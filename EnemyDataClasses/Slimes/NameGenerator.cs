using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnemyDataClasses.Slimes
{
    public class NameGenerator
    {
        private static int currentCount = 0;
        private static List<string> names = new List<string>();

        static NameGenerator()
        {
            names.Add("Wiggly");
            names.Add("Fluidity");
            names.Add("Jumpy");
            names.Add("Gobble");
            names.Add("Tank");
            names.Add("Heady");
            names.Add("Blubba");
            names.Add("Goobly");
            names.Add("Roger");
            names.Add("Boodly");
            names.Add("Goozanna");
            names.Add("Riggy");
            names.Add("Ice House");
            names.Add("Melon");
            names.Add("Topply");
            names.Add("Plopstar");
            names.Add("Peewee");
            names.Add("Riddly");
            names.Add("Fiffy");
            names.Add("Googly");
            names.Add("Biff");
            names.Add("Zoomly");
            names.Add("Bubbilly");
            names.Add("Eruvve");
            names.Add("Mibbly");
            names.Add("Cackle");
            names.Add("Slimer");
            names.Add("Gak");
            names.Add("Rocket");
            names.Add("Metal");
            names.Add("Suraimu");
        }

        public static string getSlimeName()
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
