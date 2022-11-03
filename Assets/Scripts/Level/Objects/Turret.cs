using System.Collections;
using Kodama.Architecture;
using Kodama.GameManagement;
using Kodama.Player;
using UnityEngine;
using UnityEngine.Events;

namespace Kodama.Level.Objects {
    public class Turret : Resettable {
        [SerializeField] private CharacterRuntimeSet _playerRuntimeSet;
        [SerializeField] private GameObject _projectilePrefab;
        [SerializeField] private float _projectileLifetimeInSeconds;
        [SerializeField] private float _projectileSpeed;
        [SerializeField] private float _range;
        [SerializeField] private float _cooldownInSeconds;
        [SerializeField] private float _heatupInSeconds;
        [SerializeField] private bool _needsLineOfSight;
        [SerializeField] private LayerMask _whatToIgnore;

        public UnityEvent OnStartHeatUp;
        public UnityEvent OnShoot;
        public UnityEvent OnCooldown;

        private Character _player;
        private Coroutine _shootCoroutine;

        private bool _canShoot => (_player || _playerRuntimeSet.TryGetFirst(out _player))
                                  && Vector2.Distance(transform.position, _player.transform.position) <= _range
                                  && HasLineOfSight();

        protected void OnDrawGizmosSelected() {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _range);
        }

        public override void OnLevelReset() {
            if (_shootCoroutine != null) {
                StopCoroutine(_shootCoroutine);
            }

            _shootCoroutine = StartCoroutine(Shoot_Co());
        }

        private IEnumerator Shoot_Co() {
            while (true) {
                yield return new WaitUntil(() => _canShoot);
                Heatup();
                yield return new WaitForSeconds(_heatupInSeconds);
                if (_canShoot) {
                    Shoot();
                    yield return new WaitForSeconds(_cooldownInSeconds);
                    OnCooldown?.Invoke();
                } else {
                    OnCooldown?.Invoke();
                }
            }
        }

        private void Heatup() => OnStartHeatUp.Invoke();

        private bool HasLineOfSight() {
            if (!_needsLineOfSight) {
                return true;
            }

            var position = transform.position;
            var dir = -(position - _player.transform.position);
            var res = Physics2D.Raycast(position, dir, _range, ~_whatToIgnore);
            return res.collider.CompareTag("Player");
        }

        private void Shoot() {
            var projectileObj = Instantiate(_projectilePrefab, transform.position, Quaternion.identity);
            if (!projectileObj.TryGetComponent(out Projectile projectile)) {
                return;
            }

            if (_player) {
                projectile.Initialize(_player.transform, _projectileLifetimeInSeconds, _projectileSpeed);
            }

            OnShoot.Invoke();
        }
    }
}