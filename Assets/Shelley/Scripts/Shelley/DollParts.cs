using System;
using System.ComponentModel;
using UnityEngine;

namespace Shelley {
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

        public static bool IsMirrorable(DollPartType type) {
            switch (type) {
                case DollPartType.ArmUpperRight:
                case DollPartType.ArmUpperLeft:
                case DollPartType.ArmLowerRight:
                case DollPartType.ArmLowerLeft:
                case DollPartType.HandRight:
                case DollPartType.HandLeft:
                case DollPartType.LegRight:
                case DollPartType.LegLeft:
                case DollPartType.ShoulderAttachmentRight:
                case DollPartType.ShoulderAttachmentLeft:
                case DollPartType.ElbowAttachmentRight:
                case DollPartType.ElbowAttachmentLeft:
                case DollPartType.KneeAttachmentRight:
                case DollPartType.KneeAttachmentLeft:
                    return true;

                case DollPartType.Head:
                case DollPartType.Eyebrows:
                case DollPartType.FacialHair:
                case DollPartType.Torso:
                case DollPartType.Hips:
                case DollPartType.HeadCovering:
                case DollPartType.Hair:
                case DollPartType.HeadAttachment:
                case DollPartType.BackAttachment:
                case DollPartType.HipsAttachment:
                case DollPartType.Extra:
                    return false;
                
                default:
                    throw new InvalidEnumArgumentException();
            }
        }

        public static DollPartType Mirror(DollPartType type) {
            switch (type) {
                case DollPartType.ArmUpperRight:
                    return DollPartType.ArmUpperLeft;
                case DollPartType.ArmUpperLeft:
                    return DollPartType.ArmUpperRight;
                case DollPartType.ArmLowerRight:
                    return DollPartType.ArmLowerLeft;
                case DollPartType.ArmLowerLeft:
                    return DollPartType.ArmLowerRight;
                case DollPartType.HandRight:
                    return DollPartType.HandLeft;
                case DollPartType.HandLeft:
                    return DollPartType.HandRight;
                case DollPartType.LegRight:
                    return DollPartType.LegLeft;
                case DollPartType.LegLeft:
                    return DollPartType.LegRight;
                case DollPartType.ShoulderAttachmentRight:
                    return DollPartType.ShoulderAttachmentLeft;
                case DollPartType.ShoulderAttachmentLeft:
                    return DollPartType.ShoulderAttachmentRight;
                case DollPartType.ElbowAttachmentRight:
                    return DollPartType.ElbowAttachmentRight;
                case DollPartType.ElbowAttachmentLeft:
                    return DollPartType.ElbowAttachmentLeft;
                case DollPartType.KneeAttachmentRight:
                    return DollPartType.KneeAttachmentLeft;
                case DollPartType.KneeAttachmentLeft:
                    return DollPartType.KneeAttachmentRight;

                case DollPartType.Head:
                case DollPartType.Eyebrows:
                case DollPartType.FacialHair:
                case DollPartType.Torso:
                case DollPartType.Hips:
                case DollPartType.HeadCovering:
                case DollPartType.Hair:
                case DollPartType.HeadAttachment:
                case DollPartType.BackAttachment:
                case DollPartType.HipsAttachment:
                case DollPartType.Extra:
                    return type;
                
                default:
                    throw new InvalidEnumArgumentException();
            }
        }
        
        private static string hipBone = "Hips";
        private static string hipAttachmentBone = $"{hipBone}/Hips_Attachment";
        private static string fullSpine = $"{hipBone}/Spine_01/Spine_02/Spine03";
        private static string neckBone = $"{fullSpine}/Neck";
        private static string eyebrowsBone = $"{neckBone}/Head/Eyebrows";
        private static string armUpperRightBone = $"{fullSpine}/Clavicle_R";
        private static string armUpperLeftBone = $"{fullSpine}/Clavicle_L";
        private static string armLowerRightBone = $"{armUpperRightBone}/Shoulder_R";
        private static string armLowerLeftBone = $"{armUpperLeftBone}/Shoulder_L";
        private static string handRightBone = $"{armLowerRightBone}/Elbow_R/Hand_R";
        private static string handLeftBone = $"{armLowerLeftBone}/Elbow_L/Hand_L";
        private static string legRightBone = $"{hipBone}/UpperLeg_R/LowerLeg_R";
        private static string legLeftBone = $"{hipBone}/UpperLeg_L/LowerLeg_L";

