using System;
using System.Collections.Generic;
using UnityEngine;

namespace Maru.MCore {
    public abstract class VentBehavior : MonoBehaviour {
        protected IMessageBus vent;
        protected ILocator locator;
        protected List<Action> unregister;

        protected abstract string VentKey();

        protected virtual int EventCapacity() {
            return 0;
        }

        protected virtual void Awake() {
            locator = LocatorProvider.Get();
            vent = locator.Get(VentKey()) as IMessageBus;
            unregister = new List<Action>(EventCapacity());
        }

        protected Action On<TEvent>(Action<TEvent> handler) {
            var off = vent.On(handler);
            unregister.Add(off);
            return off;
        }

        protected Action On<TEvent>(string key, Action<TEvent> handler) where TEvent : IKeyedEvent {
            var off = vent.On(key, handler);
            unregister.Add(off);
            return off;
        }

        protected Action Once<TEvent>(Action<TEvent> handler) {
            var off = vent.Once(handler);
            unregister.Add(off);
            return off;
        }
        
        protected Action Once<TEvent>(string key, Action<TEvent> handler) where TEvent: IKeyedEvent {
            var off = vent.Once(key, handler);
            unregister.Add(off);
            return off;
        }

        protected virtual void OnDestroy() {
            foreach (var off in unregister) {
                off();
            }
        }
    }
}
