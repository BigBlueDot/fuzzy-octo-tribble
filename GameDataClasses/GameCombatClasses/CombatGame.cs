using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDataClasses.GameCombatClasses
{
    public class CombatGame
    {
        public List<CombatPC> pcs { get; set; }
        public List<CombatNPC> npcs { get; set; }
        public CombatTurnOrder turnOrders { get; set; }
        public int currentTime { get; set; }

        public CombatGame()
        {
            pcs = new List<CombatPC>();
            npcs = new List<CombatNPC>();
        }

        private void Init(List<PlayerModels.Models.CharacterModel> characters)
        {
            currentTime = 0;
            turnOrders = new CombatTurnOrder();
            foreach (PlayerModels.Models.CharacterModel cm in characters)
            {
                CombatPC pc = new CombatPC();

                pc.characterUniq = cm.uniq;
                pc.stats = new CombatStats();
                pc.stats.maxHP = cm.stats.maxHP;
            }

        }
    }
}
