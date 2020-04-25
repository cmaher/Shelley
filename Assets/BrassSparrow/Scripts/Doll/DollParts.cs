using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrassSparrow.Scripts.Doll {
    public class Doll {
        public DollParts Parts;

        public DollConfig ToConfig() {
            return new DollConfig {
                parts = Parts.ToConfig(),
            };
        }
    }

    public class DollParts : IEnumerable<DollPart> {
        // From GenderedChoices
        public DollHead Head;
        public DollPart Eyebrows;
        public DollPart FacialHair;
        public DollPart Torso;
        public DollPart ArmUpperRight;
        public DollPart ArmUpperLeft;
        public DollPart ArmLowerRight;
        public DollPart ArmLowerLeft;
        public DollPart HandRight;
        public DollPart HandLeft;
        public DollPart Hips;
        public DollPart LegRight;
        public DollPart LegLeft;

        public DollHeadCovering HeadCovering;
        public DollPart Hair;
        public DollPart HeadAttachment;
        public DollPart BackAttachment;
        public DollPart ShoulderAttachmentRight;
        public DollPart ShoulderAttachmentLeft;
        public DollPart ElbowAttachmentRight;
        public DollPart ElbowAttachmentLeft;
        public DollPart HipsAttachment;
        public DollPart KneeAttachmentRight;
        public DollPart KneeAttachmentLeft;
        public DollPart Extra;

        public IEnumerator<DollPart> GetEnumerator() {
            yield return Head;
            yield return Eyebrows;
            yield return FacialHair;
            yield return Torso;
            yield return ArmUpperRight;
            yield return ArmUpperLeft;
            yield return ArmLowerRight;
            yield return ArmLowerLeft;
            yield return HandRight;
            yield return HandLeft;
            yield return Hips;
            yield return LegRight;
            yield return LegLeft;

            yield return HeadCovering;
            yield return Hair;
            yield return HeadAttachment;
            yield return BackAttachment;
            yield return ShoulderAttachmentRight;
            yield return ShoulderAttachmentLeft;
            yield return ElbowAttachmentRight;
            yield return ElbowAttachmentLeft;
            yield return HipsAttachment;
            yield return KneeAttachmentRight;
            yield return KneeAttachmentLeft;
            yield return Extra;
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public DollParts ShallowClone() {
            return MemberwiseClone() as DollParts;
        }

        public DollPartsConfig ToConfig() {
            return new DollPartsConfig {
                head = Head?.Path,
                eyebrows = Eyebrows?.Path,
                facialHair = FacialHair?.Path,
                torso = Torso?.Path,
                armUpperRight = ArmUpperRight?.Path,
                armUpperLeft = ArmUpperLeft?.Path,
                armLowerRight = ArmLowerRight?.Path,
                armLowerLeft = ArmLowerLeft?.Path,
                handRight = HandRight?.Path,
                handLeft = HandLeft?.Path,
                hips = Hips?.Path,
                legRight = LegRight?.Path,
                legLeft = LegLeft?.Path,
                headCovering = HeadCovering?.Path,
                hair = Hair?.Path,
                headAttachment = HeadAttachment?.Path,
                backAttachment = BackAttachment?.Path,
                shoulderAttachmentRight = ShoulderAttachmentRight?.Path,
                shoulderAttachmentLeft = ShoulderAttachmentLeft?.Path,
                elbowAttachmentLeft = ElbowAttachmentRight?.Path,
                elbowAttachmentRight = ElbowAttachmentLeft?.Path,
                hipsAttachment = HipsAttachment?.Path,
                kneeAttachmentRight = KneeAttachmentRight?.Path,
                kneeAttachmentLeft = KneeAttachmentLeft?.Path,
                extra = Extra?.Path,
            };
        }
    }

    public class DollPart {
        public GameObject Go;
        public readonly string Path;

        public DollPart(GameObject go, string path) {
            Go = go;
            Path = path;
        }
    }

    public class DollHead : DollPart {
        public readonly bool AllowsElements;

        public DollHead(GameObject go, string path, bool allowsElements) : base(go, path) {
            AllowsElements = allowsElements;
        }
    }

    public class DollHeadCovering : DollPart {
        public readonly bool AllowsHair;
        public readonly bool AllowsFacialHair;

        public DollHeadCovering(GameObject go, string path, bool allowsHair, bool allowsFacialHair) : base(go, path) {
            AllowsHair = allowsHair;
            AllowsFacialHair = allowsFacialHair;
        }
    }
}
