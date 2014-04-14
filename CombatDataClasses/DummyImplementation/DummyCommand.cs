using CombatDataClasses.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombatDataClasses.DummyImplementation
{
    public class DummyCommand : ICommand
    {
        public DummyCommand(bool hasChildCommands, List<ICommand> childCommands, bool limitedUsage, int uses, string name, bool mpNeeded, int mpCost)
        {
            _hasChildCommands = hasChildCommands;
            _childCommands = childCommands;
            _limitedUsage = limitedUsage;
            _uses = uses;
            _name = name;
            _mpNeeded = mpNeeded;
            _mpCost = mpCost;
        }

        private bool _hasChildCommands;
        public bool hasChildCommands
        {
            get
            {
                return _hasChildCommands;
            }
        }

        private List<ICommand> _childCommands;
        public List<ICommand> childCommands
        {
            get
            {
                return _childCommands;
            }
        }

        private bool _limitedUsage;
        public bool limitedUsage
        {
            get
            {
                return _limitedUsage;
            }
        }

        private int _uses;
        public int uses
        {
            get
            {
                return _uses;
            }
        }

        private string _name;
        public string name
        {
            get
            {
                return _name;
            }
        }

        private bool _mpNeeded;
        public bool mpNeeded
        {
            get
            {
                return _mpNeeded;
            }
        }

        private int _mpCost;
        public int mpCost
        {
            get
            {
                return _mpCost;
            }
        }
    }
}
