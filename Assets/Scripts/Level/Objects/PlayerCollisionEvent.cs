using UnityEngine;
using UnityEngine.Events;

namespace Level.Objects
{
    public class PlayerCollisionEvent : MonoBehaviour
    {
        [SerializeField] UnityEvent collisionEnterEvent;

        [SerializeField] UnityEvent collisionExitEvent;

        void OnCollisionEnter2D(Collision2D col)
        {
            if (col.collider.tag != "Player") return;
            collisionEnterEvent.Invoke();
        }

        void OnCollisionExit2D(Collision2D col)
        {
            if (col.collider.tag != "Player") return;
            collisionExitEvent.Invoke();
        }
    }
}