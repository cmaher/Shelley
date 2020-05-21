using Maru.MCore;
using UnityEngine;

namespace BrassSparrow.Scripts.UI.Adjustable {

    public class AdjustableUIManagerBehavior : MonoBehaviour {
        public float scale = 1f;
        public Vector2 expand;
        public Canvas canvas;
        
        private AdjustableUIManager manager;
        private float lastScale;
        private Vector2 lastExpand;

        private void Awake() {
            manager = new AdjustableUIManager(canvas);
            manager.Listen(LocatorProvider.Get().Get(MaruKeys.Vent) as IMessageBus);
        }

        private void Start() {
            manager.ScaleUI(scale);
            manager.ExpandUI(expand);
            lastScale = scale;
            lastExpand = expand;
        }

        private void Update() {
            if (scale != lastScale) {
                manager.ScaleUI(scale);
                lastScale = scale;
            }

            if (!expand.Equals(lastExpand)) {
                manager.ExpandUI(expand);
                lastExpand = expand;
            }
        }

        private void OnDestroy() {
            manager.Shutdown();
        }
    }

    public struct RegisterScalableUIEvent {
        public int Id;
        public IScalableUI Scalable;
    }

    public struct UnregisterScalableUIEvent {
        public int Id;
    }

    public struct RegisterExpandableUIEvent {
        public int Id;
        public IExpandableUI Expandable;
    }

    public struct UnregisterExpandableUIEvent {
        public int Id;
    }
}
