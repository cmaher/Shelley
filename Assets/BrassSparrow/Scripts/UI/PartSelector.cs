using System;
using Doozy.Engine.UI;
using Maru.MCore;
using Maru.Scripts.MUI;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;

namespace BrassSparrow.Scripts.UI {
    public class PartSelector : MonoBehaviour {
        public float meshDistance;
        public Color unselectedColor;
        public Color selectedColor;
        public GameObject mesh;
        public float meshScale;

        private IMessageBus vent;
        private UIButton doozyButton;
        private ProceduralImage box;
        private UIMesh uiMesh;

        public void Awake() {
            var locator = LocatorProvider.Get();
            vent = locator.Get(SceneManager.VentKey) as IMessageBus;
            doozyButton = GetComponent<UIButton>();

            var evtKey = DoozyEvents.DoozyEventKey(DoozyEvents.PartSelectorClickEvent.Prefix, this);
            doozyButton.OnClick.OnTrigger.GameEvents.Add(evtKey);

            // Configure mesh
            mesh.transform.parent = transform;
            uiMesh = mesh.AddComponent<UIMesh>();
            uiMesh.unscaledDistance = meshDistance;

            box = GetComponent<ProceduralImage>();
        }

        public void Start() {
            vent.Trigger(new RegisterUiComponentEvent {Component = this});
        }

        public void OnDestroy() {
            vent.Trigger(new UnregisterUiComponentEvent {Component = this});
        }

        public void SetSelected(bool selected) {
            if (selected) {
                box.color = selectedColor;
            } else {
                box.color = unselectedColor;
            }
        }
    }
}
