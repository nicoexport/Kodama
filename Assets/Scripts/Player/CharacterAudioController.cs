using Audio;
using Scriptable;
using Scriptable.Channels;
using UnityEngine;


namespace Player
{
   public class CharacterAudioController : MonoBehaviour
   {
      [Header("Channels")] 
      [SerializeField] private VoidEventChannelSO _onPlayerHurtChannel;
      [SerializeField] private VoidEventChannelSO _onPlayerDeathEventChannel;
      
      [Header("Audio Cues")] 
      [SerializeField] private AudioCue _jump;
      [SerializeField] private AudioCue _land;
      [SerializeField] private AudioCue _step;
      [SerializeField] private AudioCue _hurt;
      [SerializeField] private AudioCue _death;
      [SerializeField] private AnimationCurve _stepSpeedCurve;
      private AudioCue _audioCue;
      private Character _character;
      private CharacterAnimationController _animationController;
      private bool _running = false;
      private float _speed = 1f;
      private bool _canStep = true;

      [Range(0f, 1f)]
      [SerializeField]
      private float _stepTimer = 0.5f;
      private float _stepTimerCurrent;

      protected void Awake()
      {
         _animationController = GetComponent<CharacterAnimationController>();
      }

      protected void OnEnable()
      {
         _animationController.OnAnimationStateChange += HandleStateChange;
         _onPlayerDeathEventChannel.OnEventRaised += PlayDeathAudio;
         _onPlayerHurtChannel.OnEventRaised += PlayHurtAudio;
      }

      protected void OnDisable()
      {
         _animationController.OnAnimationStateChange -= HandleStateChange;
         _onPlayerDeathEventChannel.OnEventRaised -= PlayDeathAudio;
         _onPlayerHurtChannel.OnEventRaised -= PlayHurtAudio;
      }

      protected void Update()
      {
         _speed = _animationController.Speed;
      }

      protected void FixedUpdate()
      {
         if (_running && _canStep)
         {
            _step.PlayAudioCue();
            _canStep = false;
            _stepTimerCurrent = _stepTimer;
         }
         if (!_canStep)
         {
            if (_stepTimerCurrent <= 0f)
            {
               _canStep = true;
            }
            _stepTimerCurrent -= Time.deltaTime * _stepSpeedCurve.Evaluate(_speed);
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
               _canStep = true;
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

      private void PlayDeathAudio()
      {
         _death.PlayAudioCue();
      }

      private void PlayHurtAudio()
      {
         _hurt.PlayAudioCue();
      }
   }
}
