using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Doozy.Engine;
using Maru.MCore;
using UnityEngine;
using static BrassSparrow.Scripts.UI.DoozyEvents;

namespace BrassSparrow.Scripts.UI {
    // Translates Doozy events into Maru events
    public class DoozyEventTranslator {
        // Use string for id, because that's the form the id will take in a game object
        private Dictionary<string, Component> componentDict;

        private IMessageBus vent;
        private GameEventListener doozyListener;
        private Action[] unregister;

        public DoozyEventTranslator(IMessageBus vent, GameEventListener doozyListener) {
            componentDict = new Dictionary<string, Component>();
            this.vent = vent;
            this.doozyListener = doozyListener;
        }

        public void ListenAndTranslate() {
            if (unregister != null) {
                Debug.LogError("Cannot call listen more than once");
                return;
            }

            var count = 0;
            unregister = new Action[2];
            unregister[count++] = vent.On<RegisterUIComponentEvent>(evt => {
                var id = evt.Component.GetInstanceID().ToString();
                componentDict[id] = evt.Component;
            });
            unregister[count++] = vent.On<UnregisterUIComponentEvent>(evt => {
                var id = evt.Component.GetInstanceID().ToString();
                componentDict.Remove(id);
            });

            doozyListener.Event.AddListener(TranslateDoozyEvent);
        }

        public void TranslateDoozyEvent(string evt) {
            var sepPos = evt.IndexOf(Sep);
            var sepPos2 = evt.IndexOf(Sep, sepPos + 1);
            var prefix = evt.Substring(0, sepPos);
            string id, key;
            if (sepPos2 < 0) {
                id = evt.Substring(sepPos + 1);
            } else {
                var idLen = sepPos2 - sepPos - 1;
                id = evt.Substring(sepPos + 1, idLen);
                key = evt.Substring(sepPos2 + 1);
            }

            if (!componentDict.ContainsKey(id)) {
                Debug.Log($"No GameObject registered for Doozy event {evt}");
                return;
            }

            var comp = componentDict[id];

            switch (prefix) {
                case PartSelectedEvent.Prefix:
                    vent.Trigger(new PartSelectedEvent {PartSelector = comp as PartSelector});
                    break;
                case DoozySelfEvent.Prefix:
                    vent.Trigger(new DoozySelfEvent(comp));
                    break;
                default:
                    Debug.Log($"No event handler found for Doozy event {evt}");
                    break;
            }
        }

        public void Shutdown() {
            // remove these first in case another thread registers a Go before we clean the dict
            foreach (var action in unregister) {
                action();
            }

            componentDict = null;
            doozyListener.Event.RemoveListener(TranslateDoozyEvent);
        }
    }

    public class RegisterUIComponentEvent {
        public Component Component;
    }

    public class UnregisterUIComponentEvent {
        public Component Component;
    }

    public struct PartSelectedEvent : IKeyedEvent {
        public const string Prefix = "PartSelector";
        public string Key;
        public PartSelector PartSelector;

        public string GetEventKey() {
            return Key;
        }
    }

    public struct UIComponentEvent {
        public Component Component;
    }

    // Useful for doozy components that want to react tho their own events
    // I can't believe UI events are this insane
    public class DoozySelfEvent : IKeyedEvent {
        public const string Prefix = "DoozySelf";
        private string compId;

        public DoozySelfEvent(Component comp) {
            compId = comp.GetInstanceID().ToString();
        }

        public string GetEventKey() {
            return compId;
        }

        public static string DoozyEventKey(Component comp) {
            var compId = comp.GetInstanceID().ToString();
            return DoozyEvents.DoozyEventKey(Prefix, compId, compId);
        }
    }

    public class DoozyEvents {
        public const string Sep = "|";

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string DoozyEventKey(string prefix, Component comp) {
            return $"{prefix}{Sep}{comp.GetInstanceID().ToString()}";
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string DoozyEventKey(string prefix, Component comp, string key) {
            return $"{prefix}{Sep}{comp.GetInstanceID().ToString()}{Sep}{key}";
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string DoozyEventKey(string prefix, string compId, string key) {
            return $"{prefix}{Sep}{compId}{Sep}{key}";
        }
    }
}
