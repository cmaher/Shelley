using Maru.MCore;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Shelley.Scripts.ShelleyStudio.UI.ColorPicker {
    public class ColorPickerPresetRemove : VentBehavior, IPointerClickHandler {
        public int id;
        public string key;
        public Color color;

        public void OnPointerClick(PointerEventData eventData) {
            if (eventData.button == PointerEventData.InputButton.Left) {
                Vent.Trigger(new SelectColorPickerPresetEvent {
                    Key = key,
                    Id = id,
                    Color = color,
                });
            } else if (eventData.button == PointerEventData.InputButton.Right) {
                Vent.Trigger(new RemoveColorPickerPresetEvent {
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
