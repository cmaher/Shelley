using System;
using Maru.MCore;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BrassSparrow.Scripts.UI {
    public class ColorPickerPresetRemove : MonoBehaviour, IPointerClickHandler {
        public int id;
        public string key;
        public Color color;

        private IMessageBus vent;

        private void Awake() {
            vent = LocatorProvider.Get().Get(SceneManager.VentKey) as IMessageBus;
        }

        public void OnPointerClick(PointerEventData eventData) {
            if (eventData.button == PointerEventData.InputButton.Left) {
                vent.Trigger(new SelectColorPickerPresetEvent {
                    Key = key,
                    Id = id,
                    Color = color,
                });
            } else if (eventData.button == PointerEventData.InputButton.Right) {
                vent.Trigger(new RemoveColorPickerPresetEvent {
                    Key = key,
                    Id = id,
                });
            }
        }
    }

    public struct SelectColorPickerPresetEvent : IKeyedEvent {
        public string Key;
        public int Id;
        public Color Color;

        public string GetEventKey() {
            return Key;
        }
    }

    public struct RemoveColorPickerPresetEvent : IKeyedEvent {
        public string Key;
        public int Id;

        public string GetEventKey() {
            return Key;
        }
    }
}
