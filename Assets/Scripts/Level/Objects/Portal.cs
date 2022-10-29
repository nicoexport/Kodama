using UnityEngine;
using UnityEngine.Events;

namespace Level.Objects
{
    public class Portal : MonoBehaviour
    {
        [SerializeField] private Transform exit;
        public UnityEvent OnEnter;
        private void OnTriggerEnter2D(Collider2D entity)
        {
            if (entity.TryGetComponent(out PortalUser user) && user.CanUse)
            {
                entity.transform.position = exit.transform.position;
                user.CanUse = false;
                OnEnter?.Invoke();
            }
        }
    }
}