using UnityEngine;
using UnityEngine.UI;

namespace Shelley.Scripts.ShelleyStudio.UI {
    public class ColorSwatchUpdater : MonoBehaviour {
        public Image colorSwatch;

        public void SetColor(Color color) {
            colorSwatch.color = new Color(color.r, color.g, color.b, 1);
        }
    }
}
