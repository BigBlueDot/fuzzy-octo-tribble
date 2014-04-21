using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerModels.Models
{
    public class CharacterModel
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int uniq { get; set; }
        public string name { get; set; }
        public string currentClass { get; set; }
        public int lvl { get; set; }
        public int xp { get; set; }
        [NotMapped]
        public int xpToLevel
        {
            get
            {
                return PlayerDataManager.getXPForLevel(this.lvl);
            }
        }
        [NotMapped]
        public int cp
        {
            get
            {
                foreach (CharacterClassModel ccm in characterClasses)
                {
                    if (ccm.className == currentClass)
                    {
                        return ccm.cp;
                    }
                }

                return 0;
            }
        }
        [NotMapped]
        public int cpToLevel
        {
            get
            {
                foreach (CharacterClassModel ccm in characterClasses)
                {
                    if (ccm.className == currentClass)
                    {
                        return ccm.cpToLevel;
                    }
                }

                return 0;
            }
        }
        [NotMapped]
        public List<AbilityDescription> abilities { get; set; }
        public StatsModel stats { get; set; }
        public EquipmentModel equipment { get; set; }
        public CharacterQuestModel currentQuest { get; set; }
        public List<CharacterClassModel> characterClasses { get; set; }
        public List<CharacterAbilityModel> activeAbilities { get; set; }
    }

    public class CharacterAbilityModel
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int uniq { get; set; }
        public string name { get; set; }
    }
}
