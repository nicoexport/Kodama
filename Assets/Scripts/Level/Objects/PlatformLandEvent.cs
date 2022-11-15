using System;
using UnityEngine;

namespace Kodama.Level.Objects {
    public class PlatformLandEvent : PlayerCollisionEvent {
        protected override void OnCollisionEnter2D(Collision2D col) {
            if (!col.collider.CompareTag("Player")) {
                return;
            }

            if (col.transform.position.y < transform.position.y) {
                return;
            }

            collisionEnterEvent.Invoke();
        }
    }
}