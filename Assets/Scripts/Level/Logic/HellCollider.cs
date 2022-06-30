using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Level.Logic
{
    public class HellCollider : MonoBehaviour
    {
        [FormerlySerializedAs("_deathTimeInSeconds")] [SerializeField]
        float _timeToKillInSeconds = 1.5f;

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                OnTriggerEntered?.Invoke(_timeToKillInSeconds);
                print("ENTERED HELL COLLIDER");
            }
        }

        public static event Action<float> OnTriggerEntered;
    }
}