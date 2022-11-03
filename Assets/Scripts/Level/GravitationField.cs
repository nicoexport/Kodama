using System;
using UnityEngine;

namespace Kodama.Level {
    public class GravitationField : MonoBehaviour {
        [SerializeField] private float _intensity;
        [SerializeField] private float _range = 1f;
        [SerializeField] private LayerMask _whatToAttract;
        [SerializeField] private GravityForceType _forceType;
        [SerializeField] private ForceMode2D _forceMode;
        [SerializeField] private bool _inverse;
        private Transform _transform;

        protected void FixedUpdate() {
            var hits = Physics2D.CircleCastAll(_transform.position, _range, Vector2.one, 0f, _whatToAttract);
            foreach (var hit in hits) {
                var hitPosition = hit.transform.position;
                var position = _transform.position;
                float distance = Vector2.Distance(hitPosition, position);
                if (distance < 0.1f) {
                    return;
                }

                Vector2 force;
                switch (_forceType) {
                    case GravityForceType.InvLerp:
                        force = (position - hitPosition).normalized / Mathf.InverseLerp(0, _range, distance) *
                                _intensity;
                        break;
                    case GravityForceType.InvLerpSqr:
                        force = (position - hitPosition).normalized /
                                Mathf.Pow(Mathf.InverseLerp(0, _range, distance), 2) *
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
                        break;
                }

                if (_inverse) {
                    hit.rigidbody.AddForce(-force, _forceMode);
                } else {
                    hit.rigidbody.AddForce(force, _forceMode);
                }
            }
        }

        private void OnDrawGizmosSelected() => Gizmos.DrawWireSphere(_transform.position, _range);

        private void OnValidate() {
            if (!_transform) {
                _transform = transform;
            }
        }
    }

    public enum GravityForceType {
        InvLerp,
        InvLerpSqr,
        Dist,
        DistSqr
    }
}