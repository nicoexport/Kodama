using Kodama.Player;
using UnityEngine;

namespace Kodama.Level.Objects {
    public class DamageZone : MonoBehaviour {
        [SerializeField] private int damage = 1;
        [SerializeField] private Vector2 damageForce;

        protected void OnTriggerEnter2D(Collider2D col) => OnEnter(col);

        private void OnEnter(Collider2D col) {
            if (col.CompareTag("Player")) {
                if (col.TryGetComponent(out PlayerHealth lifeHandler)) {
                    lifeHandler.TakeDamage(damage);
                }

                if (col.TryGetComponent(out Rigidbody2D rb)) {
                    var force = (rb.transform.position - transform.position).normalized;
                    force = new Vector2(force.x * damageForce.x, force.y * damageForce.y);
                    rb.velocity = Vector2.zero;
                    rb.AddForce(force, ForceMode2D.Impulse);
                }
            }
        }
    }
}