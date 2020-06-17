using System;
using Maru.MCore;
using UnityEngine;

namespace ShelleyStudio.UI.Adjustable {
    public interface IExpandableUI {
        void SetCanvas(Canvas canvas);

        // negative values contract
        void Expand(Vector2 expand);
    }

    public class ExpandableUI : MonoBehaviour, IExpandableUI {
        private Vector3 defaultPosition;
        private Canvas canvas;
        private Camera uiCamera;
        private Vector2 sign;
        private IMessageBus vent;

        public void Awake() {
            defaultPosition = transform.position;
            vent = LocatorProvider.Get().Get(MaruKeys.Vent) as IMessageBus;
        }

        public void Start() {
            vent.Trigger(new RegisterExpandableUIEvent {
                Id = GetInstanceID(),
                Expandable = this,
            });
        }

        public void OnDestroy() {
            vent.Trigger(new UnregisterExpandableUIEvent {Id = GetInstanceID()});
        }

        public void SetCanvas(Canvas canvas) {
            var canvasTransform = canvas.GetComponent<RectTransform>();
            var center = canvasTransform.InverseTransformPoint(defaultPosition);
            sign = new Vector2(Math.Sign(center.x), Math.Sign(center.y));
        }

        public void Expand(Vector2 expand) {
            var x = expand.x * sign.x;
            var y = expand.y * sign.y;
            transform.position = defaultPosition + new Vector3(x, y, 0);
        }
    }
}