        public static string RootBonePath(DollPartType partType) {
            switch (partType) {
                case DollPartType.Head:
                    return neckBone;
                case DollPartType.Eyebrows:
                    return eyebrowsBone;
                case DollPartType.FacialHair:
                    break;
                case DollPartType.Torso:
                    return hipBone;
                case DollPartType.ArmUpperRight:
                    return armUpperRightBone;
                case DollPartType.ArmUpperLeft:
                    return armUpperLeftBone;
                case DollPartType.ArmLowerRight:
                    return armLowerRightBone;
                case DollPartType.ArmLowerLeft:
                    return armLowerLeftBone;
                case DollPartType.HandRight:
                    return handRightBone;
                case DollPartType.HandLeft:
                    return handLeftBone;
                case DollPartType.Hips:
                    return hipBone;
                case DollPartType.LegRight:
                    return legRightBone;
                case DollPartType.LegLeft:
                    return legLeftBone;
                case DollPartType.HeadCovering:
                    break;
                case DollPartType.Hair:
                    break;
                case DollPartType.HeadAttachment:
                    break;
                case DollPartType.BackAttachment:
                    break;
                case DollPartType.ShoulderAttachmentRight:
                    break;
                case DollPartType.ShoulderAttachmentLeft:
                    break;
                case DollPartType.ElbowAttachmentRight:
                    break;
                case DollPartType.ElbowAttachmentLeft:
                    break;
                case DollPartType.HipsAttachment:
                    break;
                case DollPartType.KneeAttachmentRight:
                    break;
                case DollPartType.KneeAttachmentLeft:
                    break;
                case DollPartType.Extra:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(partType), partType, null);
            }

            return "";
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

        public string MirrorPartPath() {
            if (!DollPartTypes.IsMirrorable(Type)) {
                return Path;
            }
            
            switch (Type) {
                case DollPartType.ArmUpperRight:
                    return Path.Replace("04_Arm_Upper_Right", "05_Arm_Upper_Left")
                        .Replace("ArmUpperRight", "ArmUpperLeft");
                case DollPartType.ArmUpperLeft:
                    return Path.Replace("05_Arm_Upper_Left", "04_Arm_Upper_Right")
                        .Replace("ArmUpperLeft", "ArmUpperRight");
                case DollPartType.ArmLowerRight:
                    return Path.Replace("06_Arm_Lower_Right", "07_Arm_Lower_Left")
                        .Replace("ArmLowerRight", "ArmLowerLeft");
                case DollPartType.ArmLowerLeft:
                    return Path.Replace("07_Arm_Lower_Left", "06_Arm_Lower_Right")
                        .Replace("ArmLowerLeft", "ArmLowerRight");
                case DollPartType.HandRight:
                    return Path.Replace("08_Hand_Right", "09_Hand_Left")
                        .Replace("HandRight", "HandLeft");
                case DollPartType.HandLeft:
                    return Path.Replace("09_Hand_Left", "08_Hand_Right")
                        .Replace("HandLeft", "HandRight");
                case DollPartType.LegRight:
                    return Path.Replace("11_Leg_Right", "12_Leg_Left")
                        .Replace("LegRight", "LegLeft");
                case DollPartType.LegLeft:
                    return Path.Replace("12_Leg_Left", "11_Leg_Right")
                        .Replace("LegLeft", "LegRight");
                case DollPartType.ShoulderAttachmentRight:
                    return Path.Replace("05_Shoulder_Attachment_Right", "06_Shoulder_Attachment_Left")
                        .Replace("ShoulderAttachRight", "ShoulderAttachLeft");
                case DollPartType.ShoulderAttachmentLeft:
                    return Path.Replace("06_Shoulder_Attachment_Left", "05_Shoulder_Attachment_Right")
                        .Replace("ShoulderAttachLeft", "ShoulderAttachRight");
                case DollPartType.ElbowAttachmentRight:
                    return Path.Replace("07_Elbow_Attachment_Right", "08_Elbow_Attachment_Left")
                        .Replace("ElbowAttachRight", "ElbowAttachLeft");
                case DollPartType.ElbowAttachmentLeft:
                    return Path.Replace("08_Elbow_Attachment_Left", "07_Elbow_Attachment_Right")
                        .Replace("ElbowAttachLeft", "ElbowAttachRight");
                case DollPartType.KneeAttachmentRight:
                    return Path.Replace("10_Knee_Attachement_Right", "11_Knee_Attachement_Left") // sic "Attachement"
                        .Replace("KneeAttachRight", "KneeAttachLeft");
                case DollPartType.KneeAttachmentLeft:
                    return Path.Replace("11_Knee_Attachement_Left", "10_Knee_Attachement_Right") // sic "Attachement"
                        .Replace("KneeAttachLeft", "KneeAttachRight");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
