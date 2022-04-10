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
        if (other.CompareTag("Player"))
        {
            var lifeHandler = other.GetComponent<CharacterLifeHandler>();
            if (lifeHandler != null) lifeHandler.TakeDamage(damage);
        }
        if (destroyedOnGroundCollision && other.CompareTag("Ground"))
        {
            Destroy(gameObject, 0.1f);
        }
    }
}
