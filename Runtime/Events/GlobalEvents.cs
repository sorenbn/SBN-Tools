using System;
using System.Collections.Generic;

// https://stackoverflow.com/questions/14518139/how-cast-generic-t-to-action-to-get-the-method-params

namespace SBN.Events
{
    public static class GlobalEvents
    {
        private static Dictionary<Type, List<Action<GameEvent>>> allSubscriptions = new Dictionary<Type, List<Action<GameEvent>>>();

        public static void Publish(GameEvent data)
        {
            if (allSubscriptions.TryGetValue(data.GetType(), out var subscriptions))
            {
                foreach (var subscriber in subscriptions)
                    subscriber?.Invoke(data);
            }
        }

        public static void Subscribe<T>(Action<GameEvent> callback) where T : GameEvent
        {
            var type = typeof(T);

            if (!allSubscriptions.ContainsKey(type))
                allSubscriptions.Add(type, new List<Action<GameEvent>> { });

            // Figure out how to store generic Action<T> callbacks properly
            //Action<object> wrapperCallback = args => callback((T)args);

            allSubscriptions[type].Add(callback);
        }

        public static void Unsubscribe<T>(Action<GameEvent> callback) where T : GameEvent
        {
            var type = typeof(T);

            if (!allSubscriptions.ContainsKey(type))
                return;

            allSubscriptions[type].Remove(callback);
        }
    }
}