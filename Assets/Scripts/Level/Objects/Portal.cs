using UnityEngine;
using UnityEngine.Events;

namespace Kodama.Level.Objects {
    public class Portal : MonoBehaviour {
        [SerializeField] private Transform exit;
        public UnityEvent onEnter;

        private void OnTriggerEnter2D(Collider2D entity) {
            if (!entity.TryGetComponent(out PortalUser user) || !user.CanUse) {
                return;
            }

            entity.transform.position = exit.transform.position;
            user.CanUse = false;
            onEnter?.Invoke();
        }
    }
}