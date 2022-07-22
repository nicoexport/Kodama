using System;
using UnityEngine;

namespace Level
{
    public class GravitationField : MonoBehaviour
    {
        [SerializeField] float _intensity;
        [SerializeField] float _range = 1f; 
        [SerializeField] LayerMask _whatToAttract;
        [SerializeField] GravityForceType _forceType;
        [SerializeField] ForceMode2D _forceMode;
        [SerializeField] bool _inverse;
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

                Vector2 force;
                switch (_forceType)
                {
                    case GravityForceType.InvLerp:
                        force = (position - hitPosition).normalized / Mathf.InverseLerp(0, _range, distance) * _intensity;
                        break;
                    case GravityForceType.InvLerpSqr:
                        force = (position - hitPosition).normalized / Mathf.Pow(Mathf.InverseLerp(0, _range, distance), 2) * 
                        _intensity;
                        break;
                    case GravityForceType.Dist:
                        force = (position - hitPosition).normalized / distance * _intensity;
                        break;
                    case GravityForceType.DistSqr:
                        force = (position - hitPosition).normalized / Mathf.Pow(distance, 2) * _intensity;
                        break;
                    default:
                        force = (position - hitPosition).normalized / distance * _intensity;
                        throw new ArgumentOutOfRangeException();
                }
                if(_inverse)
                    hit.rigidbody.AddForce(-force, _forceMode);
                else
                {
                    hit.rigidbody.AddForce(force, _forceMode);
                }
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

    public enum GravityForceType
    {
        InvLerp,
        InvLerpSqr,
        Dist,
        DistSqr
    }
}