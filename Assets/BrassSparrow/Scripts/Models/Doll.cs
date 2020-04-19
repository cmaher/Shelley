using System;
using System.Collections.Generic;
using PsychoticLab;
using UnityEngine;

namespace BrassSparrow.Scripts.Models {
    public class DollChoices {
        public UngenderedDollChoices Ungendered;
        public GenderedDollChoices Male;
        public GenderedDollChoices Female;
    }

    public class UngenderedDollChoices {
        public List<DollHeadCovering> HeadCovering;
        public List<GameObject> Hair;
        public List<GameObject> HeadAttachment;
        public List<GameObject> BackAttachment;
        public List<GameObject> ShoulderAttachmentRight;
        public List<GameObject> ShoulderAttachmentLeft;
        public List<GameObject> ElbowAttachmentRight;
        public List<GameObject> ElbowAttachmentLeft;
        public List<GameObject> HipsAttachment;
        public List<GameObject> KneeAttachmentRight;
        public List<GameObject> KneeAttachmentLeft;
        public List<GameObject> Extra;
    }

    public class GenderedDollChoices {
        public List<DollHead> Head;
        public List<GameObject> Eyebrows;
        public List<GameObject> FacialHair;
        public List<GameObject> Torso;
        public List<GameObject> ArmUpperRight;
        public List<GameObject> ArmUpperLeft;
        public List<GameObject> ArmLowerRight;
        public List<GameObject> ArmLowerLeft;
        public List<GameObject> HandRight;
        public List<GameObject> HandLeft;
        public List<GameObject> Hips;
        public List<GameObject> LegRight;
        public List<GameObject> LegLeft;
    }

    public class DollHead {
        public GameObject GameObject;
        public bool AllowsElements;

        public DollHead(GameObject gameObject, bool allowsElements) {
            GameObject = gameObject;
            AllowsElements = allowsElements;
        }
    }

    public class DollHeadCovering {
        public GameObject GameObject;
        public bool AllowsHair;
        public bool AllowsFacialHair;

        public DollHeadCovering(GameObject gameObject, bool allowsHair, bool allowsFacialHair) {
            GameObject = gameObject;
            AllowsHair = allowsHair;
            AllowsFacialHair = allowsFacialHair;
        }
    }
}
