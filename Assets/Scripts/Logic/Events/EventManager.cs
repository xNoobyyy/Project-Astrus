using System;
using System.Collections.Generic;
using UnityEngine;

namespace Logic.Events {
    public class EventManager : MonoBehaviour {
        public static EventManager Instance { get; private set; }
        private Dictionary<Type, Delegate> events;

        private void Awake() {
            if (Instance == null) {
                Instance = this;
            } else {
                Destroy(gameObject);
                return;
            }

            events = new Dictionary<Type, Delegate>();
        }

        public void Subscribe<T>(Action<T> listener) {
            if (events.TryGetValue(typeof(T), out var existingDelegate)) {
                events[typeof(T)] = (existingDelegate as Action<T>) + listener;
            } else {
                events[typeof(T)] = listener;
            }
        }

        public void Unsubscribe<T>(Action<T> listener) {
            if (events.TryGetValue(typeof(T), out var existingDelegate)) {
                events[typeof(T)] = (existingDelegate as Action<T>) - listener;
            }
        }

        public void Trigger<T>(T eventObject) {
            if (events.TryGetValue(typeof(T), out var existingDelegate)) {
                (existingDelegate as Action<T>)?.Invoke(eventObject);
            }
        }
    }
}