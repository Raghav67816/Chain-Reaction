using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // player objects
    private Animator animator;
    private Rigidbody2D playerBody;
    private AudioSource playerAudio;
    private PlayerControls playerControls;

    [Header("Effects")]
    public ParticleSystem eatBurst;

    [Header("Sounds")]
    public List<AudioClip> jumpSounds;

    [Header("Body")]
    public float bodyMass = 80.0f; // in kg
    public float playerHealth = 100f;
    private bool isFacingRight = true;
    private float immediateGas = 0f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public LayerMask groundLayer;


    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpHeight = 2f;
    public float moveInput;
    private bool isOnGround = true;

    [Header("UI Elements")]
    public Slider healthSlider;
    public Slider gasSlider;

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

        playerAudio = GetComponent<AudioSource>();
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

        if (moveInput < 0f && isFacingRight)
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

    System.Collections.IEnumerator UpdateSlider(
        Slider slider,
        float targetValue
    )
    {
        while(slider.value < targetValue)
        {
            slider.value = Mathf.Lerp(
                slider.value,
                targetValue,
                Time.deltaTime * 0.5f
            );

            yield return null;
        }

        slider.value = targetValue;
    }

    void Jump()
    {
        if (isOnGround)
        {
            playerBody.linearVelocity = new Vector2(
                playerBody.linearVelocity.x, jumpHeight
            );


            int soundIdx = Random.Range(0, jumpSounds.Count);
            playerAudio.PlayOneShot(jumpSounds[soundIdx]);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        FoodItem foodItem = collision.GetComponent<FoodItem>();
        if (foodItem != null)
        {
            playerBody.mass += foodItem.foodData.massValue;
            Destroy(collision.gameObject);

            StartCoroutine(UpdateSlider(gasSlider, playerBody.mass * 2 /100));
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
