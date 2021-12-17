using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerCollisionEvent : MonoBehaviour
{
    [SerializeField]
    private UnityEvent collisionEnterEvent;
    [SerializeField]
    private UnityEvent collisionExitEvent;

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.tag != "Player") return;
        collisionEnterEvent.Invoke();
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.collider.tag != "Player") return;
        collisionExitEvent.Invoke();
    }
}
