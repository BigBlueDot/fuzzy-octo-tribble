using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDataClasses
{
    public interface IGameInteractions
    {
        event RestoreHealthToNonActiveHandler onRestoreHealthToNonActive;
        event RestoreMPToNonActiveHandler onRestoreMPToNonActive;
        event SelectCharacterInteractionHandler onSelectCharacterInteraction;
    }

    public delegate void RestoreHealthToNonActiveHandler(object sender, EventArgs e);
    public delegate void RestoreMPToNonActiveHandler(object sender, EventArgs e);
    public delegate void SelectCharacterInteractionHandler(object sender, EventArgs e);
}
