using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnemyDataClasses
{
    public interface IDescriptions
    {
        bool isType(string name);
        string getDescription(string name);
    }
}
