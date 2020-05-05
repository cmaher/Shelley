using System;
using BrassSparrow.Scripts.Doll;
using Doozy.Engine.UI;

namespace BrassSparrow.Scripts.UI {
    public class PartTypeSelector : DoozyBehavior {
        public DollPartType type;
        public String label;

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
            Vent.Trigger(new PartTypeSelectedEvent {Type = type});
        }
    }

    public struct PartTypeSelectedEvent {
        public DollPartType Type;
    }
}
