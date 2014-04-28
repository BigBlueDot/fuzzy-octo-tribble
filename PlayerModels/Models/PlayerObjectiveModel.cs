using MapDataClasses.EventClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerModels.Models
{
    public class PlayerObjectiveModel
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int uniq { get; set; }
        public ObjectiveType type { get; set; }
    }
}
