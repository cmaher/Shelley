using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace BrassSparrow.Scripts.Doll {
    [Serializable]
    public class DollConfig {
        public DollPartsConfig parts;

        public DollConfig Clone() {
            return new DollConfig {
                parts = parts.Clone()
            };
        }
    }

    [Serializable]
    public class DollPartsConfig : IEnumerable<string> {
        public string head;
        public string eyebrows;
        public string facialHair;
        public string torso;
        public string armUpperRight;
        public string armUpperLeft;
        public string armLowerRight;
        public string armLowerLeft;
        public string handRight;
        public string handLeft;
        public string hips;
        public string legRight;
        public string legLeft;

        public string headCovering;
        public string hair;
        public string headAttachment;
        public string backAttachment;
        public string shoulderAttachmentRight;
        public string shoulderAttachmentLeft;
        public string elbowAttachmentRight;
        public string elbowAttachmentLeft;
        public string hipsAttachment;
        public string kneeAttachmentRight;
        public string kneeAttachmentLeft;
        public string extra;

        public void Set(DollPartType type, string path) {
            switch (type) {
                case DollPartType.Head:
                    head = path;
                    break;
                case DollPartType.Eyebrows:
                    eyebrows = path;
                    break;
                case DollPartType.FacialHair:
                    facialHair = path;
                    break;
                case DollPartType.Torso:
                    torso = path;
                    break;
                case DollPartType.ArmUpperRight:
                    armUpperRight = path;
                    break;
                case DollPartType.ArmUpperLeft:
                    armUpperLeft = path;
                    break;
                case DollPartType.ArmLowerRight:
                    armLowerRight = path;
                    break;
                case DollPartType.ArmLowerLeft:
                    armLowerLeft = path;
                    break;
                case DollPartType.HandRight:
                    handRight = path;
                    break;
                case DollPartType.HandLeft:
                    handLeft = path;
                    break;
                case DollPartType.Hips:
                    hips = path;
                    break;
                case DollPartType.LegRight:
                    legRight =path;
                    break;
                case DollPartType.LegLeft:
                    legLeft = path;
                    break;
                case DollPartType.HeadCovering:
                    headCovering = path;
                    break;
                case DollPartType.Hair:
                    hair = path;
                    break;
                case DollPartType.HeadAttachment:
                    headAttachment = path;
                    break;
                case DollPartType.BackAttachment:
                    backAttachment = path;
                    break;
                case DollPartType.ShoulderAttachmentRight:
                    shoulderAttachmentRight = path;
                    break;
                case DollPartType.ShoulderAttachmentLeft:
                    shoulderAttachmentLeft = path;
                    break;
                case DollPartType.ElbowAttachmentRight:
                    elbowAttachmentRight = path;
                    break;
                case DollPartType.ElbowAttachmentLeft:
                    elbowAttachmentLeft = path;
                    break;
                case DollPartType.HipsAttachment:
                    hipsAttachment = path;
                    break;
                case DollPartType.KneeAttachmentRight:
                    kneeAttachmentRight = path;
                    break;
                case DollPartType.KneeAttachmentLeft:
                    kneeAttachmentLeft = path;
                    break;
                case DollPartType.Extra:
                    extra = path;
                    break;
            }
        }
        
        public IEnumerator<string> GetEnumerator() {
            yield return head;
            yield return eyebrows;
            yield return facialHair;
            yield return torso;
            yield return armUpperRight;
            yield return armUpperLeft;
            yield return armLowerRight;
            yield return armLowerLeft;
            yield return handRight;
            yield return handLeft;
            yield return hips;
            yield return legRight;
            yield return legLeft;

            yield return headCovering;
            yield return hair;
            yield return headAttachment;
            yield return backAttachment;
            yield return shoulderAttachmentRight;
            yield return shoulderAttachmentLeft;
            yield return elbowAttachmentRight;
            yield return elbowAttachmentLeft;
            yield return hipsAttachment;
            yield return kneeAttachmentRight;
            yield return kneeAttachmentLeft;
            yield return extra;
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public DollPartsConfig Clone() {
            return MemberwiseClone() as DollPartsConfig;
        }
    }
}
