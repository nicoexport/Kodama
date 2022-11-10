using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Kodama
{
    public class Parallax : MonoBehaviour {
        [SerializeField] protected Transform _subject;
        protected Camera _camera;
        protected Vector2 _startPosition;
        protected float _startZ;
        protected Transform _transform;
        protected Transform _cameraTransform;


        private void Awake() {
            _transform = transform;
            _camera = Camera.main;
            _cameraTransform = _camera.transform;
            var position = transform.position;
            _startPosition = position;
            _startZ = position.z;
        }

        protected virtual void FixedUpdate() {
            if (!_cameraTransform) {
                return;
            }

            if (!_subject) {
                return;
            }

            float distanceFromSubject = _transform.position.z - _subject.transform.position.z;
            float clippingPlane = _camera.transform.position.z +
                                  (distanceFromSubject > 0 ? _camera.farClipPlane : _camera.nearClipPlane);
            float parallaxFactor = Mathf.Abs(distanceFromSubject / clippingPlane);
            var travel = (Vector2)_cameraTransform.position - _startPosition;
            var newPos = _startPosition + (travel * parallaxFactor);
            _transform.position = new Vector3(newPos.x, newPos.y, _startZ);
        }
    }
}