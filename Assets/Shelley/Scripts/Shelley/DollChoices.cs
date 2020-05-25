using System.Collections.Generic;
using System.ComponentModel;

namespace Shelley.Scripts.Shelley {
    public class DollChoices {
        public UngenderedDollChoices Ungendered;
        public GenderedDollChoices Male;
        public GenderedDollChoices Female;
    }

    public class UngenderedDollChoices {
        public List<DollPart> HeadCovering;
        public List<DollPart> Hair;
        public List<DollPart> HeadAttachment;
        public List<DollPart> BackAttachment;
        public List<DollPart> ShoulderAttachmentRight;
        public List<DollPart> ShoulderAttachmentLeft;
        public List<DollPart> ElbowAttachmentRight;
        public List<DollPart> ElbowAttachmentLeft;
        public List<DollPart> HipsAttachment;
        public List<DollPart> KneeAttachmentRight;
        public List<DollPart> KneeAttachmentLeft;
        public List<DollPart> Extra;

        public List<DollPart> Get(DollPartType type) {
            switch (type) {
                case DollPartType.HeadCovering:
                    return HeadCovering;
                case DollPartType.Hair:
                    return Hair;
                case DollPartType.HeadAttachment:
                    return HeadAttachment;
                case DollPartType.BackAttachment:
                    return BackAttachment;
                case DollPartType.ShoulderAttachmentRight:
                    return ShoulderAttachmentRight;
                case DollPartType.ShoulderAttachmentLeft:
                    return ShoulderAttachmentLeft;
                case DollPartType.ElbowAttachmentRight:
                    return ElbowAttachmentRight;
                case DollPartType.ElbowAttachmentLeft:
                    return ElbowAttachmentLeft;
                case DollPartType.HipsAttachment:
                    return HipsAttachment;
                case DollPartType.KneeAttachmentRight:
                    return KneeAttachmentRight;
                case DollPartType.KneeAttachmentLeft:
                    return KneeAttachmentLeft;
                case DollPartType.Extra:
                    return Extra;
                default:
                    throw new InvalidEnumArgumentException();
            }
        }
    }

    public class GenderedDollChoices {
        public List<DollPart> Head;
        public List<DollPart> Eyebrows;
        public List<DollPart> FacialHair;
        public List<DollPart> Torso;
        public List<DollPart> ArmUpperRight;
        public List<DollPart> ArmUpperLeft;
        public List<DollPart> ArmLowerRight;
        public List<DollPart> ArmLowerLeft;
        public List<DollPart> HandRight;
        public List<DollPart> HandLeft;
        public List<DollPart> Hips;
        public List<DollPart> LegRight;
        public List<DollPart> LegLeft;

        public List<DollPart> Get(DollPartType type) {
            switch (type) {
                case DollPartType.Head:
                    return Head;
                case DollPartType.Eyebrows:
                    return Eyebrows;
                case DollPartType.FacialHair:
                    return FacialHair;
                case DollPartType.Torso:
                    return Torso;
                case DollPartType.ArmUpperRight:
                    return ArmUpperRight;
                case DollPartType.ArmUpperLeft:
                    return ArmUpperLeft;
                case DollPartType.ArmLowerRight:
                    return ArmLowerRight;
                case DollPartType.ArmLowerLeft:
                    return ArmLowerLeft;
                case DollPartType.HandRight:
                    return HandRight;
                case DollPartType.HandLeft:
                    return HandLeft;
                case DollPartType.Hips:
                    return Hips;
                case DollPartType.LegRight:
                    return LegRight;
                case DollPartType.LegLeft:
                    return LegLeft;
                default:
                    throw new InvalidEnumArgumentException();
            }
        }
    }
}
