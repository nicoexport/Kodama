using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float movementSpeed = 20f;
    public float airMovementSpeed = 20f;
    public float jumpForce = 30f;
    public float doubleJumpForce = 20f;
    public float longJumpTimer;
    public StandingState standing;
    public JumpingState jumping;
    public FallingState falling;
    public DoubleJumpingState doubleJumping;
    public Rigidbody2D rb;
    public Transform groundCheck;
    public float groundCheckRadius;
    public bool canDoubleJump;
    public bool hasDoubleJumped;
    public bool hasDashed;

    private StateMachine movementSm;
    [SerializeField]
    private LayerMask whatIsGround;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Initializing the MovemenStateMachine and its states
        movementSm = new StateMachine();
        standing = new StandingState(movementSm, this);
        jumping = new JumpingState(movementSm, this);
        falling = new FallingState(movementSm, this);
        doubleJumping = new DoubleJumpingState(movementSm, this);
        movementSm.Initialize(standing);
    }

    public void Update()
    {
        // Update Loop of movementSm
        movementSm.CurrentState.HandleInput();
        movementSm.CurrentState.LogicUpdate();
    }

    public void FixedUpdate()
    {
        // PhysicsUpdate Loop of movementSM
        movementSm.CurrentState.PhysicsUpdate();
    }

    // Method used for moving the character left and right
    public void Move(float horizontalMove, float speed)
    {
        rb.velocity = new Vector2(horizontalMove * speed * Time.fixedDeltaTime * 10f, rb.velocity.y);
    }

    // TO DO: restetting move parameters
    public void ResetMoveParams()
    {
        hasDoubleJumped = false;
        hasDashed = false;
    }

    // Method checking for a collision with ground returning a boolean
    public bool CheckCollisionOverlap(Vector3 point, float radius)
    {
        return Physics2D.OverlapCircle(point, radius, whatIsGround);
    }

    // visualizing the groundCheckRadius
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
