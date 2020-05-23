using System;
using UnityEngine;

namespace Maru.Scripts.MSerialize {
    [Serializable]
    public struct SerializableColor {
        public float r;
        public float g;
        public float b;
        public float a;
        
        public SerializableColor(float r, float g, float b, float a) {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        public static SerializableColor FromColor(Color color) {
            return new SerializableColor(color.r, color.g, color.b, color.a);
        }

        public Color ToColor() {
            return new Color(r, g, b, a);
        }
    }
}
