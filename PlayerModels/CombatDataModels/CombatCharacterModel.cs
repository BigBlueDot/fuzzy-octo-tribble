using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerModels.CombatDataModels
{
    public class CombatCharacterModel
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int uniq { get; set; }
        public string name { get; set; }
        public TemporaryCombatStatsModel stats { get; set; }
        public List<CombatModificationsModel> mods { get; set; }
        public int characterUniq { get; set; }
        public int nextAttackTime { get; set; }
        public int combatUniq { get; set; }
        public string classType { get; set; }
    }
}
