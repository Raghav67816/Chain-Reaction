using UnityEditor.Callbacks;
using UnityEngine;

public class Player : MonoBehaviour
{
    // player objects
    private Rigidbody2D playerBody;
    private PlayerControls playerControls;

    [Header("Body")]
    public float bodyMass = 80.0f; // in kg

    [Header("Ground Check")]
    public Transform groundCheck;
    public LayerMask groundLayer;

    [Header("Movement")]
    private float gravity = -9.8f;
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
    }

    void Jump()
    {
        if (isOnGround)
        {
            playerBody.linearVelocity = new Vector2(
                playerBody.linearVelocityX, jumpHeight
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
        if(groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, 0.2f);
        }
    }

}
