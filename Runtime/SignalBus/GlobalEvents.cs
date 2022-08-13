using System;
using System.Collections.Generic;

namespace SBN.Events
{
    public static class GlobalEvents<TEvent> where TEvent : new()
    {
        private static HashSet<Action<TEvent>> subscriptions = new HashSet<Action<TEvent>>();

        public static void Publish(TEvent args)
        {
            foreach (var subscription in subscriptions)
                subscription.Invoke(args);
        }

        public static void Subscribe(Action<TEvent> callback)
        {
            subscriptions.Add(callback);
        }

        public static void Unsubscribe(Action<TEvent> callback)
        {
            subscriptions.Remove(callback);
        }
    }
}