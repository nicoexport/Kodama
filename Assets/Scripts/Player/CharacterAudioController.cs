using System;
using Audio;
using Player.MovementStates;
using Scriptable;
using UnityEngine;
using Utility;

namespace Player
{
   public class CharacterAudioController : MonoBehaviour
   {
      
      [Header("Audio Cues")] 
      [SerializeField] private AudioCue _jump;
      [SerializeField] private AudioCue _land;
      [SerializeField] private AudioCue _step;
      [Range(0f, 3f)] [SerializeField] private float _stepDelay = 1f;
      [SerializeField] private AnimationCurve _stepSpeedCurve;
      private AudioCue _audioCue;
      private Character _character;
      private CharacterAnimationController _animationController;
      private bool _running = false;
      private float _speed = 1f;
      private bool _canStep = true;

      protected void Awake()
      {
         _animationController = GetComponent<CharacterAnimationController>();
         _animationController.OnAnimationStateChange += HandleStateChange;
      }

      protected void Update()
      {
         _speed = _animationController.Speed;
      }

      protected void FixedUpdate()
      {
         if (_running && _canStep)
         {
            _canStep = false;
            _step.PlayAudioCue();
            StartCoroutine(Utilities.ActionAfterDelayEnumerator(_stepSpeedCurve.Evaluate(_speed), () => { _canStep = 
            true; }));
         }
                                          
                                       
      }

      private void HandleStateChange(string currentState, string newState, float speed)
      {
         _running = false;
         switch (newState)
         {
            case CharacterAnimationController.idle:
               if(currentState is CharacterAnimationController.falling or CharacterAnimationController.fallingTransition)
                  _land.PlayAudioCue();
               break;
            case CharacterAnimationController.dying:
               break;
            case CharacterAnimationController.falling:
               break;
            case CharacterAnimationController.jumping:
               _jump.PlayAudioCue();
               break; 
            case CharacterAnimationController.landing:
               _land.PlayAudioCue();
               break;
            case CharacterAnimationController.running:
               _running = true;
               break;
            case CharacterAnimationController.spawning:
               break;
            case CharacterAnimationController.winning:
               break;
            case CharacterAnimationController.doubleJumping:
               break;
            case CharacterAnimationController.fallingTransition:
               break;
            case CharacterAnimationController.wallJumping:
               _jump.PlayAudioCue();
               break;
            case CharacterAnimationController.wallSliding:
               _land.PlayAudioCue();
               break;
            case CharacterAnimationController.walkingAgainstWall:
               break;
         }
      }
   }
}
