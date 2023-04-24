using System;
using Kodama.Data;
using Kodama.Scriptable.Channels;
using Kodama.Utility;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Kodama.Player {
    public class PlayerHealth : MonoBehaviour {
        [SerializeField] private int _defaultHealth = 1;
        [SerializeField] private float invuDuration = 1.5f;
        [SerializeField] private VoidEventChannelSO onPlayerHurtChannel;
        [SerializeField] private VoidEventChannelSO onPlayerDeathEventChannel;
        [SerializeField] private LevelDataEventChannelSO onLevelCompleteChannel;
        [SerializeField] private VoidEventChannelSO onLoseShield;
        [SerializeField] private VoidEventChannelSO onGainShield;
        [SerializeField] private ShieldVisuals shieldVisuals;

        public bool Damageable = true;
        private int _health;
        private bool hasShield;
        private float invuTimer;
        private bool levelComplete;

        private void OnEnable() {
            onLevelCompleteChannel.OnEventRaised += LevelComplete;
        }


        private void OnDisable() {
            onLevelCompleteChannel.OnEventRaised -= LevelComplete;
        }

        private void Awake() => Reset();

        private void Update() {
            if (Keyboard.current.lKey.wasPressedThisFrame) {
                GainShield();
            }

            if (invuTimer > 0f && !hasShield) {
                invuTimer -= Time.deltaTime;
            } else if(!Damageable && !hasShield && !levelComplete) {
                Damageable = true;
                Debug.Log("Damageable");
            }
        }

        public void Reset() {
            Damageable = true;
            _health = _defaultHealth;
            shieldVisuals.Disable();
        }
        private void LevelComplete(LevelData obj) {
            levelComplete = true;
            Damageable = false;
            Debug.Log(this.name + "LevelComplete");
        }

        public void TakeDamage(int amount) {
            if (hasShield) {
                LoseShield();
            }
            
            if (!Damageable) {
                return;
            }

            if (_health <= 0) {
                return;
            }

            

            _health -= amount;
            if (_health <= 0) {
                Die();
            } else {
                onPlayerHurtChannel.RaiseEvent();
            }
        }

        private void LoseShield() {
            Debug.Log("Lose Shield");
            hasShield = false;
            onLoseShield.RaiseEvent();
            shieldVisuals.Disable();
        }

        public bool GainShield() {
            if (hasShield) {
                return false;
            }
            hasShield = true;
            Damageable = false;
            invuTimer = invuDuration;
            shieldVisuals.Enable();
            onGainShield.RaiseEvent();
            return true;
        }

        public void Die() => onPlayerDeathEventChannel.RaiseEvent();
    }
}