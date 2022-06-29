using System;
using Architecture;
using GameManagement;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    public class PlayerLifeCycleHandler : Resettable
    {
        [SerializeField] TransformRuntimeSet _playerSpawnRuntimeSet;
        public static event Action<Character> OnCharacterDeath;
        public bool Damageable = true;
        [FormerlySerializedAs("defaultHealth")] 
        [SerializeField] private int _defaultHealth = 1;

        int _health;
        private Character _character;
        Rigidbody2D _rb;
        SpriteRenderer _rend;
        RigidbodyConstraints2D _rbConstraints2D;
        
        private void Awake()
        {
            _character = GetComponent<Character>(); 
            _rend = GetComponent<SpriteRenderer>();
            _rb = GetComponent<Rigidbody2D>();
            _rbConstraints2D = _rb.constraints;
            
            Damageable = true;
            _health = _defaultHealth;
        }

        public void TakeDamage(int amount)
        {
            if(!Damageable) return;
            if (_health <= 0) return;
            _health -= amount;
            if (_health <= 0) Die();
        }

        private void Die()
        {
            KillPlayer();
            OnCharacterDeath?.Invoke(_character);
        }

        void KillPlayer()
        {
            _rb.constraints = RigidbodyConstraints2D.FreezeAll;
            _rend.enabled = false;
        }
        

        public override void ResetResettable()
        {
            transform.position = _playerSpawnRuntimeSet.GetItemAtIndex(0).position;
            Damageable = true;
            _health = _defaultHealth;
            _rb.constraints = _rbConstraints2D;
            _rend.enabled = true;
        }
    }
}
