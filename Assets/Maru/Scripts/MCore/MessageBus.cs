using System;
using System.Collections.Generic;

namespace Maru.MCore {
    public class MessageBus : IMessageBus {
        protected Dictionary<Type, ICollection<Delegate>> listeners;
        protected Dictionary<Tuple<Type, string>, ICollection<Delegate>> keyedListeners;

        public MessageBus() {
            listeners = new Dictionary<Type, ICollection<Delegate>>();
            keyedListeners = new Dictionary<Tuple<Type, string>, ICollection<Delegate>>();
        }

        public Action On<TEvent>(Action<TEvent> handler) {
            var eventType = typeof(TEvent);
            ICollection<Delegate> eventHandlers;

            if (listeners.ContainsKey(eventType)) {
                eventHandlers = listeners[eventType];
            } else {
                eventHandlers = NewHandlerCollection();
                listeners[eventType] = eventHandlers;
            }

            eventHandlers.Add(handler);
            return () => Unsubscribe(handler);
        }

        public Action On<TEvent>(string key, Action<TEvent> handler) where TEvent : IKeyedEvent {
            if (key == null) {
                return On(handler);
            }

            var eventType = typeof(TEvent);
            ICollection<Delegate> eventHandlers;
            var dictKey = new Tuple<Type, string>(eventType, key);
            if (keyedListeners.ContainsKey(dictKey)) {
                eventHandlers = keyedListeners[dictKey];
            } else {
                eventHandlers = NewHandlerCollection();
                keyedListeners[dictKey] = eventHandlers;
            }

            eventHandlers.Add(handler);

            return () => Unsubscribe(key, handler);
        }

        public Action Once<TEvent>(Action<TEvent> handler) {
            return On<TEvent>(evt => {
                handler(evt);
                Unsubscribe(handler);
            });
        }

        public Action Once<TEvent>(string key, Action<TEvent> handler) where TEvent : IKeyedEvent {
            if (key == null) {
                return Once(handler);
            }

            return On<TEvent>(key, evt => {
                handler(evt);
                Unsubscribe(key, handler);
            });
        }

        public void Unsubscribe<TEvent>(Action<TEvent> handler) {
            var eventType = typeof(TEvent);
            if (listeners.ContainsKey(eventType)) {
                var eventHandlers = listeners[eventType];
                eventHandlers.Remove(handler);

                if (eventHandlers.Count == 0) {
                    RemoveHandlerCollection(eventType);
                }
            }
        }

        public void Unsubscribe<TEvent>(string key, Action<TEvent> handler) where TEvent : IKeyedEvent {
            if (key == null) {
                Unsubscribe(handler);
                return;
            }

            var eventType = typeof(TEvent);
            var dictKey = new Tuple<Type, string>(eventType, key);
            if (keyedListeners.ContainsKey(dictKey)) {
                var eventHandlers = keyedListeners[dictKey];
                eventHandlers.Remove(handler);

                if (eventHandlers.Count == 0) {
                    RemoveHandlerCollection(eventType);
                }
            }
        }

        public void Trigger<TEvent>(TEvent evt) {
            var eventType = typeof(TEvent);
            if (listeners.ContainsKey(eventType)) {
                var eventHandlers = listeners[eventType];
                foreach (var handler in eventHandlers) {
                    ((Action<TEvent>) handler).Invoke(evt);
                }
            }

            if (evt is IKeyedEvent keyedEvent) {
                var key = keyedEvent.GetEventKey();
                var dictKey = new Tuple<Type, string>(eventType, key);
                if (keyedListeners.ContainsKey(dictKey)) {
                    var eventHandlers = keyedListeners[dictKey];
                    foreach (var handler in eventHandlers) {
                        ((Action<TEvent>) handler).Invoke(evt);
                    }
                }
            }
        }

        protected ICollection<Delegate> NewHandlerCollection() {
            return new HashSet<Delegate>();
        }

        protected void RemoveHandlerCollection(Type eventType) {
            listeners.Remove(eventType);
        }
    }
}
