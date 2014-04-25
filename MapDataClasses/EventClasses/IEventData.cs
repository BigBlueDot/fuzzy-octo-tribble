using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapDataClasses.EventClasses
{
    public interface IEventData
    {
        bool hasMessage { get;  }
        string message { get;  }
        int eventId { get;  }
        EventDataType type { get;  } 
    }
    public enum EventDataType
    {
        Combat
    }
}