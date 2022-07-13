using System;
using UnityEngine;

namespace Level
{
    public class GravitationField : MonoBehaviour
    {
        [SerializeField] float _intensity;
        [SerializeField] float _range = 1f; 
        [SerializeField] LayerMask _whatToAttract;
        Transform _transform;
        
        protected void FixedUpdate()
        {
            var hits = Physics2D.CircleCastAll(_transform.position, _range, Vector2.one,0f, _whatToAttract);
            foreach (var hit in hits)
            {
                Debug.Log(hit.transform.name);
                var hitPosition = hit.transform.position;
                var position = _transform.position;
                var distance = Vector2.Distance(hitPosition, position);
                if (distance < 0.1f) return;
                var force = (position - hitPosition).normalized / Mathf.InverseLerp(0, _range, distance) * _intensity;
                hit.rigidbody.AddForce(force, ForceMode2D.Impulse);
            }
        }

        void OnValidate()
        {
            if (!_transform)
                _transform = transform;
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(_transform.position, _range);
        }
    }
}