using System;
using UnityEngine.Serialization;

namespace BrassSparrow.Scripts.Doll {
    [Serializable]
    public class DollConfig {
        public DollPartsConfig parts;
    }

    [Serializable]
    public class DollPartsConfig {
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
    }
}
