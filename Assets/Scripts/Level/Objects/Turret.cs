using System.Collections;
using Architecture;
using GameManagement;
using Player;
using UnityEngine;
using UnityEngine.Events;

namespace Level.Objects
{
    public class Turret : Resettable
    {
        [SerializeField] private GameObject _projectilePrefab;
        [SerializeField] private CharacterRuntimeSet _playerRuntimeSet;
        [SerializeField] private float _range;
        [SerializeField] private float _cooldownInSeconds;
        [SerializeField] private float _heatupInSeconds;
        [SerializeField] private bool _needsLineOfSight;
        [SerializeField] private LayerMask _whatIsPlayer;
        
        private Character _player;
        private Coroutine _shootCoroutine;
        private bool _canShoot => (_player || _playerRuntimeSet.TryGetFirst(out _player))
                                  && Vector2.Distance(transform.position, _player.transform.position) <= _range;

        public UnityEvent OnStartHeatUp;
        public UnityEvent OnShoot;
        public UnityEvent OnCooldown;
        
        public override void OnLevelReset()
        {
            if (_shootCoroutine != null)
                StopCoroutine(_shootCoroutine);
            _shootCoroutine = StartCoroutine(Shoot_Co());
        }

        private IEnumerator Shoot_Co()
        {
            while (true)
            {
                yield return new WaitUntil(()=> _canShoot);
                if (HasLineOfSight())
                {
                    Heatup();
                    yield return new WaitForSeconds(_heatupInSeconds);
                }
                if (HasLineOfSight())
                {
                    Shoot();
                    yield return new WaitForSeconds(_cooldownInSeconds);
                    OnCooldown?.Invoke();
                }
                else
                {
                    OnCooldown?.Invoke();
                }
            }
        }

        private void Heatup()
        {
            OnStartHeatUp.Invoke();
        }

        private bool HasLineOfSight()
        {
            if (!_needsLineOfSight) return true;
            var position = transform.position;
            var dir = -(position - _player.transform.position);
            var res = Physics2D.Raycast(position, dir, _range);
            Debug.Log(res.collider.CompareTag("Player"));
            return (res.collider.CompareTag("Player"));
        }
        
        private void Shoot()
        {
            var projectileObj = Instantiate(_projectilePrefab, transform.position, Quaternion.identity);
            if (!projectileObj.TryGetComponent(out Projectile projectile)) return;
            if(_player)projectile.Initialize(_player.transform);
            OnShoot.Invoke();
        }
        
        protected void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _range);
        }
    }
}