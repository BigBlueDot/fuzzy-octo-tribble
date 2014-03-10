using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapDataClasses
{
    public class ClientMap
    {
        public string name { get; set; }
        public ClientMapSquare[][] mapSquares { get; set; }
        public bool isDungeon { get; set; }
    }
}
