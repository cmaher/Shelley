using Doozy.Engine.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Shelley.Scripts.ShelleyStudio.UI {
    public class ColorSwatchUpdater : MonoBehaviour {
        public Image colorSwatch;

        public void SetColor(Color color) {
            colorSwatch.color = color.WithAlpha(1);
        }
    }
}
