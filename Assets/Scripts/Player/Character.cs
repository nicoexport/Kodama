using System;
using Architecture;
using Data;
using GameManagement;
using Player.MovementStates;
using UnityEngine;
using UnityEngine.InputSystem;
using Utility;

namespace Player {
    [RequireComponent(typeof(PlayerHealth))]
    public class Character : MonoBehaviour {
        [SerializeField] private CharacterRuntimeSet characterRuntimeSet;

        [HideInInspector] public float hasPressedRightTimer;

        [HideInInspector] public float hasPressedLeftTimer;


        public float spawnDelay = 1f;

        [HideInInspector] public bool wantjump;

        [HideInInspector] public bool hasPressedRight;

        [HideInInspector] public bool hasPressedLeft;

        [Header("Collision Checks")] public float groundCheckRadius;

        public float ceilingCheckRadius;
        public float frontCheckRadius = 0.23f;

        [SerializeField] private LayerMask whatIsGround;

        [Header("Refrences")] public Transform groundCheck;

        public Transform frontCheck;
        public Transform ceilingCheck;
        public Transform ceilingCheck1;
        public Rigidbody2D rb;

        [Header("Debugging")] [SerializeField] public bool debugStates;

        [SerializeField] private bool logVelocity;


        [HideInInspector] public bool facingRight = true;

        [HideInInspector] public bool wasGrounded;
        private CharacterAnimationController cAnimController;


        public DyingState dying;
        public FallingState falling;
        public JumpingState jumping;
        private float jumpInputTimer;
        private StateMachine movementSm;
        public RunningState running;
        public SpawningState spawning;

        public StandingState standing;
        public WalljumpingState wallJumping;
        public WallslidingState wallsliding;
        public WinningState winning;

        [field: SerializeField] public CharacterMovementValues MovementValues { get; private set; }

        public PlayerHealth Health { get; private set; }

        private void Awake() {
            rb = GetComponent<Rigidbody2D>();
            cAnimController = GetComponent<CharacterAnimationController>();
            Health = GetComponent<PlayerHealth>();
            InputManager.playerInputActions.Player.Jump.started += StartJumpInputTimer;
            AddCharacterToRuntimeSet();
            InitializeStates();
            InitializeStateMachine(spawning);
            StartCoroutine(
                Utilities.ActionAfterDelayEnumerator(spawnDelay, () => { movementSm.ChangeState(standing); }));
        }

        public void Update() {
            // Update Loop of movementSm
            movementSm.CurrentState.HandleInput();
            movementSm.CurrentState.LogicUpdate();
            UpdateVisuals();
        }

        public void FixedUpdate() {
            // PhysicsUpdate Loop of movementSM
            movementSm.CurrentState.PhysicsUpdate();
            CountDownInputTimer();
        }

        private void OnEnable() => AddCharacterToRuntimeSet();

        private void OnDisable() {
            movementSm.CurrentState.Exit();
            characterRuntimeSet.RemoveFromList(this);
        }

        // visualizing the groundCheckRadius
        private void OnDrawGizmosSelected() {
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(frontCheck.position, frontCheckRadius);
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(ceilingCheck.position, ceilingCheckRadius);
            Gizmos.DrawWireSphere(ceilingCheck1.position, ceilingCheckRadius);
        }

        public event Action<State> onStateChanged;

        // Method for flipping character
        public void Flip() {
            facingRight = !facingRight;
            transform.Rotate(0f, 180f, 0f);
        }

        private void StartJumpInputTimer(InputAction.CallbackContext context) {
            wantjump = true;
            jumpInputTimer = MovementValues.jumpInputTimer;
        }

        // Method used for moving the character left and right
        public void Move(float horizontalMove, float speed) {
            // rb.velocity = new Vector2(horizontalMove * speed * Time.deltaTime * 10f, rb.velocity.y);
            var newForce = new Vector2(horizontalMove * Time.deltaTime * speed, 0f);
            rb.AddForce(newForce, ForceMode2D.Impulse);
            /*if (rb.velocity.x > MovementValues.maxVelocityX)
                rb.velocity = new Vector2(MovementValues.maxVelocityX, rb.velocity.y);
            else if (rb.velocity.x < -MovementValues.maxVelocityX)
                rb.velocity = new Vector2(-MovementValues.maxVelocityX, rb.velocity.y);*/
            if (logVelocity) {
                Debug.Log("Velocity x: " + rb.velocity.x + " y: " + rb.velocity.y);
            }
        }


        private void ChangeToWinningState(LevelData levelData) => movementSm.ChangeState(winning);

        public void ResetMoveParams() => rb.gravityScale = MovementValues.normalGravity;

        public State GetState() => movementSm.CurrentState;

        // Method checking for a collision with ground returning a boolean
        public bool CheckCollisionOverlap(Vector3 point, float radius) =>
            Physics2D.OverlapCircle(point, radius, whatIsGround);

        private void UpdateVisuals() {
            bool touchingWall = CheckCollisionOverlap(frontCheck.position, frontCheckRadius);
            cAnimController.SetAnimationState(movementSm.CurrentState, InputManager.GetHorizontalMovementValue(),
                rb.velocity.x, MovementValues.maxVelocityX, touchingWall);
        }

        private void InitializeStates() {
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

        private void InitializeStateMachine(State state) {
            movementSm.Initialize(state);
            movementSm.OnStateChanged += InvokeStateChange;
        }

        private void InvokeStateChange(State obj) => onStateChanged?.Invoke(obj);

        private void CountDownInputTimer() {
            if (jumpInputTimer > 0f) {
                jumpInputTimer -= Time.fixedDeltaTime;
            }

            if (jumpInputTimer <= 0f) {
                wantjump = false;
            }

            if (hasPressedLeftTimer > 0f) {
                hasPressedLeftTimer -= Time.fixedDeltaTime;
            }

            if (hasPressedLeftTimer <= 0f) {
                hasPressedLeft = false;
            }

            if (hasPressedRightTimer > 0f) {
                hasPressedRightTimer -= Time.fixedDeltaTime;
            }

            if (hasPressedRightTimer <= 0f) {
                hasPressedRight = false;
            }
        }

        public void SetWasGroundedTrue() {
            if (!gameObject.activeInHierarchy) {
                return;
            }

            wasGrounded = true;
            StartCoroutine(
                Utilities.ActionAfterDelayEnumerator(MovementValues.hangTime, () => { wasGrounded = false; }));
        }

        private void AddCharacterToRuntimeSet() {
            if (characterRuntimeSet.IsEmpty()) {
                characterRuntimeSet.AddToList(this);
            }
        }
    }
}