using System;
using System.Collections.Generic;

namespace SBN.Events
{
    /// <summary>
    /// Very basic generic global events system.
    /// Can be used to publish, subscribe and unsubscribe to events 
    /// all over the application. 
    /// 
    /// This eventhandler also makes sure there's no accidental 
    /// 'double subscriptions' by using a HashSet. 
    /// 
    /// Events (TEvent) must be a struct.
    /// </summary>
    public static class GlobalEvents<TEvent> where TEvent : struct
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