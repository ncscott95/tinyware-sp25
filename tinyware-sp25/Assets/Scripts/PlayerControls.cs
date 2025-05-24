using UnityEngine;
using UnityEngine.Timeline;

public class PlayerControls : MonoBehaviour
{
    public static PlayerControls Player;

    [SerializeField] private Rigidbody2D rb;
    private InputSystem_Actions inputs;

    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    private float move;
    private bool facingRight = true;
    [SerializeField] private float jumpPower;

    [Header("Grounding")]
    [SerializeField] private bool isGrounded;
    [SerializeField] private Vector2 boxSize;
    [SerializeField] private float castDistance;
    [SerializeField] private LayerMask groundLayer;

    [Header("Attack")]
    [SerializeField] private Transform attackPosition;
    [SerializeField] private float attackRadius;
    [SerializeField] private LayerMask attackLayer;
    private bool canAttack = true;

    void Awake()
    {
        inputs = new InputSystem_Actions();

        inputs.Player.Move.performed += ctx => move = ctx.ReadValue<Vector2>().x;
        inputs.Player.Move.canceled += ctx => move = 0f;
        inputs.Player.Jump.performed += ctx => Jump();
        inputs.Player.Attack.performed += ctx => Attack();
    }

    void OnEnable()
    {
        inputs.Player.Enable();
    }

    void OnDisable()
    {
        inputs.Player.Disable();
    }

    void Update()
    {
        rb.linearVelocityX = move * moveSpeed;
        if (move < 0 && facingRight || move > 0 && !facingRight)
        {
            facingRight = !facingRight;
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }

        isGrounded = Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, castDistance, groundLayer);
    }

    void Jump()
    {
        if (isGrounded)
        {
            isGrounded = false;
            rb.AddForce(Vector2.up * jumpPower);
        }
    }

    void Attack()
    {
        if (canAttack)
        {
            canAttack = false;
            Collider2D[] targets = Physics2D.OverlapCircleAll(attackPosition.position, attackRadius, attackLayer);
            foreach (Collider2D col in targets)
            {
                col.GetComponent<IInteractable>()?.OnInteract();
            }
            // TODO: attack cooldown
            canAttack = true; // debug, remove later
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position - transform.up * castDistance, boxSize);
        Gizmos.DrawWireSphere(attackPosition.position, attackRadius);
    }
}
