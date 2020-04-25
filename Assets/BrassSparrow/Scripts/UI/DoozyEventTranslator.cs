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
        private Action offRegister;
        private Action offDeregister;

        public DoozyEventTranslator(IMessageBus vent, GameEventListener doozyListener) {
            componentDict = new Dictionary<string, Component>();
            this.vent = vent;
            this.doozyListener = doozyListener;
        }

        public void ListenAndTranslate() {
            offRegister = vent.On<RegisterUiComponentEvent>(evt => {
                var id = evt.Component.GetInstanceID().ToString();
                componentDict[id] = evt.Component;
            });
            offDeregister = vent.On<UnregisterUiComponentEvent>(evt => {
                var id = evt.Component.GetInstanceID().ToString();
                componentDict.Remove(id);
            });

            doozyListener.Event.AddListener(TranslateDoozyEvent);
        }

        public void TranslateDoozyEvent(string evt) {
            var sepPos = evt.IndexOf(Sep);
            var prefix = evt.Substring(0, sepPos);
            var id = evt.Substring(sepPos + 1);
            if (!componentDict.ContainsKey(id)) {
                Debug.Log($"No GameObject registered for Doozy event {evt}");
                return;
            }

            var comp = componentDict[id];

            switch (prefix) {
                case PartSelectorClickEvent.Prefix:
                    vent.Trigger(new PartSelectorClickEvent {PartSelector = comp as PartSelector});
                    break;
                default:
                    Debug.Log($"No event handler found for Doozy event {evt}");
                    break;
            }
        }

        public void Shutdown() {
            // remove these first in case another thread registers a Go before we clean the dict
            offRegister();
            offDeregister();
            componentDict = null;
            doozyListener.Event.RemoveListener(TranslateDoozyEvent);
        }
    }
    
    public class RegisterUiComponentEvent {
        public Component Component;
    }

    public class UnregisterUiComponentEvent {
        public Component Component;
    }

    public class DoozyEvents {
        public const string Sep = ":";

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string DoozyEventKey(string prefix, Component comp) {
            return $"{prefix}:{comp.GetInstanceID().ToString()}";
        }

        public struct PartSelectorClickEvent {
            public const string Prefix = "PartSelector-Click";
            public PartSelector PartSelector;
        }
    }
}
