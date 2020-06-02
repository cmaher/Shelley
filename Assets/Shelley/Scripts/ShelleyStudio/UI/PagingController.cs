using Maru.MCore;
using UnityEngine.UI;

namespace Shelley.Scripts.ShelleyStudio.UI {
    public class PagingController : VentBehavior {
        public int advancePages = 1; // use -1 for previous
        public string ventKey;

        protected override void Awake() {
            base.Awake();
            var button = GetComponent<Button>();
            button.onClick.AddListener(TriggerPagination);
        }

        private void TriggerPagination() {
            Vent.Trigger(new PaginationEvent {
                Key = ventKey,
                AdvancePages = advancePages
            });
        }
    }
}
