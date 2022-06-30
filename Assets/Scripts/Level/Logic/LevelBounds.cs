using System;
using GameManagement;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Level.Logic
{
    public class LevelBounds : MonoBehaviour
    {
        [SerializeField] CharacterRuntimeSet _characterRuntime;
        [SerializeField] float _innerRadius;
        [SerializeField] float _innerMargin = 1f;
        [SerializeField] float _outerRadius;
        [SerializeField] Light2D _playerLight;
        Transform _playerTransform;

        Transform _transform;

        void Awake()
        {
            _transform = transform;
        }

        void FixedUpdate()
        {
            if (!_playerTransform)
            {
                var player = _characterRuntime.GetItemAtIndex(0);
                if (player != null) _playerTransform = player.transform;
                return;
            }

            var distance = PlayerDistance(_playerTransform);
            if (distance < _innerRadius - _innerMargin) return;
            var remappedValue = Remap(_innerRadius, _outerRadius, 0, 1, distance);
            OnNearingLevelBounds?.Invoke(remappedValue);
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.gray;
            var position = transform.position;
            Gizmos.DrawWireSphere(position, _innerRadius);
        }

        void OnDrawGizmosSelected()
        {
            if (_playerLight)
                _outerRadius = _playerLight.pointLightOuterRadius;
            Gizmos.color = Color.blue;
            var position = transform.position;
            Gizmos.DrawWireSphere(position, _outerRadius);
        }

        public static event Action<float> OnNearingLevelBounds;

        float PlayerDistance(Transform playerTransform)
        {
            return Mathf.Abs(Vector2.Distance(playerTransform.position, _transform.position));
        }

        static float Remap(float iMin, float iMax, float oMin, float oMax, float value)
        {
            var t = Mathf.InverseLerp(iMin, iMax, value);
            return Mathf.Lerp(oMin, oMax, t);
        }
    }
}