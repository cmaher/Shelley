using System;
using Maru.MCore;
using ShelleyStudio.Events;
using TMPro;
using UnityEngine.UI;

namespace ShelleyStudio.UI {
    public class EnumComboSlider : VentBehavior {
        public Slider slider;
        public TMP_Text label;
        public TMP_InputField textInput;
        public IEnumDataTrigger<float> ValueTrigger;

        private Boolean listen = true;

        private void Start() {
            slider.onValueChanged.AddListener(OnSliderChange);
            textInput.onValueChanged.AddListener(OnTextInputChange);
        }

        private void OnSliderChange(float value) {
            if (listen) {
                listen = false;
                textInput.text = value.ToString();
                listen = true;
                ValueTrigger.TriggerEvent(Vent, value);
            }
        }

        private void OnTextInputChange(string text) {
            if (listen) {
                listen = false;

                float value;
                var parsed = float.TryParse(text, out value);
                if (!parsed) {
                    value = 0f;
                } else {
                    if (value < 0f) {
                        value = 0f;
                        textInput.text = "0.0";
                    } else if (value > 1f) {
                        value = 1f;
                        textInput.text = "1.0";
                    }
                }
                slider.value = value;
                
                listen = true;
                ValueTrigger.TriggerEvent(Vent, value);
            }
        }

        public void SetValue(float value) {
            listen = false;
            slider.value = value;
            textInput.text = value.ToString();
            listen = true;
        }
    }
}
