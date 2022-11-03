using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Kodama.Level.Logic {
    public class HellCollider : MonoBehaviour {
        [FormerlySerializedAs("_deathTimeInSeconds")] [SerializeField]
        private float _timeToKillInSeconds = 1.5f;

        private void OnTriggerEnter2D(Collider2D other) {
            if (other.CompareTag("Player")) {
                OnTriggerEntered?.Invoke(_timeToKillInSeconds);
                print("ENTERED HELL COLLIDER");
            }
        }

        public static event Action<float> OnTriggerEntered;
    }
}