using Kodama.Audio;
using Kodama.Scriptable.Channels;
using UnityEngine;

namespace Kodama.Player {
    public class CharacterAudioController : MonoBehaviour {
        [Header("Channels")] [SerializeField] private VoidEventChannelSO _onPlayerHurtChannel;

        [SerializeField] private VoidEventChannelSO _onPlayerDeathEventChannel;

        [Header("Audio Cues")] [SerializeField]
        private AudioCue _jump;

        [SerializeField] private AudioCue _land;
        [SerializeField] private AudioCue _step;
        [SerializeField] private AudioCue _hurt;
        [SerializeField] private AudioCue _death;
        [SerializeField] private AnimationCurve _stepSpeedCurve;

        [Range(0f, 1f)] [SerializeField] private float _stepTimer = 0.5f;

        private CharacterAnimationController _animationController;
        private AudioCue _audioCue;
        private bool _canStep = true;
        private Character _character;
        private bool _running;
        private float _speed = 1f;
        private float _stepTimerCurrent;

        protected void Awake() => _animationController = GetComponent<CharacterAnimationController>();

        protected void Update() => _speed = _animationController.speed;

        protected void FixedUpdate() {
            if (_running && _canStep) {
                _step.PlayAudioCue();
                _canStep = false;
                _stepTimerCurrent = _stepTimer;
            }

            if (!_canStep) {
                if (_stepTimerCurrent <= 0f) {
                    _canStep = true;
                }

                _stepTimerCurrent -= Time.deltaTime * _stepSpeedCurve.Evaluate(_speed);
            }
        }

        protected void OnEnable() {
            _animationController.onOnAnimationStateChange += HandleStateChange;
            _onPlayerDeathEventChannel.OnEventRaised += PlayDeathAudio;
            _onPlayerHurtChannel.OnEventRaised += PlayHurtAudio;
        }

        protected void OnDisable() {
            _animationController.onOnAnimationStateChange -= HandleStateChange;
            _onPlayerDeathEventChannel.OnEventRaised -= PlayDeathAudio;
            _onPlayerHurtChannel.OnEventRaised -= PlayHurtAudio;
        }

        private void HandleStateChange(string currentState, string newState, float speed) {
            _running = false;
            switch (newState) {
                case CharacterAnimationController.IDLE:
                    if (currentState is CharacterAnimationController.FALLING
                        or CharacterAnimationController.FALLING_TRANSITION) {
                        _land.PlayAudioCue();
                    }

                    break;
                case CharacterAnimationController.DYING:
                    break;
                case CharacterAnimationController.FALLING:
                    break;
                case CharacterAnimationController.JUMPING:
                    _jump.PlayAudioCue();
                    break;
                case CharacterAnimationController.LANDING:
                    _land.PlayAudioCue();
                    break;
                case CharacterAnimationController.RUNNING:
                    _running = true;
                    _canStep = true;
                    break;
                case CharacterAnimationController.SPAWNING:
                    break;
                case CharacterAnimationController.WINNING:
                    break;
                case CharacterAnimationController.DOUBLE_JUMPING:
                    break;
                case CharacterAnimationController.FALLING_TRANSITION:
                    break;
                case CharacterAnimationController.WALL_JUMPING:
                    _jump.PlayAudioCue();
                    break;
                case CharacterAnimationController.WALL_SLIDING:
                    _land.PlayAudioCue();
                    break;
                case CharacterAnimationController.WALKING_AGAINST_WALL:
                    break;
            }
        }

        private void PlayDeathAudio() => _death.PlayAudioCue();

        private void PlayHurtAudio() => _hurt.PlayAudioCue();
    }
}