using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombatDataClasses.LiveImplementation
{
    public class CombatData
    {
        public CombatData()
        {
            Init();
        }

        private void Init()
        {
            currentFleeCount = 0;
        }

        public int currentFleeCount { get; set; }
    }
}
