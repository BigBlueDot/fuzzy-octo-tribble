using MapDataClasses.EventClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapDataClasses
{
    public class MapModel
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int uniq { get; set; }
        public string name { get; set; }
        public string[,] map { get; set; }
        public int startX { get; set; }
        public int startY { get; set; }
        public MapEventCollectionModel eventCollection { get; set; }
        public MapEventModel activeEvent { get; set; }
    }
}
