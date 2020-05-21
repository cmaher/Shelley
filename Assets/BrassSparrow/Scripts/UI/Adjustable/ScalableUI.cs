using Maru.MCore;
using UnityEngine;

namespace BrassSparrow.Scripts.UI.Adjustable {
    public interface IScalableUI {
        void Scale(float factor);
    }

    public class ScalableUI : MonoBehaviour, IScalableUI {
        private Vector3 defaultScale;
        private IMessageBus vent;
        
        public void Awake() {
            defaultScale = transform.localScale;
            vent = LocatorProvider.Get().Get(MaruKeys.Vent) as IMessageBus;
        }

        public void Start() {
            vent.Trigger(new RegisterScalableUIEvent {
                Id = GetInstanceID(),
                Scalable = this,
            });
        }

        public void OnDestroy() {
            vent.Trigger(new UnregisterScalableUIEvent {Id = GetInstanceID()});
        }

        public void Scale(float factor) {
            transform.localScale = defaultScale * factor;
        }
    }
}
