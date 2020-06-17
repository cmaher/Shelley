using UnityEngine;

namespace Maru.MUI {
    public class UIMesh : MonoBehaviour {
        [Tooltip(">0 -> in front of canvas, toward camera")]
        public float unscaledDistance;

        [Tooltip("The canvas to match the scaling of")]
        public GameObject canvas;

        private void Update() {
            var tf = transform;
            var currentPosition = tf.localPosition;
            tf.localPosition = new Vector3(
                currentPosition.x,
                currentPosition.y,
                -1 * unscaledDistance / canvas.transform.localScale.z
            );
        }
    }
}
