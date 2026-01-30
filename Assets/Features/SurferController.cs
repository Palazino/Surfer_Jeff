using UnityEngine;
using UnityEngine.InputSystem;

public class SurferController : MonoBehaviour
{
    [SerializeField] private float minX = -6f;
    [SerializeField] private float maxX = 6f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float jumpHoldForce = 20f;
    [SerializeField] private float maxJumpHoldTime = 0.2f;

    private float jumpHoldTimer;
    private bool isJumping;
    private bool isGrounded;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool jumpPressed;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        CheckGround();
        HandleMovement();
        HandleJump();

    }

    void HandleMovement()
    {
        float moveSpeed = 8f;

        Vector2 velocity = rb.velocity;
        velocity.x = moveInput.x * moveSpeed;
        rb.velocity = velocity;

        // Clamp horizontal
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        transform.position = pos;
    }
    void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );
        Debug.Log(isGrounded);
    }
    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
    void HandleJump()
    {
        if (jumpPressed && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            jumpPressed = false;
        }
    }

    // === INPUT SYSTEM EVENTS ===

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            jumpPressed = true;
        }
    }
}
