using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterParticleController : MonoBehaviour
{
    [SerializeField] private ParticleSystem _walkingDust;
    [SerializeField] private ParticleSystem _wallDust;

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

    private void HandleAnimationStateChange(string currentAnimState, string newAnimState)
    {
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
        }

        switch (newAnimState)
        {
            case CharacterAnimationController.idle:
                break;

            case CharacterAnimationController.running:
                _walkingDust.Play();
                break;

            case CharacterAnimationController.falling:
                break;

            case CharacterAnimationController.jumping:
                break;

            case CharacterAnimationController.doubleJumping:
                break;

            case CharacterAnimationController.wallSliding:
                _wallDust.Play();
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