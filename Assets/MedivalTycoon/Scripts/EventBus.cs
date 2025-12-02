
using System;
using System.Collections.Generic;

public static class EventBus
{
    private static Dictionary<Type, Delegate> _events = new Dictionary<Type, Delegate>();

    public static void Subscribe<T>(Action<T> callback)
    {
        if (_events.TryGetValue(typeof(T), out var existing))
        {
            _events[typeof(T)] = Delegate.Combine(existing, callback);
        }
        else
        {
            _events[typeof(T)] = callback;
        }
    }

    public static void Unsubscribe<T>(Action<T> callback)
    {
        if (_events.TryGetValue(typeof(T), out var existing))
        {
            var current = Delegate.Remove(existing, callback);
            if (current == null)
                _events.Remove(typeof(T));
            else
                _events[typeof(T)] = current;
        }
    }

    public static void Raise<T>(T eventData)
    {
        if (_events.TryGetValue(typeof(T), out var del))
        {
            ((Action<T>)del)?.Invoke(eventData);
        }
    }
}

