using UnityEngine;
using UnityEngine.Events;

namespace Level.Objects
{
    public class TriggerEvent : MonoBehaviour
    {
        [Space(10)] [SerializeField] bool playerOnly = true;

        [Space(10)] public UnityEvent onEnter;

        public UnityEvent onExit;
        public UnityEvent onStay;


        void OnTriggerEnter2D(Collider2D col)
        {
            if (playerOnly && col.tag != "Player") return;
            onEnter.Invoke();
        }

        void OnTriggerExit2D(Collider2D col)
        {
            if (playerOnly && col.tag != "Player") return;
            onExit.Invoke();
        }

        void OnTriggerStay2D(Collider2D col)
        {
            if (playerOnly && col.tag != "Player") return;
            onStay.Invoke();
        }
    }
}