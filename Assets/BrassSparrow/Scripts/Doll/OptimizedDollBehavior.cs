using System.Runtime.Serialization.Formatters.Binary;
using Maru.MCore;
using UnityEngine;

namespace BrassSparrow.Scripts.Doll {
    // semi-optimized. removes unused parts at load
    public class OptimizedDollBehavior : MonoBehaviour {
        public GameObject modularDoll;
        public Shader shader;
        public DollConfig dollConfig;
        public string dollConfigPath;

        private void Start() {
            var locator = LocatorProvider.Get();
            var bf = locator.Get(MaruKeys.BinaryFormatter) as BinaryFormatter;
            if (!string.IsNullOrEmpty(dollConfigPath)) {
                dollConfig = DollConfig.FromFile(bf, dollConfigPath);
            }
            
        }
    }
}
