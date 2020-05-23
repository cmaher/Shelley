using System;
using System.ComponentModel;
using UnityEngine;

namespace BrassSparrow.Scripts.Doll {
    public enum DollPartType {
        Head,
        Eyebrows,
        FacialHair,
        Torso,
        ArmUpperRight,
        ArmUpperLeft,
        ArmLowerRight,
        ArmLowerLeft,
        HandRight,
        HandLeft,
        Hips,
        LegRight,
        LegLeft,

        HeadCovering,
        Hair,
        HeadAttachment,
        BackAttachment,
        ShoulderAttachmentRight,
        ShoulderAttachmentLeft,
        ElbowAttachmentRight,
        ElbowAttachmentLeft,
        HipsAttachment,
        KneeAttachmentRight,
        KneeAttachmentLeft,
        Extra,
    }

    public static class DollPartTypes {
        public static bool IsGendered(DollPartType type) {
            switch (type) {
                case DollPartType.Head:
                case DollPartType.Eyebrows:
                case DollPartType.FacialHair:
                case DollPartType.Torso:
                case DollPartType.ArmUpperRight:
                case DollPartType.ArmUpperLeft:
                case DollPartType.ArmLowerRight:
                case DollPartType.ArmLowerLeft:
                case DollPartType.HandRight:
                case DollPartType.HandLeft:
                case DollPartType.Hips:
                case DollPartType.LegRight:
                case DollPartType.LegLeft:
                    return true;

                case DollPartType.HeadCovering:
                case DollPartType.Hair:
                case DollPartType.HeadAttachment:
                case DollPartType.BackAttachment:
                case DollPartType.ShoulderAttachmentRight:
                case DollPartType.ShoulderAttachmentLeft:
                case DollPartType.ElbowAttachmentRight:
                case DollPartType.ElbowAttachmentLeft:
                case DollPartType.HipsAttachment:
                case DollPartType.KneeAttachmentRight:
                case DollPartType.KneeAttachmentLeft:
                case DollPartType.Extra:
                    return false;
                default:
                    throw new InvalidEnumArgumentException();
            }
        }

        public static readonly Array Values = Enum.GetValues(typeof(DollPartType));
        public static readonly int Length = Values.Length;
    }

    public class DollPart {
        public readonly GameObject Go;
        public readonly string Path;
        public readonly DollPartType Type;

        // C# covariance is borderline useless and fails with List<T: Base>
        // So let's put all the relevant infnormation here instead of making a child class
        public readonly bool AllowsElements;
        public readonly bool AllowsHair;
        public readonly bool AllowsFacialHair;

        public DollPart(GameObject go, string path, DollPartType type) {
            Go = go;
            Path = path;
            Type = type;
            AllowsElements = true;
            AllowsHair = true;
            AllowsFacialHair = true;
        }

        public DollPart(
            GameObject go, string path, DollPartType type,
            bool allowsElements
        ) {
            Go = go;
            Path = path;
            Type = type;
            AllowsElements = allowsElements;
            AllowsHair = allowsElements;
            AllowsFacialHair = allowsElements;
        }

        public DollPart(
            GameObject go, string path, DollPartType type,
            bool allowsHair, bool allowsFacialHair
        ) {
            Go = go;
            Path = path;
            Type = type;
            AllowsElements = true;
            AllowsHair = allowsHair;
            AllowsFacialHair = allowsFacialHair;
        }
    }
}
