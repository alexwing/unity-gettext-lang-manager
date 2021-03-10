using System.Collections.Generic;
using UnityEngine.Events;


namespace LangManager
{
    public class EventManager
    {
        private static Dictionary<string, UnityEvent> eventDictionary = new Dictionary<string, UnityEvent>();
        public static void StartListening(string eventName, UnityAction listener)
        {
            UnityEvent thisEvent = null;
            if (eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.AddListener(listener);
            }
            else
            {
                thisEvent = new UnityEvent();
                thisEvent.AddListener(listener);
                eventDictionary.Add(eventName, thisEvent);
            }
        }

        public static void StopListening(string eventName, UnityAction listener)
        {
            UnityEvent thisEvent = null;
            if (eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.RemoveListener(listener);
            }
        }

        public static void TriggerEvent(string eventName)
        {
            UnityEvent thisEvent = null;
            if (eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.Invoke();
            }
        }
    }
}