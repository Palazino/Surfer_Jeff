using UnityEngine;
using UnityEngine.InputSystem;

public class SurferController : MonoBehaviour
{
    [SerializeField] private float minX = -6f;
    [SerializeField] private float maxX = 6f;


    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool jumpPressed;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        HandleMovement();
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
