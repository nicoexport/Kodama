using Kodama.GameManagement;
using UnityEngine;

namespace Kodama
{
    public class LevelParallax : Parallax {
        [SerializeField] private CharacterRuntimeSet _characterRuntimeSet;

        protected override void FixedUpdate() {
            if (!CameraTransform) {
                return;
            }

            if (!_subject) {
                var player = _characterRuntimeSet.GetItemAtIndex(0);
                if (player != null) {
                    _subject = player.transform;
                }

                return;
            }

            float distanceFromSubject = _transform.position.z - _subject.transform.position.z;
            float clippingPlane = _camera.transform.position.z +
                                  (distanceFromSubject > 0 ? _camera.farClipPlane : _camera.nearClipPlane);
            float parallaxFactor = Mathf.Abs(distanceFromSubject / clippingPlane);
            var travel = (Vector2)CameraTransform.position - _startPosition;
            var newPos = _startPosition + (travel * parallaxFactor);
            _transform.position = new Vector3(newPos.x, newPos.y, _startZ);
        }
    }
}