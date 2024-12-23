using System;
using System.Collections.Generic;
using UnityEngine;

namespace Exerussus._1Extensions.GameEvents
{
    public class EventTracker
    {
        private readonly Dictionary<string, Event> _events = new();
        private readonly Dictionary<string, List<Event>> _eventsByName = new();
        
        public int LastEventIndex { get; private set; } = -1;
        public bool HasAnyEvent => LastEventIndex > -1;
        public bool LogTraceEnabled { get; set; }
        public bool LogErrorEnabled { get; set; }

        public EventTracker SetLogTrace(bool isEnabled = true)
        {
            LogTraceEnabled = isEnabled;
            return this;
        }

        public EventTracker SetLogError(bool isEnabled = true)
        {
            LogErrorEnabled = isEnabled;
            return this;
        }
        
        public void ClearOldEvents(int keepCount)
        {
            foreach (var list in _eventsByName.Values)
            {
                if (list.Count > keepCount)
                {
                    list.RemoveRange(0, list.Count - keepCount);
                }
            }
        }
        
        public bool TryGetLastEventByName(string eventName, out Event @event)
        {
            if (!_eventsByName.TryGetValue(eventName, out var eventsList))
            {
                @event = null;
                return false;
            }
            
            @event = eventsList[^1];
            return true;
        }
        
        public Event GetLastEventByName(string eventName)
        {
            return _eventsByName[eventName][^1];
        }
        
        public Event GetEvent(string eventId)
        {
            if (!_events.TryGetValue(eventId, out var @event))
            {
                if (LogErrorEnabled) Debug.LogError($"[EventTracker] Event not found: {eventId}");
            }
            return @event;
        }
        
        public bool TryGetEvent(string eventId, out Event @event)
        {
            return _events.TryGetValue(eventId, out @event);
        }

        public Event NewEvent(string eventName)
        {
            LastEventIndex++;
            var signalEvent = new Event(eventName, LastEventIndex);
            
            if (LogTraceEnabled)
            {
                Debug.Log("[EventTracker] Created new Event:\n" +
                          $"id : {signalEvent.id}\n" +
                          $"name : {signalEvent.name}\n" +
                          $"order : {signalEvent.order}\n" +
                          $"timestamp : {signalEvent.timestamp}");
            }
            
            if (!_eventsByName.TryGetValue(eventName, out var eventList)) _eventsByName[eventName] = eventList = new List<Event>();
            eventList.Add(signalEvent);
            _events.Add(signalEvent.id, signalEvent);
            return signalEvent;
        }
    }
    
    [Serializable]
    public class Event
    {
        public Event(string eventName, int order)
        {
            name = eventName;
            this.order = order;
        }

        [SerializeField] public string name;
        [SerializeField] public string id = Guid.NewGuid().ToString();
        [SerializeField] public int order;
        [SerializeField] public long timestamp = DateTime.UtcNow.Ticks;
        [SerializeField, HideInInspector] private string serializedContext;

        private Dictionary<string, object> _context = new();

        public Dictionary<string, object> Context
        {
            get => _context;
            set => _context = value;
        }

        [SerializeField] public List<EventContextPair> contextPairs = new();

        [ContextMenu("Serialize Context")]
        public string Serialize()
        {
            serializedContext = JsonUtility.ToJson(new SerializableDictionary(Context));
            return JsonUtility.ToJson(this);
        }

        [ContextMenu("Deserialize Context")]
        public void DeserializeContext()
        {
            if (!string.IsNullOrEmpty(serializedContext))
            {
                var deserializedDict = JsonUtility.FromJson<SerializableDictionary>(serializedContext);
                Context = deserializedDict.ToDictionary();
            }
        }
    }

    [Serializable]
    public class EventContextPair
    {
        public string Key;
        public string Value;
    }

    [Serializable]
    public class SerializableDictionary
    {
        public List<string> Keys = new();
        public List<string> Values = new();

        public SerializableDictionary() { }

        public SerializableDictionary(Dictionary<string, object> dictionary)
        {
            foreach (var kvp in dictionary)
            {
                Keys.Add(kvp.Key);
                Values.Add(kvp.Value.ToString());
            }
        }

        public Dictionary<string, object> ToDictionary()
        {
            var dict = new Dictionary<string, object>();
            for (int i = 0; i < Keys.Count; i++)
            {
                dict[Keys[i]] = Values[i];
            }
            return dict;
        }
    }

    public static class EventExtensions
    {
        public static bool HasContext(this Event @event, string key)
        {
            return @event.Context?.ContainsKey(key) ?? false;
        }

        public static object GetContext(this Event @event, string key)
        {
            if (!@event.Context.TryGetValue(key, out var value))
            {
#if UNITY_EDITOR
                Debug.LogError($"[Event] Context key '{key}' not found.");
#endif
                return null;
            }
            return value;
        }

        public static T GetContext<T>(this Event @event, string key)
        {
            if (!@event.Context.TryGetValue(key, out var value))
            {
#if UNITY_EDITOR
                Debug.LogError($"[Event] Context key '{key}' not found.");
#endif
                return default;
            }
            
            return (T)value;
        }

        public static object GetOrAddContext(this Event @event, string key, object defaultValue = null)
        {
            if (@event.Context.TryGetValue(key, out var value)) return value;
            
            @event.Context[key] = defaultValue;
            return defaultValue;
        }
    }
}
