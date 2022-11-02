using Cinemachine;
using GameManagement;
using Player;
using UnityEngine;

namespace Level.Logic {
    public class LevelConfiner : MonoBehaviour {
        [SerializeField] private GameObjectRuntimeSet _cinemachineRuntimeSet;
        [SerializeField] private CharacterRuntimeSet _playerRuntimeSet;
        private Collider2D _collider;
        private Transform _playerTransform;
        private Rigidbody2D _rb;

        private void Awake() {
            TryGetComponent(out _collider);
            TryGetComponent(out _rb);
            var position = transform.position;
        }

        protected void Start() => SetCamCollider();

        private void Update() {
            if (_playerTransform) {
                GetPlayerCollider();
            } else {
                CheckBounds();
            }
        }

        private void GetPlayerCollider() {
            if (_playerRuntimeSet.TryGetFirst(out var player)) {
                _playerTransform = player.transform;
            }
        }

        private void CheckBounds() {
            if (!_rb.OverlapPoint(_playerTransform.position)) {
                KillPlayer();
            }
        }

        private void KillPlayer() {
            if (_playerTransform.TryGetComponent(out PlayerHealth health)) {
                health.Die();
            }
        }

        private void SetCamCollider() {
            var cmCam = _cinemachineRuntimeSet.GetItemAtIndex(0);
            if (cmCam == null) {
                return;
            }

            if (!cmCam.TryGetComponent(out CinemachineConfiner2D confiner)) {
                return;
            }

            if (!TryGetComponent(out Collider2D col)) {
                return;
            }

            confiner.m_BoundingShape2D = col;
        }
    }
}