using Doozy.Engine.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace BrassSparrow.Scripts.UI {
    public class ColorSwatchUpdater : MonoBehaviour {
        public Image colorSwatch;

        public void SetColor(Color color) {
            colorSwatch.color = color.WithAlpha(1);
        }
    }
}
