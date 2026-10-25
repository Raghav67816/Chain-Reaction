using UnityEngine;

public class Player : MonoBehaviour
{
    // player objects
    private Animator animator;
    private Rigidbody2D playerBody;
    private PlayerControls playerControls;

    [Header("Body")]
    public float bodyMass = 80.0f; // in kg
    private bool isFacingRight = true;

    [Header("Ground Check")]
    public Transform groundCheck;
    public LayerMask groundLayer;

    [Header("Movement")]
    private bool isOnGround = true;
    public float moveSpeed = 5f;
    public float jumpHeight = 2f;
    public float moveInput;

    void Awake()
    {
        playerControls = new PlayerControls();

        playerControls.Keyboard.Move.performed += ctx => moveInput = ctx.ReadValue<float>();
        playerControls.Keyboard.Move.canceled += ctx => moveInput = 0f;

        playerControls.Keyboard.Jump.performed += ctx => Jump();
    }

    void Start()
    {
        playerBody = GetComponent<Rigidbody2D>();
        playerBody.mass = bodyMass;

        animator = GetComponent<Animator>();
        animator.SetBool("isRunning", false);
    }

    void FixedUpdate()
    {
        playerBody.linearVelocity = new Vector2(
            moveInput * moveSpeed,
            playerBody.linearVelocity.y
        );
    }

    void Update()
    {
        isOnGround = Physics2D.OverlapCircle(
            groundCheck.position, 0.2f, groundLayer
        );

        animator.SetBool("isOnGround", isOnGround);

        if (Mathf.Abs(moveInput) > 0f)
        {
            animator.SetBool("isRunning", true);
        }

        else
        {
            animator.SetBool("isRunning", false);
        }

        if (moveInput > 0f && !isFacingRight)
        {
            Flip();
        }

        if(moveInput < 0f && isFacingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    void Jump()
    {
        if (isOnGround)
        {
            playerBody.linearVelocity = new Vector2(
                playerBody.linearVelocity.x, jumpHeight
            );
        }
    }

    void OnDisable()
    {
        playerControls.Disable();
    }

    void OnEnable()
    {
        playerControls.Enable();
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, 0.2f);
        }
    }

}
