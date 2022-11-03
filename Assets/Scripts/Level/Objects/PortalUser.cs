using System.Collections;
using UnityEngine;

namespace Kodama.Level.Objects {
    public class PortalUser : MonoBehaviour {
        [SerializeField] private float _cooldown = 0.2f;
        private bool _canUse = true;

        public bool CanUse {
            get => _canUse;
            set {
                _canUse = value;
                StartCoroutine(Cooldown_Co());
            }
        }

        private IEnumerator Cooldown_Co() {
            yield return new WaitForSeconds(_cooldown);
            _canUse = true;
        }
    }
}