using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    public class CharacterLifeHandler : MonoBehaviour
    {
        public event Action<Character> OnCharacterDeath;
        public bool Damageable = true;
        [FormerlySerializedAs("defaultHealth")] 
        [SerializeField] private int _defaultHealth = 1;
        private int Health { get; set; }
        private Character _character;

        private void Awake()
        {
            Damageable = true;
            _character = GetComponent<Character>();
            Health = _defaultHealth;
        }

        public void TakeDamage(int amount)
        {
            if(!Damageable) return;
            if (Health <= 0) return;
            Health -= amount;
            if (Health <= 0) Die();
        }

        private void Die()
        {
            OnCharacterDeath?.Invoke(_character);
        }
    
    }
}
