using Doozy.Engine.UI;

namespace Shelley.Scripts.ShelleyStudio.UI {
    // A class used to simply emit events 
    public class ButtonClickTranslator : DoozyBehavior {
        public string key;

        protected override int EventCapacity => 1;

        protected override void Awake() {
            base.Awake();
            var doozyButton = GetComponent<UIButton>();
            doozyButton.OnClick.OnTrigger.GameEvents.Add(DoozySelfEvent.DoozyEventKey(this));
            OnSelfEvent(() => {
                var evt = new UIComponentEvent {Component = this};
                Vent.Trigger(key, evt);
            });
        }
    }
}
