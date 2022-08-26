using System;
using Audio;
using Player.MovementStates;
using Scriptable;
using UnityEngine;

namespace Player
{
   public class CharacterAudioController : MonoBehaviour
   {
      
      [Header("Audio Cues")] 
      [SerializeField] private AudioCue _jump;
      [SerializeField] private AudioCue _land;
         
      private AudioCue _audioCue;
      private Character _character;
      private CharacterAnimationController _animationController;

      protected void Awake()
      {
         _animationController = GetComponent<CharacterAnimationController>();
         _animationController.OnAnimationStateChange += HandleStateChange;
      }

      private void HandleStateChange(string currentState, string newState)
      {
         
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
