using System;
using UnityEngine;

namespace Maru.MCore {
    public abstract class VentBehavior : MonoBehaviour {
        protected IMessageBus Vent;
        protected ILocator Locator;
        protected Action[] Unregister;
        private int ventIdx = 0;

        protected abstract string VentLocatorKey { get; }

        protected virtual int EventCapacity => 0;

        protected virtual void Awake() {
            Locator = LocatorProvider.Get();
            Vent = Locator.Get(VentLocatorKey) as IMessageBus;
            Unregister = new Action[EventCapacity];
                
        }

        protected Action On<TEvent>(Action<TEvent> handler) {
            var off = Vent.On(handler);
            Unregister[ventIdx++] = off;
            return off;
        }

        protected Action On<TEvent>(string key, Action<TEvent> handler) where TEvent : IKeyedEvent {
            var off = Vent.On(key, handler);
            Unregister[ventIdx++] = off;
            return off;
        }

        protected Action Once<TEvent>(Action<TEvent> handler) {
            var off = Vent.Once(handler);
            Unregister[ventIdx++] = off;
            return off;
        }
        
        protected Action Once<TEvent>(string key, Action<TEvent> handler) where TEvent: IKeyedEvent {
            var off = Vent.Once(key, handler);
            Unregister[ventIdx++] = off;
            return off;
        }

        protected virtual void OnDestroy() {
            foreach (var off in Unregister) {
                off();
            }
        }
    }
}
