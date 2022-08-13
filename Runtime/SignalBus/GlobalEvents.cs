using System;
using System.Collections.Generic;

namespace SBN.Events
{
    // TODO:
    // GlobalEvents<TEvent>, try it. 

    public static class GlobalEvents
    {
        private static Dictionary<Type, List<Action<object>>> eventListeners = new Dictionary<Type, List<Action<object>>>();

        public static void Publish<TEvent>(object args) where TEvent : new()
        {
            var type = typeof(TEvent);

            if (eventListeners.TryGetValue(type, out var listeners))
            {
                for (int i = 0; i < listeners.Count; i++)
                    listeners[i].Invoke(args);
            }
        }

        public static void Subscribe<TEvent>(Action<object> callback) where TEvent : new()
        {
            var type = typeof(TEvent);

            if (!eventListeners.ContainsKey(type))
                eventListeners.Add(type, new List<Action<object>>());

            var listeners = eventListeners[type];

            if (!listeners.Contains(callback))
                listeners.Add(callback);
        }

        public static void Unsubscribe<TEvent>(Action<object> callback) where TEvent : new()
        {
            var type = typeof(TEvent);

            if (eventListeners.TryGetValue(type, out var listeners))
                listeners.Remove(callback);
        }
    }
}