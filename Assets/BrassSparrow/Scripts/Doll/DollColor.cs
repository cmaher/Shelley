using System;
using UnityEngine;

namespace BrassSparrow.Scripts.Doll {
    public class DollColor {
        public readonly DollColorType Type;
        public readonly string Label;
        public readonly string VarName;
        public readonly int Id;

        private DollColor(DollColorType type, string varName) {
            Type = type;
            Label = Enum.GetName(typeof(DollColorType), type);
            VarName = varName;
            Id = Shader.PropertyToID(varName);
        }
        
        public static readonly DollColor Primary = new DollColor(DollColorType.Primary, "_Color_Primary");
        public static readonly DollColor Secondary = new DollColor(DollColorType.Secondary, "_Color_Secondary");
        public static readonly DollColor LeatherPrimary = new DollColor(DollColorType.LeatherPrimary, "_Color_LeatherPrimaryy");
        public static readonly DollColor LeatherSecondary = new DollColor(DollColorType.LeatherSecondary, "_Color_LeatherSecondary");
        public static readonly DollColor MetalPrimary = new DollColor(DollColorType.MetalPrimary, "_Color_MetalPrimary");
        public static readonly DollColor MetalDark = new DollColor(DollColorType.MetalDark, "_Color_MetalDark");
        public static readonly DollColor MetalSecondary = new DollColor(DollColorType.MetalSecondary, "_Color_MetalSecondary");
        public static readonly DollColor Hair = new DollColor(DollColorType.Hair, "_Color_Hair");
        public static readonly DollColor Skin = new DollColor(DollColorType.Skin, "_Color_Skin");
        public static readonly DollColor Stubble = new DollColor(DollColorType.Stubble, "_Color_Stubble");
        public static readonly DollColor Scar = new DollColor(DollColorType.Scar, "_Color_Scar");
        public static readonly DollColor BodyArt = new DollColor(DollColorType.BodyArt, "_Color_BodyArt");
        public static readonly DollColor Eyes = new DollColor(DollColorType.Eyes, "_Color_Eyes");

        public static DollColor Get(DollColorType type) {
            switch (type) {
                case DollColorType.Primary:
                    return Primary;
                case DollColorType.Secondary:
                    return Secondary;
                case DollColorType.LeatherPrimary:
                    return LeatherPrimary;
                case DollColorType.LeatherSecondary:
                    return LeatherSecondary;
                case DollColorType.MetalPrimary:
                    return MetalPrimary;
                case DollColorType.MetalDark:
                    return MetalDark;
                case DollColorType.MetalSecondary:
                    return MetalSecondary;
                case DollColorType.Hair:
                    return Hair;
                case DollColorType.Skin:
                    return Skin;
                case DollColorType.Stubble:
                    return Stubble;
                case DollColorType.Scar:
                    return Scar;
                case DollColorType.BodyArt:
                    return BodyArt;
                case DollColorType.Eyes:
                    return Eyes;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }

    public enum DollColorType {
        Primary,
        Secondary,
        LeatherPrimary,
        LeatherSecondary,
        MetalPrimary,
        MetalDark,
        MetalSecondary,
        Hair,
        Skin,
        Stubble,
        Scar,
        BodyArt,
        Eyes
    }

    public enum DollShaderRanges {
        Metallic,
        Smoothness,
        BodyArtAmount,

        // What does this do?
        Emission
    }
}
