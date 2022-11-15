using UnityEngine;
using UnityEngine.Events;

namespace Kodama.Level.Objects {
    public class PlayerCollisionEvent : MonoBehaviour {
        [SerializeField] protected UnityEvent collisionEnterEvent;
        [SerializeField] protected UnityEvent collisionExitEvent;

        protected virtual void OnCollisionEnter2D(Collision2D col) {
            if (!col.collider.CompareTag("Player")) {
                return;
            }

            collisionEnterEvent.Invoke();
        }

        protected void OnCollisionExit2D(Collision2D col) {
            if (!col.collider.CompareTag("Player")) {
                return;
            }

            collisionExitEvent.Invoke();
        }
    }
}