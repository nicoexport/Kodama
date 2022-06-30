using UnityEngine;

namespace Player
{
    public class CharacterParticleController : MonoBehaviour
    {
        [SerializeField] ParticleSystem _walkingDust;
        [SerializeField] ParticleSystem _wallDust;
        [SerializeField] ParticleSystem _landDust;
        [SerializeField] ParticleSystem _leafGlimmer;

        CharacterAnimationController _animController;

        void Awake()
        {
            _animController = GetComponent<CharacterAnimationController>();
        }

        void OnEnable()
        {
            _animController.OnAnimationStateChange += HandleAnimationStateChange;
        }

        void OnDisable()
        {
            _animController.OnAnimationStateChange -= HandleAnimationStateChange;
        }

        void HandleAnimationStateChange(string currentAnimState, string newAnimState)
        {
            _leafGlimmer.Stop();

            switch (currentAnimState)
            {
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

            switch (newAnimState)
            {
                case CharacterAnimationController.idle:
                    if (currentAnimState == CharacterAnimationController.falling)
                    {
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