using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Utility;

[RequireComponent(typeof(CharacterLifeHandler))]
public class Character : MonoBehaviour
{
    [SerializeField]
    private CharacterMovementValues defaultMovementValues;
    [SerializeField]
    private CharacterRuntimeSet characterRuntimeSet;

    public float movementSpeed { get; private set; }
    public float airMovementSpeed { get; private set; }
    public float maxVelocityX { get; private set; }
    public float jumpForce { get; private set; }
    public float groundDecelDrag { get; private set; }
    private float jumpInputTimerMax;
    private float jumpInputTimer;

    public float horizontalInputTimer { get; private set; }
    [HideInInspector]
    public float hasPressedRightTimer;
    [HideInInspector]
    public float hasPressedLeftTimer;

    public float longJumpMultiplier { get; private set; }
    public float longJumpTimer { get; private set; }

    public float wallSlideInputThresh { get; private set; }
    public float horizontalWallJumpForce { get; private set; }
    public float verticalWallJumpForce { get; private set; }
    public float wallJumpTimer { get; private set; }

    public float normalGravity { get; private set; }
    public float fastFallGravity { get; private set; }
    public float wallslidingGravity { get; private set; }


    public float spawnDelay = 1f;

    [HideInInspector]
    public bool wantjump;
    [HideInInspector]
    public bool hasPressedRight;
    [HideInInspector]
    public bool hasPressedLeft;

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
    public SpawningState spawning;
    public WinningState winning;
    public DyingState dying;


    private StateMachine movementSm;
    private CharacterAnimationController cAnimController;
    public CharacterLifeHandler LifeHandler { get; private set; }

    private void Awake()
    {
        ReadMovementValues(defaultMovementValues);
        rb = GetComponent<Rigidbody2D>();
        cAnimController = GetComponent<CharacterAnimationController>();
        LifeHandler = GetComponent<CharacterLifeHandler>();
        InputManager.playerInputActions.Player.Jump.started += StartJumpInputTimer;
        AddCharacterToRuntimeSet();
        InitializeStates();
        InitializeStateMachine(spawning);
        StartCoroutine(Utilities.ActionAfterDelayEnumerator(spawnDelay, () => { movementSm.ChangeState(standing); }));
    }

    private void OnEnable()
    {
        AddCharacterToRuntimeSet();
        LevelManager.OnCompleteLevel += () => { movementSm.ChangeState(winning); };
    }

    private void OnDisable()
    {
        //movementSm.CurrentState.Exit();
        characterRuntimeSet.RemoveFromList(this);
        LevelManager.OnCompleteLevel -= () => { movementSm.ChangeState(winning); };
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

    public IEnumerator DieEnumerator()
    {
        movementSm.ChangeState(dying);
        UpdateVisuals();
        var waitForAnim = new WaitForSeconds(cAnimController.animator.GetCurrentAnimatorClipInfo(0).Length);
        yield return waitForAnim;
    }
    
    public void ResetMoveParams()
    {
        rb.gravityScale = normalGravity;
    }

    public State GetState()
    {
        return movementSm.CurrentState;
    }
    // Method checking for a collision with ground returning a boolean
    public bool CheckCollisionOverlap(Vector3 point, float radius)
    {
        return Physics2D.OverlapCircle(point, radius, whatIsGround);
    }

    private void UpdateVisuals()
    {
        var touchingWall = CheckCollisionOverlap(frontCheck.position, frontCheckRadius);
        cAnimController.SetAnimationState(movementSm.CurrentState, InputManager.playerInputActions.Player.Movement.ReadValue<Vector2>().x, rb.velocity.x, maxVelocityX, touchingWall);
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
        spawning = new SpawningState(movementSm, this);
        winning = new WinningState(movementSm, this);
        dying = new DyingState(movementSm, this);
    }

    private void InitializeStateMachine(State state)
    {
        movementSm.Initialize(state);
    }

    private void ReadMovementValues(CharacterMovementValues values)
    {
        movementSpeed = values.moveSpeed;
        airMovementSpeed = values.airMoveSpeed;
        maxVelocityX = values.maxVelocityX;
        groundDecelDrag = values.groundDecelDrag;
        jumpForce = values.jumpForce;
        jumpInputTimerMax = values.jumpInputTimerMax;
        horizontalInputTimer = values.horizontalInputTimer;
        longJumpMultiplier = values.longJumpMultiplier;
        longJumpTimer = values.longJumpTimer;
        wallSlideInputThresh = values.wallSlideInputThresh;
        horizontalWallJumpForce = values.horizontalWallJumpForce;
        verticalWallJumpForce = values.verticalWallJumpForce;
        wallJumpTimer = values.wallJumpTimer;
        normalGravity = values.normalGravity;
        fastFallGravity = values.fastFallGravity;
        wallslidingGravity = values.wallslidingGravity;
        Debug.Log(longJumpTimer);
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
    
    private void AddCharacterToRuntimeSet()
    {
        if (characterRuntimeSet.IsEmpty())
        {
            characterRuntimeSet.AddToList(this);
        }
        else
        {
            Debug.Log("Player already exists");
        }
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
