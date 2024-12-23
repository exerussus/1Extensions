using System;
using System.Collections.Generic;
using Unity.Plastic.Newtonsoft.Json;
using UnityEditor;
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

        public readonly string name;
        public readonly string id = GUID.Generate().ToString();
        public readonly int order;
        public readonly long timestamp = DateTime.UtcNow.Ticks;
        public readonly Dictionary<string, object> context = new();
    }
    
    public class DictionaryConverter : JsonConverter<Dictionary<string, object>>
    {
        public override void WriteJson(JsonWriter writer, Dictionary<string, object> value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            foreach (var kvp in value)
            {
                writer.WritePropertyName(kvp.Key);
                serializer.Serialize(writer, kvp.Value);
            }
            writer.WriteEndObject();
        }

        public override Dictionary<string, object> ReadJson(JsonReader reader, Type objectType, Dictionary<string, object> existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var result = new Dictionary<string, object>();
            while (reader.Read() && reader.TokenType != JsonToken.EndObject)
            {
                var key = reader.Value.ToString();
                reader.Read();
                var value = serializer.Deserialize(reader);
                result[key] = value;
            }
            return result;
        }
    }
    
    public static class EventExtensions
    {
        
        public static bool HasContext(this Event @event, string key)
        {
            return @event.context?.ContainsKey(key) ?? false;
        }

        public static T GetContext<T>(this Event @event, string key)
        {
            if (@event.context == null || !@event.context.ContainsKey(key))
            {
#if UNITY_EDITOR
                Debug.LogError($"[Event] Context key '{key}' not found.");
#endif
                return default;
            }
            return (T)@event.context[key];
        }

        public static T GetOrAddContext<T>(this Event @event, string key, T defaultValue = default)
        {
            if (@event.context.TryGetValue(key, out var value)) return (T)value;
            @event.context[key] = defaultValue;
            return defaultValue;
        }
        
        public static bool HasContext(this Dictionary<string, object> context, string key)
        {
            return context?.ContainsKey(key) ?? false;
        }

        public static T GetContext<T>(this Dictionary<string, object> context, string key)
        {
            if (context == null || !context.ContainsKey(key))
            {
#if UNITY_EDITOR
                Debug.LogError($"[Event] Context key '{key}' not found.");
#endif
                return default;
            }
            return (T)context[key];
        }

        public static T GetOrAddContext<T>(this Dictionary<string, object> context, string key, T defaultValue = default)
        {
            if (context.TryGetValue(key, out var value)) return (T)value;
            context[key] = defaultValue;
            return defaultValue;
        }
        
        public static string Serialize(this Event @event)
        {
            return JsonConvert.SerializeObject(@event, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                Converters = new List<JsonConverter> { new DictionaryConverter() }
            });
        }

        public static Event Deserialize(this Event @event, string json)
        {
            return JsonConvert.DeserializeObject<Event>(json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                Converters = new List<JsonConverter> { new DictionaryConverter() }
            });
        }
    }
}