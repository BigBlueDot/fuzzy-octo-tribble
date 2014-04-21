using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerModels.Models
{
    public class CharacterQuestProgressModel
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int uniq { get; set; }
        public string questName { get; set; }
        public int questProgressId { get; set; }
        public int currentProgress { get; set; }
        public int goal { get; set; }
    }
}
