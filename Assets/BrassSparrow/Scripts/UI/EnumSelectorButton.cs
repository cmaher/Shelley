using System;
using Doozy.Engine.UI;
using Maru.MCore;

namespace BrassSparrow.Scripts.UI {
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

    public interface IEnumSelection {
        void TriggerEvent(IMessageBus vent);
        int EnumValue();
    }

    public class EnumSelection<T> : IEnumSelection where T : Enum {
        private EnumSelectedEvent<T> evt;

        public EnumSelection(T type, string ventKey) {
            evt = new EnumSelectedEvent<T> {Key = ventKey, Type = type};
        }

        public int EnumValue() {
            return (int) (object) evt.Type;
        }

        public void TriggerEvent(IMessageBus vent) {
            vent.Trigger(evt);
        }
    }

    public struct EnumSelectedEvent<T> : IKeyedEvent where T : Enum {
        public string Key;
        public T Type;

        public string GetEventKey() {
            return Key;
        }
    }
}
