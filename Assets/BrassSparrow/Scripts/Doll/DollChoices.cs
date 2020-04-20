using System.Collections.Generic;
using UnityEngine;

namespace BrassSparrow.Scripts.Doll {
    public class DollChoices {
        public UngenderedDollChoices Ungendered;
        public GenderedDollChoices Male;
        public GenderedDollChoices Female;
    }

    public class UngenderedDollChoices {
        public List<DollHeadCovering> HeadCovering;
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
    }

    public class GenderedDollChoices {
        public List<DollHead> Head;
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
    }
}
