using MapDataClasses.MapDataClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapDataClasses.EventClasses
{
    public class EventData : IEventData
    {
        public EventData(bool hasMessage, string message, int eventId, EventDataType eventDataType)
        {

        }

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int uniq { get; set; }

        private bool _hasMessage;
        public bool hasMessage
        {
            get
            {
                return _hasMessage;
            }
        }

        private string _message;
        public string message
        {
            get
            {
                return message;
            }
        }

        private int _eventId;
        public int eventId
        {
            get
            {
                return _eventId;
            }
        }

        private EventDataType _type;
        public EventDataType type
        {
            get
            {
                return _type;
            }
        }
    }

    public class CombatEventData : IEventData
    {
        public CombatEventData(Encounter encounter)
        {
            _encounter = encounter;
        }

        private Encounter _encounter;
        public Encounter encounter
        {
            get
            {
                return _encounter;
            }
        }
        public bool hasMessage
        {
            get { return false; }
        }

        public string message
        {
            get { return string.Empty; }
        }

        private int _eventId;
        public int eventId
        {
            get { return _eventId; }
        }

        private EventDataType _type;
        public EventDataType type
        {
            get { return _type; }
        }

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int uniq { get; set; }
    }
}
