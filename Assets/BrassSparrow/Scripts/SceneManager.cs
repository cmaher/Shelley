using Maru.MCore;
using UnityEngine;

namespace BrassSparrow.Scripts {
    public class SceneManager : MonoBehaviour {
        public static readonly string RandomKey = "BrassSparrow.Random";
        public static readonly string VentKey = "BrassSparrow.Vent";
        private void Awake() {
            var locator = new DictLocator();
            LocatorProvider.Init(locator);

            locator.Set(RandomKey, new System.Random());
            locator.Set(VentKey, new MessageBus());
        }
    }
}
