using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;



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
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;
    [SerializeField] private int maxLives = 4;
    [SerializeField] private SpriteRenderer[] lifeIcons;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;
    [SerializeField] private FishSpawner fishSpawner;
    [SerializeField] private float knockbackForce = 5f;
    [SerializeField] private float hitStunDuration = 0.2f;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Sprite idleSprite;
    [SerializeField] private Sprite jumpSprite;
    [SerializeField] private float invertDuration = 2f;
    [SerializeField] private Sprite koSprite;


    private bool isInverted = false;
    private float invertTimer;



    private float hitStunTimer;
    private SpriteRenderer spriteRenderer;
    private bool isDead = false;
    private int currentLives;
    private float jumpHoldTimer;
    private bool isJumping;
    private bool isGrounded;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool jumpPressed;
    private bool jumpHeld;
    private float survivalTime;



    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();     
        rb = GetComponent<Rigidbody2D>();
        currentLives = maxLives;
        UpdateLivesUI();


    }

    void FixedUpdate()
    {
        if (hitStunTimer > 0)
        {
            hitStunTimer -= Time.deltaTime;
            return;
        }

        UpdateGameOverInput();
        if (isDead) return;
        HandleInvert();
        UpdateSprite();
        survivalTime += Time.deltaTime;
        scoreText.text = "Score : " + Mathf.FloorToInt(survivalTime);

        CheckGround();
        HandleMovement();
        HandleJump();
        ApplyBetterJumpPhysics();

    }

    void HandleMovement()
    {
        float moveSpeed = 8f;

        Vector2 velocity = rb.velocity;

        float move = isInverted ? -moveInput.x : moveInput.x;
        velocity.x = move * moveSpeed;

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
        else if (!isGrounded)
        {
            jumpPressed = false;
        }

    }
    void ApplyBetterJumpPhysics()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (rb.velocity.y > 0 && !jumpHeld)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            TakeDamage();

            // Détruire uniquement si c’est un poisson
            if (collision.GetComponent<FishMovement>() != null)
            {
                Destroy(collision.gameObject);
            }
        }

        if (collision.CompareTag("Bird"))
        {
            ActivateInvert();
            Destroy(collision.gameObject);
        }

    }
    void TakeDamage()
    {
        hitStunTimer = hitStunDuration;
        currentLives--;
        rb.velocity = new Vector2(0f, rb.velocity.y);
        rb.AddForce(new Vector2(-knockbackForce, 2f), ForceMode2D.Impulse);

        StartCoroutine(FlashRed());
        UpdateLivesUI();

        Debug.Log("Vie restante : " + currentLives);

        if (currentLives <= 0)
        {
            isDead = true;
            spriteRenderer.sprite = koSprite;
            fishSpawner.StopSpawning();
            Debug.Log("GAME OVER");
        }

    }
    void UpdateLivesUI()
    {
        for (int i = 0; i < lifeIcons.Length; i++)
        {
            if (i < currentLives)
                lifeIcons[i].sprite = fullHeart;
            else
                lifeIcons[i].sprite = emptyHeart;
        }
    }
    IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(0.1f);

        spriteRenderer.color = Color.white;
    }
    void UpdateGameOverInput()
    {
        if (isDead && Keyboard.current.rKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene("MainMenuScene");
        }
    }
    void UpdateSprite()
    {
        if (isDead) return;

        if (isGrounded)
            spriteRenderer.sprite = idleSprite;
        else
            spriteRenderer.sprite = jumpSprite;
    }

    void ActivateInvert()
    {
        isInverted = true;
        invertTimer = invertDuration;

        spriteRenderer.color = Color.blue;
    }
    void HandleInvert()
    {
        if (!isInverted) return;

        invertTimer -= Time.deltaTime;

        if (invertTimer <= 0f)
        {
            isInverted = false;
            spriteRenderer.color = Color.white;
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
            jumpHeld = true;
        }

        if (context.canceled)
        {
            jumpHeld = false;
        }
    }


}
