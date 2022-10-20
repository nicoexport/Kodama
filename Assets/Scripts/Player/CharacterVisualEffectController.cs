using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace Player
{
    public class CharacterVisualEffectController : MonoBehaviour
    {
        [SerializeField] private VisualEffect _walkingDust;
        [SerializeField] private VisualEffect _wallDust;

        [SerializeField] private List<VisualEffect> _allEffects;

        private CharacterAnimationController _animController;

        private void Awake()
        {
            _animController = GetComponent<CharacterAnimationController>();
        }

        private void OnEnable()
        {
            _animController.OnAnimationStateChange += HandleAnimationStateChange;
        }

        private void OnDisable()
        {
            _animController.OnAnimationStateChange -= HandleAnimationStateChange;
        }

        private void HandleAnimationStateChange(string currentAnimState, string newAnimState, float speed)
        {
            foreach (var effect in _allEffects) effect.Stop();

            switch (newAnimState)
            {
                case CharacterAnimationController.idle:
                    break;

                case CharacterAnimationController.running:
                    _walkingDust.Reinit();
                    break;

                case CharacterAnimationController.falling:
                    break;

                case CharacterAnimationController.jumping:
                    break;

                case CharacterAnimationController.doubleJumping:
                    break;

                case CharacterAnimationController.wallSliding:
                    _wallDust.Reinit();
                    break;

                case CharacterAnimationController.wallJumping:
                    break;

                case CharacterAnimationController.walkingAgainstWall:
                    break;

                case CharacterAnimationController.spawning:
                    break;
            }
        }
    }
}