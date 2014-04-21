using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerModels.Models
{
    public class CharacterClassModel
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int uniq { get; set; }
        public string className { get; set; }
        public int cp { get; set; }
        [NotMapped]
        public int cpToLevel
        {
            get
            {
                return PlayerDataManager.getCPForLevel(this.lvl);
            }
        }
        public int lvl { get; set; }
    }
}
