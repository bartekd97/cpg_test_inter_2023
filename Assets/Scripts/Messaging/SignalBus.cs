using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Messaging
{
    public static class SignalBus
    {
        static Dictionary<Type, IList> _Listeners = new();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void Init()
        {
            _Listeners = new();
        }

        public static void AddListener<T>(Action<T> callback) where T : ISignal
        {
            GetList<T>().Add(callback);
        }
        public static void RemoveListener<T>(Action<T> callback) where T : ISignal
        {
            GetList<T>().Remove(callback);
        }
        public static void Fire<T>(T signal) where T : ISignal
        {
            GetList<T>().ForEach(callback =>
            {
                try
                {
                    callback.Invoke(signal);
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
            });
        }

        static List<Action<T>> GetList<T>()
        {
            if (!_Listeners.TryGetValue(typeof(T), out var list))
            {
                list = new List<Action<T>>();
                _Listeners.Add(typeof(T), list);
            }
            return (List<Action<T>>)list;
        }
    }
}
