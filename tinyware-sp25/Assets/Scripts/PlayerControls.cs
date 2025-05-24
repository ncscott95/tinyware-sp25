using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.Video;

public class PlayerControls : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    private InputSystem_Actions inputs;
    private float move;

    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpPower;

    [Header("Grounding")]
    [SerializeField] private bool isGrounded;
    [SerializeField] private Vector2 boxSize;
    [SerializeField] private float castDistance;
    [SerializeField] private LayerMask groundLayer;

    void Awake()
    {
        inputs = new InputSystem_Actions();

        inputs.Player.Move.performed += ctx => move = ctx.ReadValue<Vector2>().x;
        inputs.Player.Move.canceled += ctx => move = 0f;
        inputs.Player.Jump.performed += ctx => Jump();
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

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position - transform.up * castDistance, boxSize);
    }
}
