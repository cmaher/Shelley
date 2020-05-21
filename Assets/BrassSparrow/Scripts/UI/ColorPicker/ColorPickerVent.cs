using Maru.MCore;
using UnityEngine;
using UnityEngine.UI.Extensions.ColorPicker;

namespace BrassSparrow.Scripts.UI.ColorPicker {
    public class ColorPickerVent : MonoBehaviour {
        public string key;
        private IMessageBus vent;

        private ColorPickerControl colorPicker;

        private void Awake() {
            colorPicker = GetComponent<ColorPickerControl>();
            vent = LocatorProvider.Get().Get(MaruKeys.Vent) as IMessageBus;
            colorPicker.onValueChanged.AddListener(ColorPickerChanged);
        }

        void ColorPickerChanged(Color color) {
            vent.Trigger(new ColorPickerChangedEvent {
                Key = key,
                Color = color,
            });
        }
    }

    public struct ColorPickerChangedEvent : IKeyedEvent {
        public string Key;
        public Color Color;

        public string GetEventKey() {
            return Key;
        }
    }
}
