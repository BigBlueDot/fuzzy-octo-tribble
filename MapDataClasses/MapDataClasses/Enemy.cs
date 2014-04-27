using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapDataClasses.MapDataClasses
{
    public class Enemy
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int uniq { get; set; }
        public string name { get; set; }
        public int level { get; set; }
        public string type { get; set; }
        public int maxHP { get; set; }
        public int maxMP { get; set; }
        public int strength { get; set; }
        public int vitality { get; set; }
        public int intellect { get; set; }
        public int wisdom { get; set; }
        public int agility { get; set; }
        public int xp { get; set; }
        public int cp { get; set; }
    }
}
