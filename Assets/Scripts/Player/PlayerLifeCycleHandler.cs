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
        public bool Damageable = true;

        [FormerlySerializedAs("defaultHealth")] [SerializeField]
        int _defaultHealth = 1;

        Character _character;

        int _health;
        Rigidbody2D _rb;
        RigidbodyConstraints2D _rbConstraints2D;
        SpriteRenderer _rend;

        void Awake()
        {
            _character = GetComponent<Character>();
            _rend = GetComponent<SpriteRenderer>();
            _rb = GetComponent<Rigidbody2D>();
            _rbConstraints2D = _rb.constraints;

            Damageable = true;
            _health = _defaultHealth;
        }

        public static event Action<Character> OnCharacterDeath;

        public void TakeDamage(int amount)
        {
            if (!Damageable) return;
            if (_health <= 0) return;
            _health -= amount;
            if (_health <= 0) Die();
        }

        public void Die()
        {
            KillPlayer();
            OnCharacterDeath?.Invoke(_character);
        }

        void KillPlayer()
        {
            _rb.constraints = RigidbodyConstraints2D.FreezeAll;
            _rend.enabled = false;
        }

        public override void OnLevelReset()
        {
            transform.position = _playerSpawnRuntimeSet.GetItemAtIndex(0).position;
            Damageable = true;
            _health = _defaultHealth;
            _rb.constraints = _rbConstraints2D;
            _rend.enabled = true;
        }
    }
}