using UnityEngine;

namespace Level.Objects {
    public class Bullet : Projectile {
        protected override void FixedUpdate() {
        }

        public override void Initialize(Transform target, float lifeTimeInSeconds) {
            base.Initialize(target, lifeTimeInSeconds);
            var dir = (target.position - transform.position).normalized;
            rb.AddForce(dir * _speed, ForceMode2D.Impulse);
        }

        public override void Initialize(Transform target, float lifeTimeInSeconds, float speed) {
            base.Initialize(target, lifeTimeInSeconds, speed);
            var dir = (target.position - transform.position).normalized;
            rb.AddForce(dir * _speed, ForceMode2D.Impulse);
        }
    }
}