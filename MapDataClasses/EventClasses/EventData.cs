﻿using MapDataClasses.MapDataClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapDataClasses.EventClasses
{
    public enum EventDataType
    {
        Combat,
        EmergenceCavernCeilingCollapse
    }

    public class EventDataModel 
    {
        public EventDataModel()
        {

        }

        public EventDataModel(bool hasMessage, string message, int eventId, EventDataType eventDataType)
        {

        }

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int uniq { get; set; }

        public bool hasMessage
        {
            get;
            set;
        }

        public string message
        {
            get;
            set;
        }

        public int eventId
        {
            get;
            set;
        }

        public EventDataType type
        {
            get;
            set;
        }

        public EventDataModel nextEvent
        {
            get;
            set;
        }

        public Encounter encounter
        {
            get;
            set;
        }

        public ObjectiveType objective
        {
            get;
            set;
        }

        public static EventDataModel getTrapEvent(EventDataType eventDataType, string message, ObjectiveType objective = ObjectiveType.None)
        {
            EventDataModel edm = new EventDataModel();
            edm.hasMessage = true;
            edm.message = message;
            edm.type = eventDataType;
            edm.nextEvent = null;
            edm.objective = objective;

            return edm;
        }

        public static EventDataModel getCombatEvent(Encounter encounter, ObjectiveType objective = ObjectiveType.None)
        {
            EventDataModel edm = new EventDataModel();
            edm.encounter = encounter;
            edm.hasMessage = false;
            edm.message = string.Empty;
            edm.type = EventDataType.Combat;
            edm.nextEvent = null;
            edm.objective = objective;

            return edm;
        }

        public static EventDataModel getMultiCombatEvent(List<Encounter> encounters, ObjectiveType objective = ObjectiveType.None)
        {
            EventDataModel edm = new EventDataModel();
            edm.hasMessage = false;
            edm.message = string.Empty;
            edm.type = EventDataType.Combat;
            edm.objective = objective;

            EventDataModel currentEvent = edm;

            foreach (Encounter encounter in encounters)
            {
                currentEvent.encounter = encounter;

                EventDataModel edm2 = new EventDataModel();
                edm2.encounter = encounter;
                edm2.hasMessage = false;
                edm2.message = string.Empty;
                edm2.type = EventDataType.Combat;
                edm2.nextEvent = null;
                edm2.objective = objective;

                currentEvent.nextEvent = edm2;
                currentEvent = edm2;
            }

            return edm;
        }
    }
}
