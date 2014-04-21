using PlayerModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerModels.StatCalculations
{
    public interface IStatCalculator
    {
        void getStats(CharacterModel cm);
        IEnumerable<AbilityDescription> getAbilities(CharacterModel cm);
        string getNewAbilityMessage(CharacterModel cm);
    }
}
