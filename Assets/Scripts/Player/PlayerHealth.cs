using Architecture;
using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    public class PlayerHealth : MonoBehaviour
    {
        [SerializeField] private int _defaultHealth = 1;
        [SerializeField] private UnityEvent OnTakeDamage;
        [SerializeField] private UnityEvent OnDie;

        public bool Damageable = true;
        private int _health;

        private void Awake()
        {
            Reset();
        }

        public void TakeDamage(int amount)
        {
            if (!Damageable) return;
            if (_health <= 0) return;
            _health -= amount;
            if (_health <= 0)
            {
                Die();
            }
            else
            {
                OnTakeDamage?.Invoke();
            }
        }

        public void Die()
        {
            OnDie.Invoke();
            Destroy(gameObject);
            LevelManager.Instance.RestartLevel();
        }

        public void Reset()
        {
            Damageable = true;
            _health = _defaultHealth;
        }
    }
}