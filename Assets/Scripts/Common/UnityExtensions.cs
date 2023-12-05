using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Common
{
    public static class UnityExtensions
    {
        public static T Instantiate<T>(this GameObject prefab, Transform parent = null) where T : Component
        {
            return GameObject.Instantiate(prefab, parent).GetComponent<T>();
        }

        public static Vector2 Clamp(this Rect rect, Vector2 point)
        {
            return new(
                Mathf.Clamp(point.x, rect.xMin, rect.xMax),
                Mathf.Clamp(point.y, rect.yMin, rect.yMax)
            );
        }

        public static EventTrigger GetEventTrigger(this GameObject gameObject)
        {
            var trigger = gameObject.GetComponent<EventTrigger>();
            if (trigger == null)
                trigger = gameObject.AddComponent<EventTrigger>();
            return trigger;
        }

        public static void AddCallback(this EventTrigger eventTrigger, EventTriggerType eventID, Action callback)
        {
            var entry = eventTrigger.triggers.Find(t => t.eventID == eventID);
            if (entry == null)
            {
                entry = new();
                entry.eventID = eventID;
                eventTrigger.triggers.Add(entry);
            }
            entry.callback.AddListener(_ => callback.Invoke());
        }
    }
}
