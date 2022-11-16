namespace Kodama.Level.Objects {
    public class Rocket : Projectile {
        protected override void ChaseTarget() {
            if (!_target) {
                return;
            }

            var force = (_target.position - transform.position).normalized * _speed;
            rb.AddForce(force);
            _speed += _speed / 100f;
        }
    }
}