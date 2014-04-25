using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapDataClasses.EventClasses
{
    public interface IEventData
    {
        bool hasMessage { get; set; }
        string message { get; set; }
        int eventId { get; set; }
        EventDataType type { get; set; } 
    }
    public enum EventDataType
    {
        Combat
    }
}