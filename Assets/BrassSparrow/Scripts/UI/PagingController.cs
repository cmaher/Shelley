using Doozy.Engine.UI;

namespace BrassSparrow.Scripts.UI {
    public class PagingController : DoozyBehavior {
        public int advancePages = 1; // use -1 for previous
        public string ventKey;

        private DoozySelfEvent selfEvent;

        protected override int EventCapacity => 1;

        protected override void Awake() {
            base.Awake();
            var doozyButton = GetComponent<UIButton>();
            doozyButton.OnClick.OnTrigger.GameEvents.Add(DoozySelfEvent.DoozyEventKey(this));
            
            OnSelfEvent(TriggerPagination);
        }

        private void TriggerPagination() {
            Vent.Trigger(new PaginationEvent {
                Key = ventKey,
                AdvancePages = advancePages
            });
        }
    }
}
