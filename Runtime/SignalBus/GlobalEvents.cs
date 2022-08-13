using System;
using System.Collections.Generic;

namespace SBN.Events
{
    public static class GlobalEvents<TEvent> where TEvent : new()
    {
        private static Dictionary<Type, List<Action<TEvent>>> eventListeners = new Dictionary<Type, List<Action<TEvent>>>();

        public static void Publish(TEvent args)
        {
            var type = typeof(TEvent);

            if (eventListeners.TryGetValue(type, out var listeners))
            {
                for (int i = 0; i < listeners.Count; i++)
                    listeners[i].Invoke(args);
            }
        }

        public static void Subscribe(Action<TEvent> callback)
        {
            var type = typeof(TEvent);

            if (!eventListeners.ContainsKey(type))
                eventListeners.Add(type, new List<Action<TEvent>>());

            var listeners = eventListeners[type];

            if (!listeners.Contains(callback))
                listeners.Add(callback);
        }

        public static void Unsubscribe(Action<TEvent> callback)
        {
            var type = typeof(TEvent);

            if (eventListeners.TryGetValue(type, out var listeners))
                listeners.Remove(callback);
        }
    }
}