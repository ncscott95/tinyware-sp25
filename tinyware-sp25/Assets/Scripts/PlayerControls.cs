// Adapted from @DawnosaurDev at youtube.com/c/DawnosaurStudios

using System;
using System.Collections;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public Rigidbody2D RB { get; private set; }
    public InputSystem_Actions Inputs { get; private set; }

    public bool IsFacingRight { get; private set; }
    public bool IsJumping { get; private set; }
    public bool CanClimb { get; set; }
    public bool IsClimbing { get; private set; }
    public bool CanDrop { get; set; }
    public float LastOnGroundTime { get; private set; }
    public float LastPressedJumpTime { get; private set; }
    public float AttackTimer { get; private set; }

    private float _moveInputX;
    private float _moveInputY;
    private Vector2 _startPosition;

    public static PlayerControls Instance;
    public delegate void LitChangeDelegate(bool value);
    public static LitChangeDelegate OnLitChange;

    [Header("Gravity")]
    public float gravityScale;
    public float maxFallSpeed;

    [Header("Movement")]
    public float runMaxSpeed;
    public float runAccelAmount;
    public float runDeccelAmount;
    public float jumpForce;
    public float climbMaxSpeed;

    [Header("Attack")]
    public float attackCooldown;

    [Header("Assists")]
    [Range(0.01f, 0.5f)] public float coyoteTime;
    [Range(0.01f, 0.5f)] public float jumpInputBufferTime;

    [Header("Checks")]
    [SerializeField] private Transform _groundCheckPoint;
    [SerializeField] private Vector2 _groundCheckSize = new Vector2(0.49f, 0.03f);
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private Transform _attackCheckPoint;
    [SerializeField] private float _attackRadius;
    [SerializeField] private LayerMask _attackLayer;

    [Header("Death")]
    public float lightDeathThreshold;

    [Header("References")]
    [SerializeField] private SpriteRenderer faceSprite;
    [SerializeField] private Animator animator;
    [SerializeField] private Material litMaterial, unlitMaterial;

    [Header("Animations")]
    private const string PLAYER_IDLE = "PlayerIdle";
    private const string PLAYER_JUMP = "PlayerJump";
    private const string PLAYER_ATTACK = "PlayerAttack";
    private const string PLAYER_CLIMB = "PlayerClimb";
    private const string PLAYER_CLIMB_IDLE = "PlayerClimbIdle";
    private const string PLAYER_WALK = "PlayerWalk";
    private string currentState;

    private bool isLit;
    public bool IsLit
    {
        get => isLit;
        set
        {
            if (value != isLit)
            {
                faceSprite.material = value ? litMaterial : unlitMaterial;

                isLit = value;
                OnLitChange?.Invoke(value);
            }
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        RB = GetComponent<Rigidbody2D>();
        Inputs = new InputSystem_Actions();
        Inputs.Player.Enable();
    }

    private void OnEnable()
    {
        Inputs.Player.Move.performed += ctx =>
        {
            _moveInputX = ctx.ReadValue<Vector2>().x;
            _moveInputY = ctx.ReadValue<Vector2>().y;
        };
        Inputs.Player.Move.canceled += ctx =>
        {
            _moveInputX = 0f;
            _moveInputY = 0f;
        };
        Inputs.Player.Jump.performed += ctx => LastPressedJumpTime = jumpInputBufferTime;
        Inputs.Player.Jump.canceled += ctx =>
        {
            IsJumping = false;
            IsClimbing = false;
        };
        Inputs.Player.Attack.performed += ctx => Attack();
    }

    private void OnDisable()
    {
        Inputs.Player.Move.performed -= ctx =>
        {
            _moveInputX = ctx.ReadValue<Vector2>().x;
            _moveInputY = ctx.ReadValue<Vector2>().y;
        };
        Inputs.Player.Move.canceled -= ctx =>
        {
            _moveInputX = 0f;
            _moveInputY = 0f;
        };
        Inputs.Player.Jump.performed -= ctx => LastPressedJumpTime = jumpInputBufferTime;
        Inputs.Player.Jump.canceled -= ctx =>
        {
            IsJumping = false;
            IsClimbing = false;
        };
        Inputs.Player.Attack.performed -= ctx => Attack();
    }

    private void Start()
    {
        RB.gravityScale = gravityScale;
        IsFacingRight = true;
        _startPosition = transform.position;
    }

    private void Update()
    {
        LastOnGroundTime -= Time.deltaTime;
        LastPressedJumpTime -= Time.deltaTime;
        AttackTimer -= Time.deltaTime;

        if (_moveInputX != 0) CheckDirectionToFace(_moveInputX > 0);

        Collider2D ground = Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, _groundLayer);
        if (!IsJumping)
        {
            if (ground != null)
            {
                LastOnGroundTime = coyoteTime;
                if (_moveInputY < 0 && ground.GetComponent<PlatformEffector2D>() != null) Drop(ground);
            }
        }

        if (IsJumping && RB.linearVelocityY < 0) IsJumping = false;

        if (CanClimb && LastPressedJumpTime > 0)
        {
            IsClimbing = true;
        }
        else if (LastOnGroundTime > 0 && !IsJumping && LastPressedJumpTime > 0 && RB.linearVelocityY < 0.01f && RB.linearVelocityY > -0.01f)
        {
            IsJumping = true;
            Jump();
        }

        if (!CanClimb || !Inputs.Player.Jump.IsPressed()) IsClimbing = false;

        if (IsClimbing) RB.gravityScale = 0f;
        else RB.gravityScale = gravityScale;

        RB.linearVelocity = new Vector2(RB.linearVelocityX, Mathf.Max(RB.linearVelocityY, -maxFallSpeed));

        // Animation Controls
        if (ground != null)
        {
            if (_moveInputX != 0)
            {
                // Walk animation
                ChangeAnimationState(PLAYER_WALK);
            }
            else
            {
                // Idle animation
                ChangeAnimationState(PLAYER_IDLE);
            }
        }

        if (IsJumping && ground == null)
        {
            ChangeAnimationState(PLAYER_JUMP);
        }
    }

    private void FixedUpdate()
    {
        if (IsClimbing) Climb();
        else Run(1);
    }

    public void CheckDirectionToFace(bool isMovingRight)
    {
        if (isMovingRight != IsFacingRight)
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;

            IsFacingRight = !IsFacingRight;
        }
    }

    private void Run(float lerpAmount)
    {
        float targetSpeed = _moveInputX * runMaxSpeed;
        targetSpeed = Mathf.Lerp(RB.linearVelocityX, targetSpeed, lerpAmount);

        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? runAccelAmount : runDeccelAmount;
        float speedDif = targetSpeed - RB.linearVelocityX;

        float movement = speedDif * accelRate;
        RB.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }

    private void Climb()
    {
        Vector2 movement = new Vector2(_moveInputX, _moveInputY).normalized;
        RB.linearVelocity = movement * climbMaxSpeed;

        if (RB.linearVelocity.magnitude > 0)
        {
            ChangeAnimationState(PLAYER_CLIMB);
        }
        else
        {
            ChangeAnimationState(PLAYER_CLIMB_IDLE);
        }
    }

    private void Drop(Collider2D platformCol)
    {
        platformCol.enabled = false;
        StartCoroutine(EnablePlatformCollider(platformCol));
    }

    IEnumerator EnablePlatformCollider(Collider2D platformCol)
    {
        yield return new WaitForSeconds(0.5f);
        platformCol.enabled = true;
    }

    private void Jump()
    {
        LastPressedJumpTime = 0;
        LastOnGroundTime = 0;

        float force = jumpForce;
        if (RB.linearVelocityY < 0) force -= RB.linearVelocityY;

        RB.AddForce(Vector2.up * force, ForceMode2D.Impulse);
    }

    void Attack()
    {
        if (AttackTimer < 0)
        {
            AttackTimer = attackCooldown;
            ChangeAnimationState(PLAYER_ATTACK);
            // Vector2 attackPos = new(transform.position.x + (transform.localScale.x * 0.5f), transform.position.y);
            Collider2D[] targets = Physics2D.OverlapCircleAll(_attackCheckPoint.position, _attackRadius, _attackLayer);
            foreach (Collider2D col in targets)
            {
                col.GetComponent<IInteractable>()?.OnInteract();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(_groundCheckPoint.position, _groundCheckSize);
        Gizmos.DrawWireSphere(_attackCheckPoint.position, _attackRadius);
    }

    public void Death()
    {
        // TODO: Death animation
        // Respawn?
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        transform.position = _startPosition;
    }

    public void ChangeAnimationState(string newState)
    {
        // Debug.Log(currentState);
        if (currentState == newState) return;

        animator.Play(newState);

        currentState = newState;
    }
}
