﻿using UnityEngine;

namespace Kodama.Level.Objects {
    public class Bullet : Projectile {
        protected override void FixedUpdate() {
        }

        public override void Initialize(Transform target) {
            base.Initialize(target);
            var dir = (target.position - transform.position).normalized;
            rb.AddForce(dir * _speed, ForceMode2D.Impulse);
        }
    }
}