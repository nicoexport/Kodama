using UnityEngine;

namespace Level.Objects {
    public class WindVolume : MonoBehaviour {
        [SerializeField] private Transform _directionTransform;
        [SerializeField] private float _strength = 20f;

        private Vector3 _direction => (_directionTransform.position - _directionTransform.right
                                                                    - (_directionTransform.position +
                                                                       _directionTransform.right)).normalized;

        private void OnDrawGizmos() {
            var directionPosition = _directionTransform.position;
            var right = _directionTransform.right;
            var pos1 = directionPosition + right;
            var pos2 = directionPosition - right;

            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(pos1, pos2);
            Gizmos.DrawWireSphere(pos2, 0.3f);
        }

        private void OnTriggerStay2D(Collider2D other) {
            if (other.CompareTag("Player") && other.TryGetComponent(out Rigidbody2D rb)) {
                rb.AddForce(_direction * _strength, ForceMode2D.Force);
            }
        }
    }
}