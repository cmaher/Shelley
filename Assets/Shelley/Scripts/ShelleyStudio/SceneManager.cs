using System.Runtime.Serialization.Formatters.Binary;
using Doozy.Engine;
using Maru.MCore;
using Shelley.Scripts.ShelleyStudio.UI;
using UnityEngine;
using Random = System.Random;

// TODO Instantiate the doll design manager (or a DollManager) with these  locator values. Need to make this reusable.
// doozy vent part can be copied from scene to scene
// can also make a sub locator just for the doll design manager and call Locator.Get("Key")
namespace Shelley.Scripts.ShelleyStudio {
    public class SceneManager : MonoBehaviour {
        private DoozyEventTranslator doozyTranslator; 
            
        private void Awake() {
            
            var locator = new DictLocator();
            LocatorProvider.Init(locator);

            var vent = new MessageBus();
            locator.Set(MaruKeys.Random, new Random());
            locator.Set(MaruKeys.Vent, vent);
            locator.Set(MaruKeys.BinaryFormatter, new BinaryFormatter());
            
            var doozyListener = GetComponent<GameEventListener>();
            doozyTranslator = new DoozyEventTranslator(vent, doozyListener);
            doozyTranslator.ListenAndTranslate();
        }

        private void OnDestroy() {
            doozyTranslator.Shutdown();
        }
    }
}
