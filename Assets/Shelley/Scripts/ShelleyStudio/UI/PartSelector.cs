using Maru.MCore;
using Maru.Scripts.MUI;
using Shelley.Scripts.Shelley;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;

namespace Shelley.Scripts.ShelleyStudio.UI {
    public class PartSelector : VentBehavior {
        public float meshDistance;
        public Color unselectedColor;
        public Color selectedColor;

        public DollPart DollPart;
        public GameObject masterCanvas;
        public float defaultScale = 1;
        public float backAttachmentScale = 1;
        public float torsoScale = 1;

        private GameObject mesh;
        private ProceduralImage box;
        private UIMesh uiMesh;

        protected override void Awake() {
            base.Awake();
            var button = GetComponent<Button>();
            button.onClick.AddListener(() => { Vent.Trigger(new PartSelectedEvent {PartSelector = this}); });
            box = GetComponent<ProceduralImage>();
        }

        public void SetDollPart(DollPart part) {
            DollPart = part;
            if (mesh != null) {
                Destroy(mesh);
            }

            mesh = Instantiate(part.Go, transform);
        }

        protected void Start() {
            if (mesh == null) {
                return;
            }

            mesh.transform.parent = transform;
            mesh.layer = gameObject.layer;
            mesh.transform.localRotation = new Quaternion(0, 180, 0, 0);

            uiMesh = mesh.AddComponent<UIMesh>();
            uiMesh.canvas = masterCanvas;
            uiMesh.unscaledDistance = meshDistance;

            float scale;
            switch (DollPart.Type) {
                case DollPartType.Torso:
                    scale = torsoScale;
                    break;
                case DollPartType.BackAttachment:
                    scale = backAttachmentScale;
                    break;
                default:
                    scale = defaultScale;
                    break;
            }

            mesh.transform.localScale = new Vector3(scale, scale, scale);
        }

        public void SetSelected(bool selected) {
            if (selected) {
                box.color = selectedColor;
            } else {
                box.color = unselectedColor;
            }
        }
    }

    public struct PartSelectedEvent : IKeyedEvent {
        public const string Prefix = "PartSelector";
        public string Key;
        public PartSelector PartSelector;

        public string GetEventKey() {
            return Key;
        }
    }
}
