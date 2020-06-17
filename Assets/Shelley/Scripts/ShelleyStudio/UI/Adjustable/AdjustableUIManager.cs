using System;
using System.Collections.Generic;
using Maru.MCore;
using UnityEngine;

namespace ShelleyStudio.UI.Adjustable {
    public class AdjustableUIManager {
        private Dictionary<int, IExpandableUI> expandables;
        private Dictionary<int, IScalableUI> scalables;
        private Canvas canvas;
        private Action[] unregister;

        public AdjustableUIManager(Canvas canvas) {
            expandables = new Dictionary<int, IExpandableUI>();
            scalables = new Dictionary<int, IScalableUI>();
            this.canvas = canvas;
        }

        public void Listen(IMessageBus vent) {
            if (unregister != null) {
                Debug.LogError("Cannot call listen more than once");
                return;
            }

            var count = 0;
            unregister = new Action[4];
            unregister[count++] = vent.On<RegisterExpandableUIEvent>(evt => {
                expandables[evt.Id] = evt.Expandable;
                evt.Expandable.SetCanvas(canvas);
            });
            unregister[count++] = vent.On<UnregisterExpandableUIEvent>(evt => { expandables.Remove(evt.Id); });
            unregister[count++] = vent.On<RegisterScalableUIEvent>(evt => { scalables[evt.Id] = evt.Scalable; });
            unregister[count++] = vent.On<UnregisterScalableUIEvent>(evt => { scalables.Remove(evt.Id); });
        }

        public void Shutdown() {
            foreach (var action in unregister) {
                action();
            }
            expandables = null;
            scalables = null;
        }

        public void ScaleUI(float scale) {
            foreach (var scalable in scalables.Values) {
                scalable.Scale(scale);
            }
        }

        public void ExpandUI(Vector2 expand) {
            foreach (var expandable in expandables.Values) {
                expandable.Expand(expand);
            }
        }
    }
}
