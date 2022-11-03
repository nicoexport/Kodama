using Plugins.LeanTween.Framework;
using UnityEngine;

namespace Kodama
{
    public class ObjectWaver : MonoBehaviour {
        [SerializeField] private bool _playOnAwake;
        [SerializeField] private float _speed;
        [SerializeField] private LeanTweenType _tweenType;
        [SerializeField] private Vector2 _targetOffset;
        private Vector2 _target;


        private void Awake() {
            _target = (Vector2)transform.position + _targetOffset;
            if (_playOnAwake) {
                Wave();
            }
        }

        private void Wave() => LeanTween.move(gameObject, _target, _speed).setEase(_tweenType).setLoopPingPong();

        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.blue;
            var position = transform.position;
            var target2 = (Vector2)position + _targetOffset;
            var target = new Vector3(target2.x, target2.y, position.z);
            Gizmos.DrawLine(position, target);
            Gizmos.DrawCube(target, Vector3.one / 2);
        }
    }
}