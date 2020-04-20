using System.Collections;

namespace Maru.MCore {
    public class DeferredEvent<TEvent>: IEnumerator {
        private TEvent triggeredEvt = default;
        private bool triggered = false;

        public DeferredEvent(IMessageBus vent) {
            vent.Once<TEvent>(evt => {
                triggered = true;
                triggeredEvt = evt;
            });
        }
        
        // Yield after the event has been triggered
        public bool MoveNext() {
            return !triggered;
        }

        // Yield the event that was triggered
        public object Current => triggeredEvt;

        public void Reset() {}
    }
}
