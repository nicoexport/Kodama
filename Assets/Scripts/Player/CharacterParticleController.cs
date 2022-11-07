using UnityEngine;

namespace Kodama.Player {
    public class CharacterParticleController : MonoBehaviour {
        [SerializeField] private ParticleSystem _walkingDust;
        [SerializeField] private ParticleSystem _wallDust;
        [SerializeField] private ParticleSystem _landDust;
        [SerializeField] private ParticleSystem _leafGlimmer;

        private CharacterAnimationController _animController;

        private void Awake() => _animController = GetComponent<CharacterAnimationController>();

        private void OnEnable() => _animController.onOnAnimationStateChange += HandleAnimationStateChange;

        private void OnDisable() => _animController.onOnAnimationStateChange -= HandleAnimationStateChange;

        private void HandleAnimationStateChange(string currentAnimState, string newAnimState, float speed) {
            _leafGlimmer.Stop();

            switch (currentAnimState) {
                case CharacterAnimationController.IDLE:
                    break;

                case CharacterAnimationController.RUNNING:
                    _walkingDust.Stop();
                    break;

                case CharacterAnimationController.FALLING:
                    break;

                case CharacterAnimationController.JUMPING:
                    break;

                case CharacterAnimationController.DOUBLE_JUMPING:
                    break;

                case CharacterAnimationController.WALL_SLIDING:
                    _wallDust.Stop();
                    break;

                case CharacterAnimationController.WALL_JUMPING:
                    break;

                case CharacterAnimationController.WALKING_AGAINST_WALL:
                    break;

                case CharacterAnimationController.SPAWNING:
                    break;

                case CharacterAnimationController.WINNING:
                    break;
            }

            switch (newAnimState) {
                case CharacterAnimationController.LANDING:
                    _landDust.Stop();
                    _landDust.Play();
                    break;
                case CharacterAnimationController.IDLE:
                    if (currentAnimState is CharacterAnimationController.FALLING
                        or CharacterAnimationController.FALLING_TRANSITION) {
                        _landDust.Stop();
                        _landDust.Play();
                    }

                    break;

                case CharacterAnimationController.RUNNING:
                    _walkingDust.Play();
                    _leafGlimmer.Play();
                    break;

                case CharacterAnimationController.FALLING:
                    _leafGlimmer.Play();
                    break;

                case CharacterAnimationController.JUMPING:
                    _leafGlimmer.Play();
                    break;

                case CharacterAnimationController.DOUBLE_JUMPING:
                    break;

                case CharacterAnimationController.WALL_SLIDING:
                    _wallDust.Play();
                    break;

                case CharacterAnimationController.WALL_JUMPING:
                    _leafGlimmer.Play();
                    break;

                case CharacterAnimationController.WALKING_AGAINST_WALL:
                    break;

                case CharacterAnimationController.SPAWNING:
                    break;
            }
        }
    }
}