using System;
using BrassSparrow.Scripts.UI;
using Doozy.Engine;
using Maru.MCore;
using UnityEngine;

namespace BrassSparrow.Scripts {
    public class SceneManager : MonoBehaviour {
        public const string RandomKey = "BrassSparrow.Random";

        private DoozyEventTranslator doozyTranslator; 
            
        private void Awake() {
            
            var locator = new DictLocator();
            LocatorProvider.Init(locator);

            var vent = new MessageBus();
            locator.Set(RandomKey, new System.Random());
            locator.Set(MaruKeys.Vent, vent);
            
            var doozyListener = GetComponent<GameEventListener>();
            doozyTranslator = new DoozyEventTranslator(vent, doozyListener);
            doozyTranslator.ListenAndTranslate();
        }

        private void OnDestroy() {
            doozyTranslator.Shutdown();
        }
    }
}
