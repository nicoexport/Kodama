using System;
using Kodama.Architecture;
using UnityEngine;

namespace Kodama.Player {
    public class ShieldCollectable : Resettable {
        [SerializeField] private Renderer rend;
        private bool collected = false;

        protected void OnValidate() {
            if (!rend) {
                TryGetComponent(out rend);
            }
        }

        protected void OnTriggerEnter2D(Collider2D col) {
            if (!col.CompareTag("Player")) {
                return;
            }
            if (collected) {
                return;
            }
            
            var health = col.GetComponentInChildren<PlayerHealth>();
            if (!health) {
                return;
            }

            if (health.GainShield()) {
                Collect();
            }
        }

        private void Collect() {
            if (collected) {
                return;
            }
            collected = true;
            rend.enabled = false;
        }

        public override void OnLevelReset() {
            rend.enabled = true;
            collected = false;
        }
    }
}