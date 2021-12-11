using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PickupObject : MonoBehaviour
{
    public UnityEvent pickupEvent;

    public virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag != "Player") return;
        PickUp(col);
    }

    public virtual void PickUp(Collider2D col)
    {
        pickupEvent.Invoke();
        gameObject.SetActive(false);
    }
}
