using System;
using System.Collections;
using System.Collections.Generic;
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

    public class DollPart {
        public readonly GameObject Go;
        public readonly string Path;
        public readonly DollPartType Type;

        public DollPart(GameObject go, string path, DollPartType type) {
            Go = go;
            Path = path;
            Type = type;
        }
    }

    public class DollHead : DollPart {
        public readonly bool AllowsElements;

        public DollHead(GameObject go, string path, bool allowsElements) 
            : base(go, path, DollPartType.Head) {
            AllowsElements = allowsElements;
        }
    }

    public class DollHeadCovering : DollPart {
        public readonly bool AllowsHair;
        public readonly bool AllowsFacialHair;

        public DollHeadCovering(GameObject go, string path, bool allowsHair, bool allowsFacialHair) 
            : base(go, path, DollPartType.HeadCovering) {
            AllowsHair = allowsHair;
            AllowsFacialHair = allowsFacialHair;
        }
    }
}
