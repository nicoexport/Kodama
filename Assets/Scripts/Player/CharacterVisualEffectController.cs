using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace Kodama.Player {
    public class CharacterVisualEffectController : MonoBehaviour {
        [SerializeField] private VisualEffect _walkingDust;
        [SerializeField] private VisualEffect _wallDust;

        [SerializeField] private List<VisualEffect> _allEffects;

        private CharacterAnimationController _animController;

        private void Awake() => _animController = GetComponent<CharacterAnimationController>();

        private void OnEnable() => _animController.onOnAnimationStateChange += HandleAnimationStateChange;

        private void OnDisable() => _animController.onOnAnimationStateChange -= HandleAnimationStateChange;

        private void HandleAnimationStateChange(string currentAnimState, string newAnimState, float speed) {
            foreach (var effect in _allEffects) {
                effect.Stop();
            }

            switch (newAnimState) {
                case CharacterAnimationController.IDLE:
                    break;

                case CharacterAnimationController.RUNNING:
                    _walkingDust.Reinit();
                    break;

                case CharacterAnimationController.FALLING:
                    break;

                case CharacterAnimationController.JUMPING:
                    break;

                case CharacterAnimationController.DOUBLE_JUMPING:
                    break;

                case CharacterAnimationController.WALL_SLIDING:
                    _wallDust.Reinit();
                    break;

                case CharacterAnimationController.WALL_JUMPING:
                    break;

                case CharacterAnimationController.WALKING_AGAINST_WALL:
                    break;

                case CharacterAnimationController.SPAWNING:
                    break;
            }
        }
    }
}