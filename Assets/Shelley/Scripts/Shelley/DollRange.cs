using System;
using UnityEngine;

namespace Shelley {
    public class DollRange {
        public readonly DollRangeType Type;
        public readonly string Label;
        public readonly string VarName;
        public readonly int Id;

        private DollRange(DollRangeType type, string varName) {
            Type = type;
            Label = Enum.GetName(typeof(DollColorType), type);
            VarName = varName;
            Id = Shader.PropertyToID(varName);
        }
        
        public static readonly DollRange Metallic = new DollRange(DollRangeType.Metallic, "_Metallic");
        public static readonly DollRange Smoothness = new DollRange(DollRangeType.Smoothness, "_Smoothness");
        public static readonly DollRange BodyArtAmount = new DollRange(DollRangeType.BodyArtAmount, "_BodyArt_Amount");
        public static readonly DollRange Emission = new DollRange(DollRangeType.Emission, "_Emmision"); // sic "Emmision"

        public static DollRange Get(DollRangeType type) {
            switch (type) {
                case DollRangeType.Metallic:
                    return Metallic;
                case DollRangeType.Smoothness:
                    return Smoothness;
                case DollRangeType.BodyArtAmount:
                    return BodyArtAmount;
                case DollRangeType.Emission:
                    return Emission;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
    
    public enum DollRangeType {
        Metallic,
        Smoothness,
        BodyArtAmount,
        // What does this do?
        Emission,
    }

    public static class DollRangeTypes {
        public static readonly Array Values = Enum.GetValues(typeof(DollRangeType));
        public static readonly int Length = Values.Length;
    }
}
