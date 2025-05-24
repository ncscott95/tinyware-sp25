// Adapted from @DawnosaurDev at youtube.com/c/DawnosaurStudios

using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public Rigidbody2D RB { get; private set; }
    public InputSystem_Actions Inputs { get; private set; }

    public bool IsFacingRight { get; private set; }
    public bool IsJumping { get; private set; }
    public float LastOnGroundTime { get; private set; }

    private float _move;
    public float LastPressedJumpTime { get; private set; }
    public float AttackTimer { get; private set; }

    [Header("Gravity")]
    public float gravityScale;
    [Space(5)]
    public float fastFallGravityMult;
    public float maxFastFallSpeed;

    [Header("Run")]
    public float runMaxSpeed;
    public float runAccelAmount;
    public float runDeccelAmount;

    [Header("Jump")]
    public float jumpForce;

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

    private void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        Inputs = new InputSystem_Actions();
        Inputs.Player.Enable();

        Inputs.Player.Move.performed += ctx => _move = ctx.ReadValue<Vector2>().x;
        Inputs.Player.Move.canceled += ctx => _move = 0f;
        Inputs.Player.Jump.performed += ctx => LastPressedJumpTime = jumpInputBufferTime;
        Inputs.Player.Jump.canceled += ctx => IsJumping = false;
        Inputs.Player.Attack.performed += ctx => Attack();
    }

    private void Start()
    {
        RB.gravityScale = gravityScale;
        IsFacingRight = true;
    }

    private void Update()
    {
        LastOnGroundTime -= Time.deltaTime;
        LastPressedJumpTime -= Time.deltaTime;
        AttackTimer -= Time.deltaTime;

        if (_move != 0) CheckDirectionToFace(_move > 0);

        if (!IsJumping)
        {
            if (Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, _groundLayer) && !IsJumping)
            {
                LastOnGroundTime = coyoteTime;
            }
        }

        if (IsJumping && RB.linearVelocityY < 0) IsJumping = false;

        if (LastOnGroundTime > 0 && !IsJumping && LastPressedJumpTime > 0)
        {
            IsJumping = true;
            Jump();
        }

        if (!IsJumping) RB.gravityScale = gravityScale * fastFallGravityMult;
        else RB.gravityScale = gravityScale;

        RB.linearVelocity = new Vector2(RB.linearVelocityX, Mathf.Max(RB.linearVelocityY, -maxFastFallSpeed));
    }

    private void FixedUpdate()
    {
        Run(1);
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
        float targetSpeed = _move * runMaxSpeed;
        targetSpeed = Mathf.Lerp(RB.linearVelocityX, targetSpeed, lerpAmount);

        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? runAccelAmount : runDeccelAmount;
        float speedDif = targetSpeed - RB.linearVelocityX;

        float movement = speedDif * accelRate;
        RB.AddForce(movement * Vector2.right, ForceMode2D.Force);
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
        if (AttackTimer < attackCooldown)
        {
            AttackTimer = attackCooldown;
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
}
