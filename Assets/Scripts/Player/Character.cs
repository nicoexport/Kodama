using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterLifeHandler))]
public class Character : MonoBehaviour
{
    [Header("Movement")]
    public float movementSpeed = 20f;
    public float airMovementSpeed = 20f;
    [SerializeField]
    private float maxVelocityX = 23.55f;
    public float jumpForce = 30f;
    public float groundDecelDrag;
    [Range(0f, 0.5f)]
    [SerializeField]
    private float jumpInputTimerMax;
    private float jumpInputTimer;
    [HideInInspector]
    public bool wantjump;

    [HideInInspector]
    public bool hasPressedRight;
    [HideInInspector]
    public bool hasPressedLeft;

    [Range(0f, 0.5f)]
    public float horizontalInputTimer = 0.15f;
    [HideInInspector]
    public float hasPressedRightTimer;
    [HideInInspector]
    public float hasPressedLeftTimer;


    [Header("Longjumping")]
    [Range(0f, 10f)]
    public float longJumpMultiplier = 4f;
    public float longJumpTimer;



    [Header("Walljumping")]
    [Range(0f, 1f)]
    public float wallSlideInputThresh = 0.75f;
    public float horizontalWallJumpForce;
    public float verticalWallJumpForce;
    public float wallJumpTimer;

    [Header("Gravity Values")]
    public float normalGravity = 5f;
    public float fastFallGravity = 8f;
    public float wallslidingGravity = 2f;


    [Header("Collision Checks")]
    public float groundCheckRadius;
    public float ceilingCheckRadius;
    public float frontCheckRadius = 0.23f;
    [SerializeField]
    private LayerMask whatIsGround;



    [Header("Refrences")]
    public Transform groundCheck;
    public Transform frontCheck;
    public Transform ceilingCheck;
    public Transform ceilingCheck1;
    public Rigidbody2D rb;

    [Header("Debugging")]
    [SerializeField]
    public bool debugStates;
    [SerializeField]
    private bool logVelocity;


    [HideInInspector]
    public bool facingRight = true;

    public StandingState standing;
    public RunningState running;
    public JumpingState jumping;
    public FallingState falling;
    public WallslidingState wallsliding;
    public WalljumpingState wallJumping;


    private StateMachine movementSm;
    private CharacterAnimationController cAnimController;
    public PlayerInputActions playerInputActions { get; private set; }
    public CharacterLifeHandler LifeHandler { get; private set; }



    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cAnimController = GetComponent<CharacterAnimationController>();
        LifeHandler = GetComponent<CharacterLifeHandler>();
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Jump.started += StartJumpInputTimer;
        InitializeStates();
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
        CountDownInputTimer();
    }

    // Method for flipping character
    public void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    private void StartJumpInputTimer(InputAction.CallbackContext context)
    {
        wantjump = true;
        jumpInputTimer = jumpInputTimerMax;
    }

    // Method used for moving the character left and right
    public void Move(float horizontalMove, float speed)
    {
        // rb.velocity = new Vector2(horizontalMove * speed * Time.deltaTime * 10f, rb.velocity.y);
        var newForce = new Vector2(horizontalMove * Time.deltaTime * speed, 0f);
        rb.AddForce(newForce, ForceMode2D.Impulse);
        if (rb.velocity.x > maxVelocityX) rb.velocity = new Vector2(maxVelocityX, rb.velocity.y);
        else if (rb.velocity.x < -maxVelocityX) rb.velocity = new Vector2(-maxVelocityX, rb.velocity.y);
        if (logVelocity) Debug.Log("Velocity x: " + rb.velocity.x + " y: " + rb.velocity.y);
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
        rb.gravityScale = normalGravity;
    }

    // Method checking for a collision with ground returning a boolean
    public bool CheckCollisionOverlap(Vector3 point, float radius)
    {
        return Physics2D.OverlapCircle(point, radius, whatIsGround);
    }

    public void UpdateVisuals()
    {
        var touchingWall = CheckCollisionOverlap(frontCheck.position, frontCheckRadius);
        cAnimController.SetAnimationeState(movementSm.CurrentState, playerInputActions.Player.Movement.ReadValue<Vector2>().x, rb.velocity.x, maxVelocityX, touchingWall);
    }

    private void InitializeStates()
    {
        movementSm = new StateMachine();
        standing = new StandingState(movementSm, this);
        running = new RunningState(movementSm, this);
        jumping = new JumpingState(movementSm, this);
        falling = new FallingState(movementSm, this);
        wallsliding = new WallslidingState(movementSm, this);
        wallJumping = new WalljumpingState(movementSm, this);
        movementSm.Initialize(standing);
    }

    private void CountDownInputTimer()
    {
        if (jumpInputTimer > 0f) jumpInputTimer -= Time.fixedDeltaTime;
        if (jumpInputTimer <= 0f) wantjump = false;
        if (hasPressedLeftTimer > 0f) hasPressedLeftTimer -= Time.fixedDeltaTime;
        if (hasPressedLeftTimer <= 0f) hasPressedLeft = false;
        if (hasPressedRightTimer > 0f) hasPressedRightTimer -= Time.fixedDeltaTime;
        if (hasPressedRightTimer <= 0f) hasPressedRight = false;

    }

    public float GetMaxVelocityX()
    {
        return maxVelocityX;
    }

    // visualizing the groundCheckRadius
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(frontCheck.position, frontCheckRadius);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(ceilingCheck.position, ceilingCheckRadius);
        Gizmos.DrawWireSphere(ceilingCheck1.position, ceilingCheckRadius);
    }
}
