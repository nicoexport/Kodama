using System;
using Kodama.Architecture;
using Kodama.Utility;
using UnityEngine;

namespace Kodama {
    public class Effect : Resettable {

        [SerializeField] protected float lifeTime = 1f;
        
        public override void OnLevelReset() => Destroy(gameObject);

        private void Awake() {
            StartCoroutine(Utilities.ActionAfterDelayEnumerator(lifeTime, () => Destroy(gameObject)));
        }
    }
}