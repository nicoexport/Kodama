using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    [CreateAssetMenu(menuName = "Character/CharacterMovementValues")]
    public class CharacterMovementValues : ScriptableObject
    {
        [Header("Moving")] public float moveSpeed = 250f;

        public float sprintSpeed = 300f;
        public float airMoveSpeed = 250f;
        public float airSprintSpeed = 300f;
        public float maxVelocityX = 25f;
        public float groundDecelDrag = 15f;

        [Header("Jumping")] public float jumpForce = 50f;

        [FormerlySerializedAs("jumpInputTimerMax")] [Range(0f, 0.5f)]
        public float jumpInputTimer = 0.1f;

        public float horizontalInputTimer = 0.3f;
        public float longJumpMultiplier = 4f;
        public float longJumpTimer = 0.35f;
        public float hangTime = 0.15f;

        [Header("WallJumping")] [Range(0f, 1f)]
        public float wallSlideInputThresh;

        public float horizontalWallJumpForce = 50f;
        public float verticalWallJumpForce = 90f;
        public float wallJumpTimer = 0.5f;
        public float wallLongJumpMultiplier = 5f;

        [Header("Gravity")] public float normalGravity = 7f;

        public float fastFallGravity = 10f;
        public float wallslidingGravity = 2f;
    }
}