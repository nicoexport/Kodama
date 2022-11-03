using UnityEngine;
using UnityEngine.Events;

namespace Kodama.Pickups {
    public class PickupObject : MonoBehaviour {
        public UnityEvent pickupEvent;

        public virtual void OnTriggerEnter2D(Collider2D col) {
            if (col.tag != "Player") {
                return;
            }

            PickUp(col);
        }

        public virtual void PickUp(Collider2D col) {
            pickupEvent.Invoke();
            gameObject.SetActive(false);
        }
    }
}