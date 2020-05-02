using System;
using BrassSparrow.Scripts.Core;

namespace BrassSparrow.Scripts.UI {
    public class DoozyBehavior : BrassSparrowBehavior {
        protected virtual void Start() {
            vent.Trigger(new RegisterUiComponentEvent {Component = this});
        }

        protected override void OnDestroy() {
            base.OnDestroy();
            vent.Trigger(new UnregisterUiComponentEvent {Component = this});
        }

        protected void OnSelfEvent(Action handler) {
            Action<DoozySelfEvent> handleSelf = _ => handler();
            On(GetInstanceID().ToString(), handleSelf);
        }
    }
}
