using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    public int damage = 1;
    [SerializeField]
    private bool destroyedOnGroundCollision = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            var character = other.GetComponent<Character>();
            if (character != null) character.LifeHandler.TakeDamage(damage);
        }
        if (destroyedOnGroundCollision && other.tag == "Ground")
        {
            Destroy(gameObject, 0.1f);
        }
    }
}
