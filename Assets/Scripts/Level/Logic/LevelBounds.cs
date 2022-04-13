using System;
using GameManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Level.Logic
{
    public class LevelBounds : MonoBehaviour
    {
        public static event Action<float> OnNearingLevelBounds;
        
        [SerializeField] private CharacterRuntimeSet _characterRuntime;
        [SerializeField] private float _innerRadius;
        [SerializeField] private float _innerMargin = 1f;
        [SerializeField] private float _outerRadius;
        [SerializeField] private Light2D _playerLight;
        
        private Transform _transform;
        private Transform _playerTransform;
        
        private void Awake()
        {
            _transform = transform;
        }

        private void FixedUpdate()
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

        private float PlayerDistance(Transform playerTransform)
        {
            return Mathf.Abs(Vector2.Distance(playerTransform.position, _transform.position));
        }

        private static float Remap(float iMin, float iMax, float oMin, float oMax, float value)
        {
            var t = Mathf.InverseLerp(iMin, iMax, value);
            return Mathf.Lerp(oMin, oMax, t);
        }

        private void OnDrawGizmosSelected()
        {
            if (_playerLight)
                _outerRadius = _playerLight.pointLightOuterRadius;
            Gizmos.color = Color.blue;
            var position = transform.position;
            Gizmos.DrawWireSphere(position, _outerRadius);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.gray;
            var position = transform.position;
            Gizmos.DrawWireSphere(position, _innerRadius);
        }
    }
}