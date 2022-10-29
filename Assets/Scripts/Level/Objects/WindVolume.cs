using System;
using UnityEngine;

namespace Level.Objects
{
   public class WindVolume : MonoBehaviour
   {
      [SerializeField] private Transform _directionTransform;
      [SerializeField] private float _strength = 20f; 
      private Vector3 _direction => (_directionTransform.position - transform.position).normalized;

      private void OnTriggerStay2D(Collider2D other)
      {
         if (other.CompareTag("Player") && other.TryGetComponent(out Rigidbody2D rb))
         {
            rb.AddForce(_direction * _strength, ForceMode2D.Force);
         }
      }

      private void OnDrawGizmos()
      {
         Gizmos.color = Color.green;
         var directionPosition = _directionTransform.position;
         Gizmos.DrawLine(transform.position, directionPosition);
         Gizmos.DrawWireSphere(directionPosition, 0.3f);
      }
   }
}
