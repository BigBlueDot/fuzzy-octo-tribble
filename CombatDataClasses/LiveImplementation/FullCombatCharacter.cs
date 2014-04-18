using PlayerModels.CombatDataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombatDataClasses.LiveImplementation
{
    public class FullCombatCharacter
    {
        public string name { get; set; }
        public List<CombatModificationsModel> mods { get; set; }
        public int maxHP { get; set; }
        public int maxMP { get; set; }
        public int hp { get; set; }
        public int mp { get; set; }
        public int strength { get; set; }
        public int vitality { get; set; }
        public int intellect { get; set; }
        public int wisdom { get; set; }
        public int agility { get; set; }
        public int characterUniq { get; set; }
        public int nextAttackTime { get; set; }
        public int combatUniq { get; set; }
        public string className { get; set; }
        public int level { get; set; }
        public int classLevel { get; set; }
        public int turnOrder { get; set; }
        public bool defeated { get; set; }

        public void inflictDamage(ref int damage)
        {
            foreach (CombatModificationsModel cmm in mods)
            {
                if (cmm.name == "Guard")
                {
                    damage = damage / 2;
                }
                if (cmm.name == "Reckless")
                {
                    damage = (int)(damage * 1.5);
                }
            }

            hp -= damage;
        }
    }
}
