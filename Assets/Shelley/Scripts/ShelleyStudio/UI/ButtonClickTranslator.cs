using Maru.MCore;
using UnityEngine.UI;

namespace Shelley.Scripts.ShelleyStudio.UI {
    // A class used to simply emit events 
    public class ButtonClickTranslator : VentBehavior {
        public string key;

        protected override void Awake() {
            base.Awake();
            var button = GetComponent<Button>();
            button.onClick.AddListener(() => {
                var evt = new UIComponentEvent {Component = this};
                Vent.Trigger(key, evt);
            });
        }
    }
}
