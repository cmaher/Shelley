using Maru.MCore;
using Shelley.Scripts.ShelleyStudio.Events;
using TMPro;
using UnityEngine.UI;

namespace Shelley.Scripts.ShelleyStudio.UI {
    public class EnumSelectorButton : VentBehavior {
        // use an interface, because generic behaviors are not supported
        public IEnumSelection selection;
        public string label;

        protected override void Awake() {
            base.Awake();
            GetComponent<Button>().onClick.AddListener(TriggerSelected);
        }

        protected void Start() {
            transform.Find("Label").GetComponent<TMP_Text>().text = label;
        }

        private void TriggerSelected() {
            selection.TriggerEvent(Vent);
        }
    }
}
