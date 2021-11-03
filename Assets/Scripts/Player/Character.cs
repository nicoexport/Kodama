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

    [Range(0f, 1f)]
    public float wallSlideInputThresh = 0.75f;
    public float horizontalWallJumpForce;
    public float verticalWallJumpForce;
    public float wallJumpTimer;

    public float normalGravity = 5f;
    public float fastFallGravity = 8f;
    public float wallslidingGravity = 2f;

    public StandingState standing;
    public JumpingState jumping;
    public FallingState falling;
    public DoubleJumpingState doubleJumping;
    public WallslidingState wallsliding;
    public WalljumpingState wallJumping;
    public Rigidbody2D rb;
    public float groundCheckRadius;
    public float ceilingCheckRadius;

    public bool canDoubleJump;
    public bool hasDoubleJumped;
    public bool hasDashed;

    public bool facingRight = true;

    public Transform groundCheck;
    public Transform frontCheck;
    public Transform ceilingCheck;
    public Transform ceilingCheck1;

    public bool debugStates;

    private StateMachine movementSm;
    private CharacterAnimationController cAnimController;
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
        wallsliding = new WallslidingState(movementSm, this);
        wallJumping = new WalljumpingState(movementSm, this);
        doubleJumping = new DoubleJumpingState(movementSm, this);
        movementSm.Initialize(standing);
        cAnimController = GetComponent<CharacterAnimationController>();
    }

    public void Update()
    {
        // Update Loop of movementSm
        movementSm.CurrentState.HandleInput();
        movementSm.CurrentState.LogicUpdate();
        UpdateVisuals();
    }

    public void FixedUpdate()
    {
        // PhysicsUpdate Loop of movementSM
        movementSm.CurrentState.PhysicsUpdate();
    }

    // Method for flipping character
    public void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    // Method used for moving the character left and right
    public void Move(float horizontalMove, float speed)
    {
        // rb.velocity = new Vector2(horizontalMove * speed * Time.deltaTime * 10f, rb.velocity.y);
        var newForce = new Vector2(horizontalMove * Time.deltaTime * speed, 0f);
        rb.AddForce(newForce, ForceMode2D.Impulse);
    }

    public void Jump(float jumpForce)
    {
        transform.Translate(Vector2.up * (groundCheckRadius + 0.1f));
        var newForce = new Vector2(0f, jumpForce * Time.deltaTime);
        rb.AddForce(newForce, ForceMode2D.Impulse);
    }

    // TO DO: restetting move parameters
    public void ResetMoveParams()
    {
        hasDoubleJumped = false;
        hasDashed = false;
        rb.gravityScale = normalGravity;
    }

    // Method checking for a collision with ground returning a boolean
    public bool CheckCollisionOverlap(Vector3 point, float radius)
    {
        return Physics2D.OverlapCircle(point, radius, whatIsGround);
    }

    public void UpdateVisuals()
    {
        cAnimController.SetAnimationeState(movementSm.CurrentState, rb.velocity.x);
    }

    // visualizing the groundCheckRadius
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(frontCheck.position, groundCheckRadius);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(ceilingCheck.position, ceilingCheckRadius);
        Gizmos.DrawWireSphere(ceilingCheck1.position, ceilingCheckRadius);
    }
}
