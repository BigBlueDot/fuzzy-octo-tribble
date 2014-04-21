using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerModels.CombatDataModels
{
    public class CombatModel
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int uniq { get; set; }
        public List<CombatCharacterModel> pcs { get; set; }
        public List<CombatCharacterModel> npcs { get; set; }
        public int currentTime { get; set; }
    }
}
