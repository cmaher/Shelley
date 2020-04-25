using System;
using BrassSparrow.Scripts.Doll;
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
        
        public DollPart DollPart;
        public GameObject masterCanvas;
        public float meshScale;
        
        private GameObject mesh;
        private IMessageBus vent;
        private UIButton doozyButton;
        private ProceduralImage box;
        private UIMesh uiMesh;

        private void Awake() {
            var locator = LocatorProvider.Get();
            vent = locator.Get(SceneManager.VentKey) as IMessageBus;
            doozyButton = GetComponent<UIButton>();

            var evtKey = DoozyEvents.DoozyEventKey(DoozyEvents.PartSelectorClickEvent.Prefix, this);
            doozyButton.OnClick.OnTrigger.GameEvents.Add(evtKey);

            box = GetComponent<ProceduralImage>();
        }

        public void SetDollPart(DollPart part) {
            DollPart = part;
            mesh = Instantiate(part.Go, transform);
        }

        private void Start() {
            vent.Trigger(new RegisterUiComponentEvent {Component = this});
            
            // Configure mesh
            mesh.transform.parent = transform;
            mesh.layer = gameObject.layer;
            mesh.transform.localRotation = new Quaternion(0, 180, 0, 0);
            
            uiMesh = mesh.AddComponent<UIMesh>();
            uiMesh.canvas = masterCanvas;
            uiMesh.unscaledDistance = meshDistance;
            mesh.transform.localScale = new Vector3(meshScale, meshScale, meshScale);
        }

        private void OnDestroy() {
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
