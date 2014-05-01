using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapDataClasses.MapDataClasses
{
    public class Encounter
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int uniq { get; set; }
        public string message { get; set; }
        public List<Enemy> enemies { get; set; }
        public bool canFlee { get; set; }

        public Encounter()
        {
            canFlee = true;
            enemies = new List<Enemy>();
        }

        public Encounter(bool isEvent)
        {
            if (isEvent)
            {
                canFlee = false;
            }
            else
            {
                canFlee = true;
            }
            enemies = new List<Enemy>();
        }
    }
}
