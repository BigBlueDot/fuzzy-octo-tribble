using CombatDataClasses.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombatDataClasses.LiveImplementation
{
    public class Effect : IEffect
    {
        public Effect(EffectTypes type, int targetUniq, string message, int value)
        {
            _type = type;
            _targetUniq = targetUniq;
            _message = message;
            _value = value;
        }

        private EffectTypes _type;
        public EffectTypes type
        {
            get { return _type; }
        }

        private int _targetUniq;
        public int targetUniq
        {
            get { return _targetUniq; }
        }

        private string _message;
        public string message
        {
            get { return _message; }
        }

        private int _value;
        public int value
        {
            get { return _value; }
        }
    }
}
