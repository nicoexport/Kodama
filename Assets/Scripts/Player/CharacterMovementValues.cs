using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character/CharacterMovementValues")]
public class CharacterMovementValues : ScriptableObject
{
    [Header("Moving")]
    public float moveSpeed = 250f;
    public float airMoveSpeed = 250f;
    public float maxVelocityX = 25f;
    public float groundDecelDrag = 15f;

    [Header("Jumping")]
    public float jumpForce = 50f;
    [Range(0f, 0.5f)]
    public float jumpInputTimerMax = 0.1f;
    public float horizontalInputTimer = 0.3f;
    public float longJumpMultiplier = 4f;
    public float longJumpTimer = 0.35f;

    [Header("Walljumping")]
    [Range(0f, 1f)]
    public float wallSlideInputThresh = 0f;
    public float horizontalWallJumpForce = 50f;
    public float verticalWallJumpForce = 90f;
    public float wallJumpTimer = 0.5f;

    [Header("Gravity")]
    public float normalGravity = 7f;
    public float fastFallGravity = 10f;
    public float wallslidingGravity = 2f;
}
