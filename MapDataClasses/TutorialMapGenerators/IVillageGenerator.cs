using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapDataClasses.TutorialMapGenerators
{
    public interface IVillageGenerator
    {
        MapModel getMap();
        MapInteraction getInteraction(MapModel mm, int x, int y);
    }
}
