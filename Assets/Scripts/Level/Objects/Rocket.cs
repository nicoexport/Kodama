using UnityEngine;

namespace Level.Objects {
    public class Rocket : Projectile {
        protected override void ChaseTarget() {
            if (_target == null) {
                return;
            }

            transform.position = Vector3.MoveTowards(transform.position, _target.position, _speed / 10f);
            _speed += _speed / 100f;
        }
    }
}