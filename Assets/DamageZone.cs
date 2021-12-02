using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    public int damage = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player") other.GetComponent<Character>().LifeHandler.TakeDamage(damage);
    }
}
