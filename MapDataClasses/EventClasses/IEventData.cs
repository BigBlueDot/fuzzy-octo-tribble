using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapDataClasses.EventClasses
{
    public interface IEventData
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        int uniq { get; set; }
        bool hasMessage { get;  }
        string message { get;  }
        int eventId { get;  }
        EventDataType type { get;  }
        IEventData nextEvent { get; }
    }
    public enum EventDataType
    {
        Combat
    }
}