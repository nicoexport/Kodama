using System;
using System.Collections;
using Architecture;
using UnityEngine;
using UnityEngine.Events;

namespace Level.Objects
{
   [RequireComponent(typeof(Rigidbody2D))]
   public class Projectile : Resettable
   {
      [SerializeField] protected float _speed;
      [SerializeField] private LayerMask _collisionIgnoreLayers;
      protected Transform _target;
      protected Rigidbody2D rb;
      
      public UnityEvent OnCollision;

      private void Awake()
      {
         rb = GetComponent<Rigidbody2D>();
      }

      protected virtual void FixedUpdate()
      {
         ChaseTarget();
      }

      protected virtual void ChaseTarget()
      {
         if (_target == null) return;
         transform.position = Vector3.MoveTowards(transform.position, _target.position, _speed / 10f);
      }

      private IEnumerator DestroyAfterSeconds_Co(float lifeTimeInSeconds)
      {
         yield return new WaitForSeconds(lifeTimeInSeconds);
         Destroy(gameObject);
      }

      private void OnTriggerEnter2D(Collider2D col)
      {
         if (_collisionIgnoreLayers == (_collisionIgnoreLayers | 1 << col.gameObject.layer))
         {
            Debug.Log(col.gameObject.layer);
            return;
         }
         Destroy(gameObject);
         OnCollision?.Invoke();
      }

      public virtual void Initialize(Transform target, float lifeTimeInSeconds)
      {
         _target = target;
         StartCoroutine(DestroyAfterSeconds_Co(lifeTimeInSeconds));
      }
      
      public virtual void Initialize(Transform target, float lifeTimeInSeconds, float speed)
      {
         _target = target;
         _speed = speed;
         StartCoroutine(DestroyAfterSeconds_Co(lifeTimeInSeconds));
      }

      public override void OnLevelReset()
      {
         Destroy(gameObject);
      }
   }
}
