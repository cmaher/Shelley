using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Maru.Scripts.MSerialize;
using UnityEngine;
using UnityEngine.Serialization;

namespace BrassSparrow.Scripts.Doll {
    [Serializable]
    public class DollConfig {
        public Dictionary<DollPartType, string> activeParts;
        public Dictionary<DollPartType, DollShaderConfig> shaders;

        public static DollConfig FromFile(BinaryFormatter formatter, string path) {
            var file = File.Open(path, FileMode.Open);
            return formatter.Deserialize(file) as DollConfig;
        }
    }
    
    [Serializable]
    public class DollShaderConfig {
        public Dictionary<DollColorType, SerializableColor> colors;
        public Dictionary<DollRangeType, float> ranges;
    }
}
