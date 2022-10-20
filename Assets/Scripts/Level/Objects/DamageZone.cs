using Player;
using UnityEngine;

namespace Level.Objects
{
    public class DamageZone : MonoBehaviour
    {
        public int damage = 1;

        protected void OnTriggerEnter2D(Collider2D col) 
        {
            OnEnter(col);
        }

        protected void OnEnter(Collider2D col)
        {
            if (col.CompareTag("Player")) 
            {
                if (col.TryGetComponent(out PlayerLifeCycleHandler lifeHandler))
                {
                    lifeHandler.TakeDamage(damage);
                }
            }
        }
    }
}