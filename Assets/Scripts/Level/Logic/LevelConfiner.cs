using Cinemachine;
using Kodama.GameManagement;
using Kodama.Player;
using UnityEngine;

namespace Kodama.Level.Logic {
    public class LevelConfiner : MonoBehaviour {
        [SerializeField] private GameObjectRuntimeSet _cinemachineRuntimeSet;
        [SerializeField] private CharacterRuntimeSet _playerRuntimeSet;
        private Transform _playerTransform;
        private Rigidbody2D _rb;

        private void Awake() {
            TryGetComponent(out _rb);
        }

        protected void Start() => SetCamCollider();

        private void Update() {
            if (_playerTransform == null) {
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
            var health = _playerTransform.GetComponentInChildren<PlayerHealth>();
            if (health) {
                health.Die();
            } else {
                Debug.LogError("Could not find PlayerHealth component on: ", _playerTransform);
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