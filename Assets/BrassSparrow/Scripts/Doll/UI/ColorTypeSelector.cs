using System;
using BrassSparrow.Scripts.Doll;
using Doozy.Engine.Extensions;
using Maru.MCore;
using UnityEngine;
using UnityEngine.UI;

namespace BrassSparrow.Scripts.UI {
    public class ColorTypeSelector : VentBehavior {
        public Image colorSwatch;
        public String listenKey;

        private DollColorType colorType;

        protected override int EventCapacity => 1;

        private void Start() {
            On<DollColorChangedEvent>(listenKey, OnDollColorChanged);
            var selector = GetComponent<EnumSelectorButton>();
            colorType = (DollColorType) (object) selector.selection.EnumValue();
        }

        private void OnDollColorChanged(DollColorChangedEvent evt) {
            if (colorType == evt.ColorType) {
                SetColor(evt.Color);
            }
        }

        public void SetColor(Color color) {
            colorSwatch.color = color.WithAlpha(1);
        }
    }
}
