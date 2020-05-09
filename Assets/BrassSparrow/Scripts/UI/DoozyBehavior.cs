using System;
using BrassSparrow.Scripts.Core;

namespace BrassSparrow.Scripts.UI {
    public class DoozyBehavior : BrassSparrowBehavior {
        protected virtual void Start() {
            Vent.Trigger(new RegisterUIComponentEvent {Component = this});
        }

        protected override void OnDestroy() {
            base.OnDestroy();
            Vent.Trigger(new UnregisterUIComponentEvent {Component = this});
        }

        protected void OnSelfEvent(Action handler) {
            Action<DoozySelfEvent> handleSelf = _ => handler();
            On(GetInstanceID().ToString(), handleSelf);
        }
    }
}