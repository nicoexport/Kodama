using System.Collections;
using Architecture;
using GameManagement;
using Player;
using UnityEngine;

namespace Level.Objects
{
    public class Turret : Resettable
    {
        [SerializeField] private GameObject _projectilePrefab;
        [SerializeField] private CharacterRuntimeSet _playerRuntimeSet;
        [SerializeField] private float _range;
        [SerializeField] private float _cooldown;
        private Character _player;
        private Coroutine _shootCoroutine;
        private bool _canShoot => (_player || _playerRuntimeSet.TryGetFirst(out _player))
         && Vector2.Distance(transform.position, _player.transform.position) <= _range;
        
        public override void OnLevelReset()
        {
            if (_shootCoroutine != null)
                StopCoroutine(_shootCoroutine);
            _shootCoroutine = StartCoroutine(Shoot_Co());
        }

        private void Shoot()
        {
            var projectileObj = Instantiate(_projectilePrefab, transform.position, Quaternion.identity);
            if (!projectileObj.TryGetComponent(out Projectile projectile)) return;
            projectile.Initialize(_player.transform);
        }

        private IEnumerator Shoot_Co()
        {
            while (true)
            {
                yield return new WaitUntil(()=> _canShoot);
                Shoot();
                yield return new WaitForSeconds(_cooldown);
            }
        }
        
        protected void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _range);
        }
    }
}