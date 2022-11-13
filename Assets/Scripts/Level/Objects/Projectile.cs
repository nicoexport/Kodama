using System.Collections;
using Kodama.Architecture;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

namespace Kodama.Level.Objects {
    [RequireComponent(typeof(Rigidbody2D))]
    public class Projectile : Resettable {
        [SerializeField] protected float _speed;
        [SerializeField] private LayerMask _collisionIgnoreLayers;
        [SerializeField] protected GameObject explosionEffect;

        public UnityEvent OnDestruct;
        protected Transform _target;
        protected Rigidbody2D rb;

        private void Awake() => TryGetComponent(out rb);

        protected virtual void FixedUpdate() => ChaseTarget();

        private void OnTriggerEnter2D(Collider2D col) {
            if (_collisionIgnoreLayers == (_collisionIgnoreLayers | (1 << col.gameObject.layer))) {
                return;
            }
            Destruct();
        }

        protected virtual void ChaseTarget() {
            if (_target == null) {
                return;
            }

            transform.position = Vector3.MoveTowards(transform.position, _target.position, _speed / 10f);
        }

        private IEnumerator DestroyAfterSeconds_Co(float lifeTimeInSeconds) {
            yield return new WaitForSeconds(lifeTimeInSeconds);
            Destruct();
        }

        private void Destruct() {
            if (explosionEffect) {
                Instantiate(explosionEffect, transform.position, quaternion.identity);
            }

            OnDestruct?.Invoke();
            Destroy(gameObject);
        }

        public virtual void Initialize(Transform target, float lifeTimeInSeconds) {
            _target = target;
            StartCoroutine(DestroyAfterSeconds_Co(lifeTimeInSeconds));
        }

        public virtual void Initialize(Transform target, float lifeTimeInSeconds, float speed) {
            _target = target;
            _speed = speed;
            StartCoroutine(DestroyAfterSeconds_Co(lifeTimeInSeconds));
        }

        public override void OnLevelReset() => Destroy(gameObject);
    }
}