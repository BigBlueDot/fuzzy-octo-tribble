using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombatDataClasses.Interfaces
{
    public interface IEffect
    {
        //This for of effect will be presented in a list of stuff that could happen.  It should always end with the next command by the user, unless combat is ended
        //Beforehand
        EffectTypes type { get; }
        int targetUniq { get; }
        string message { get; }
        int value { get; }  //dmg or heal count
    }

    public enum EffectTypes
    {
        Message,
        DealDamage,
        HealDamage,
        DestroyCharacter,
        CombatEnded,
        GameOver,
        ShowCommand
    }
}
