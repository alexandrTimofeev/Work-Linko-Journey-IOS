using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MG_PhysicsDraw2D
{
    public enum PD2Event { OnUpdate, OnPointerDown, OnPointer, OnPointerUp }

    public class PD2_EventManager
    {
        Dictionary<PD2Event, UnityEvent> _eventDictionaryObject = new Dictionary<PD2Event, UnityEvent>();

        public void StartListening(PD2Event eventName, UnityAction listener)
        {
            UnityEvent thisEvent = null;
            if (_eventDictionaryObject.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.AddListener(listener);
            }
            else
            {
                thisEvent = new UnityEvent();
                thisEvent.AddListener(listener);
                _eventDictionaryObject.Add(eventName, thisEvent);
            }
        }

        public void StopListening(PD2Event eventName, UnityAction listener)
        {
            UnityEvent thisEvent = null;
            if (_eventDictionaryObject.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.RemoveListener(listener);
            }
        }

        public void TriggerEvent(PD2Event eventName)
        {
            UnityEvent thisEvent = null;
            if (_eventDictionaryObject.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.Invoke();
            }
        }
    }
}