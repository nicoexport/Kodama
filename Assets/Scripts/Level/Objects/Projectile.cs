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
      [SerializeField] private float _lifeTimeInSeconds;
      protected Transform _target;
      protected Rigidbody2D rb;

      public UnityEvent OnCollision;

      private void Awake()
      {
         StartCoroutine(DestroyAfterSeconds_Co());
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

      private IEnumerator DestroyAfterSeconds_Co()
      {
         yield return new WaitForSeconds(_lifeTimeInSeconds);
         Destroy(gameObject);
      }

      private void OnTriggerEnter2D(Collider2D col)
      {
         Destroy(gameObject);
         OnCollision?.Invoke();
      }

      virtual public void Initialize(Transform target)
      {
         _target = target;
      }
      
      public void Initialize(Transform target, float speed)
      {
         _target = target;
         _speed = speed;
      }

      public override void OnLevelReset()
      {
         Destroy(gameObject);
      }
   }
}
