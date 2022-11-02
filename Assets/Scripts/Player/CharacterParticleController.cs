using UnityEngine;

namespace Player {
    public class CharacterParticleController : MonoBehaviour {
        [SerializeField] private ParticleSystem _walkingDust;
        [SerializeField] private ParticleSystem _wallDust;
        [SerializeField] private ParticleSystem _landDust;
        [SerializeField] private ParticleSystem _leafGlimmer;

        private CharacterAnimationController _animController;

        private void Awake() => _animController = GetComponent<CharacterAnimationController>();

        private void OnEnable() => _animController.OnAnimationStateChange += HandleAnimationStateChange;

        private void OnDisable() => _animController.OnAnimationStateChange -= HandleAnimationStateChange;

        private void HandleAnimationStateChange(string currentAnimState, string newAnimState, float speed) {
            _leafGlimmer.Stop();

            switch (currentAnimState) {
                case CharacterAnimationController.idle:
                    break;

                case CharacterAnimationController.running:
                    _walkingDust.Stop();
                    break;

                case CharacterAnimationController.falling:
                    break;

                case CharacterAnimationController.jumping:
                    break;

                case CharacterAnimationController.doubleJumping:
                    break;

                case CharacterAnimationController.wallSliding:
                    _wallDust.Stop();
                    break;

                case CharacterAnimationController.wallJumping:
                    break;

                case CharacterAnimationController.walkingAgainstWall:
                    break;

                case CharacterAnimationController.spawning:
                    break;

                case CharacterAnimationController.winning:
                    break;
            }

            switch (newAnimState) {
                case CharacterAnimationController.landing:
                    _landDust.Stop();
                    _landDust.Play();
                    break;
                case CharacterAnimationController.idle:
                    if (currentAnimState is CharacterAnimationController.falling
                        or CharacterAnimationController.fallingTransition) {
                        _landDust.Stop();
                        _landDust.Play();
                    }

                    break;

                case CharacterAnimationController.running:
                    _walkingDust.Play();
                    _leafGlimmer.Play();
                    break;

                case CharacterAnimationController.falling:
                    _leafGlimmer.Play();
                    break;

                case CharacterAnimationController.jumping:
                    _leafGlimmer.Play();
                    break;

                case CharacterAnimationController.doubleJumping:
                    break;

                case CharacterAnimationController.wallSliding:
                    _wallDust.Play();
                    break;

                case CharacterAnimationController.wallJumping:
                    _leafGlimmer.Play();
                    break;

                case CharacterAnimationController.walkingAgainstWall:
                    break;

                case CharacterAnimationController.spawning:
                    break;
            }
        }
    }
}