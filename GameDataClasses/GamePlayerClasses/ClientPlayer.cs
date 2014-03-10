using PlayerModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDataClasses.GamePlayerClasses
{
    public class ClientPlayer
    {
        public int x { get; set; }
        public int y { get; set; }
        public int gp { get; set; }
        public List<CharacterModel> characters { get; set; }
        public List<int> currentPartyCharacters { get; set; } 
    }
}
