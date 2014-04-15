using CombatDataClasses.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombatDataClasses.LiveImplementation
{
    public class StatusDisplay : IStatusDisplay
    {
        public StatusDisplay(Interfaces.Type type, string value)
        {            
            _type = type;
            _value = value;
        }

        private Interfaces.Type _type;
        public Interfaces.Type type
        {
            get { return _type; }
        }

        private string _value;
        public string value
        {
            get { return _value; }
        }
    }
}
