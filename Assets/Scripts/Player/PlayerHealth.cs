using Scriptable;
using UnityEngine;

namespace Player
{
    public class PlayerHealth : MonoBehaviour
    {
        [SerializeField] private int _defaultHealth = 1;
        [SerializeField] private VoidEventChannelSO onPlayerHurtChannel;
        [SerializeField] private VoidEventChannelSO onPlayerDeathEventChannel;

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
                onPlayerHurtChannel.RaiseEvent();
            }
        }

        public void Die()
        {
            onPlayerDeathEventChannel.RaiseEvent();
        }

        public void Reset()
        {
            Damageable = true;
            _health = _defaultHealth;
        }
    }
}