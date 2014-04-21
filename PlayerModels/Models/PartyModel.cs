using MapDataClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerModels.Models
{
    public class PartyModel
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int uniq { get; set; }
        public List<PartyCharacterModel> characters { get; set; }
        public int maxSize { get; set; }
        public MapModel location { get; set; }
        public int x { get; set; }
        public int y { get; set; }
    }

    public class PartyCharacterModel
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int uniq { get; set; }
        public int characterUniq { get; set; }
        public int hp { get; set; }
        public int mp { get; set; }
        public List<string> usedAbilities { get; set; }
    }
}
