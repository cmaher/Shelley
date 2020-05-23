using System;
using System.Collections;
using System.Collections.Generic;
using Maru.Scripts.MSerialize;
using UnityEngine;
using UnityEngine.Serialization;

namespace BrassSparrow.Scripts.Doll {
    [Serializable]
    public class DollConfig {
        public Dictionary<DollPartType, string> activeParts;
        public Dictionary<DollPartType, DollShaderConfig> shaders;
    }
    
    [Serializable]
    public class DollShaderConfig {
        public Dictionary<DollColorType, SerializableColor> colors;
        public Dictionary<DollRangeType, float> ranges;
    }
}
