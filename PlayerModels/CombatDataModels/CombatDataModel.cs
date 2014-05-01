using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerModels.CombatDataModels
{
    public class CombatDataModel
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int uniq { get; set; }
        public int currentFleeCount { get; set; }
        public List<TurnOverModel> firstTurnOver { get; set; }
        public bool combatInitalized { get; set; }
        public List<CooldownModel> cooldowns { get; set; }
        public bool canFlee { get; set; }

        public class TurnOverModel
        {
            [Key]
            [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
            public int uniq { get; set; }
            public string name { get; set; }
        }
    }
}
