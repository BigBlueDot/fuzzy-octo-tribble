using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDataClasses
{
    public class GameRNG
    {
        private Random _random;
        private Random random
        {
            get
            {
                if (_random == null)
                {
                    _random = new Random();
                }
                return _random;
            }
        }

        public GameRNG()
        {

        }

        /// <summary>
        /// Get a random number
        /// </summary>
        /// <param name="min">The inclusive minimum.  The smallest number that will be returned.s</param>
        /// <param name="max">The inclusive maximum.  The largets number that will be returned.</param>
        /// <returns></returns>
        public int getNumber(int min, int max)
        {
            return random.Next(min, max + 1);
        }

        public float calculateIntiative()
        {
            return 0.5f + (float)random.NextDouble();
        }
    }
}
