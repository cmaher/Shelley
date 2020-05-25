using Doozy.Engine.UI;
using Shelley.Scripts.ShelleyStudio.Events;

namespace Shelley.Scripts.ShelleyStudio.UI {
    public class EnumSelectorButton : DoozyBehavior {
        // use an interface, because generic behaviors are not supported
        public IEnumSelection selection;
        public string label;

        private UIButton doozyButton;

        protected override int EventCapacity => 1;

        protected override void Awake() {
            base.Awake();
            doozyButton = GetComponent<UIButton>();
            doozyButton.OnClick.OnTrigger.GameEvents.Add(DoozySelfEvent.DoozyEventKey(this));
            OnSelfEvent(TriggerSelected);
        }

        protected override void Start() {
            base.Start();
            doozyButton.SetLabelText(label);
        }

        private void TriggerSelected() {
            selection.TriggerEvent(Vent);
        }
    }
}
