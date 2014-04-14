using CombatDataClasses.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombatDataClasses.DummyImplementation
{
    public class DummyCharacterDisplay : ICharacterDisplay
    {
        public DummyCharacterDisplay(string name, int hp, int maxHP, int mp, int maxMP, List<IStatusDisplay> statuses, int uniq, int turnOrder)
        {
            _name = name;
            _hp = hp;
            _maxHP = maxHP;
            _mp = mp;
            _maxMP = maxMP;
            _statuses = statuses;
            _uniq = uniq;
            _turnOrder = turnOrder;
        }

        private string _name;
        public string name
        {
            get
            {
                return _name;
            }
        }

        private int _hp;
        public int hp
        {
            get
            {
                return _hp;
            }
        }

        private int _maxHP;
        public int maxHP
        {
            get
            {
                return _maxHP;
            }
        }

        private int _mp;
        public int mp
        {
            get
            {
                return _mp;
            }
        }

        private int _maxMP;
        public int maxMP
        {
            get
            {
                return _maxMP;
            }
        }

        private IEnumerable<IStatusDisplay> _statuses;
        public IEnumerable<IStatusDisplay> statuses
        {
            get
            {
                return _statuses;
            }
        }

        private int _uniq;
        public int uniq
        {
            get {
                return _uniq;
            }
        }

        private int _turnOrder;
        public int turnOrder
        {
            get
            {
                return _turnOrder;
            }
        }
    }
}
