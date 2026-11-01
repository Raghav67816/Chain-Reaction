using UnityEngine;
using UnityEngine.UI;
using System.Numerics;
using System.Collections.Generic;

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

    [Header("Ground Check")]
    public Transform groundCheck;
    public LayerMask groundLayer;


    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpHeight = 2f;
    public float dashDuration = 0.3f;
    public float moveInput;
    private bool isOnGround = true;
    private bool isDashing = false;

    [Header("UI Elements")]
    public Slider healthSlider;
    public Slider gasSlider;

    void Awake()
    {
        playerControls = new PlayerControls();

        playerControls.Keyboard.Move.performed += ctx => moveInput = ctx.ReadValue<float>();
        playerControls.Keyboard.Move.canceled += ctx => moveInput = 0f;

        playerControls.Keyboard.Jump.performed += ctx => Jump();

        playerControls.Keyboard.Release.performed += ctx => ReleaseGas();
    }

    void Start()
    {
        playerBody = GetComponent<Rigidbody2D>();
        playerBody.mass = bodyMass;

        animator = GetComponent<Animator>();
        animator.SetBool("isRunning", false);

        playerAudio = GetComponent<AudioSource>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate()
    {
        if (!isDashing)
        {
            playerBody.linearVelocity = new UnityEngine.Vector2(
                moveInput * moveSpeed,
                playerBody.linearVelocity.y
            );
        }
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
        UnityEngine.Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    System.Collections.IEnumerator IncreaseSlider(
        Slider slider,
        float targetValue
    )
    {
        while (slider.value < targetValue)
        {
            slider.value = Mathf.Lerp(
                slider.value,
                targetValue,
                Time.deltaTime * 2f
            );

            yield return null;
        }

        slider.value = targetValue;
    }

    System.Collections.IEnumerator DecreaseSlider(
        Slider slider,
        float targetValue
    )
    {
        while (slider.value > targetValue)
        {
            slider.value = Mathf.Lerp(
                slider.value,
                targetValue,
                Time.deltaTime * 2f
            );

            yield return null;
        }
    }

    void Jump()
    {
        if (isOnGround)
        {
            playerBody.linearVelocity = new UnityEngine.Vector2(
                playerBody.linearVelocity.x, jumpHeight
            );


            int soundIdx = Random.Range(0, jumpSounds.Count);
            playerAudio.PlayOneShot(jumpSounds[soundIdx]);
        }
    }

    System.Collections.IEnumerator GasDash(UnityEngine.Vector2 direction, float thrust)
    {
        isDashing = true;
        playerBody.AddForce(
            direction * thrust,
            ForceMode2D.Impulse
        );

        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
        gasSlider.value = 0f;
    }

    void ReleaseGas()
    {
        if (gasSlider.value > 0f)
        {
            float thrust = gasSlider.value * 500f;
            UnityEngine.Vector2 direction = isFacingRight ? transform.right : -transform.right;

            playerBody.AddForce(
                direction * thrust,
                ForceMode2D.Impulse
            );

            StartCoroutine(DecreaseSlider(gasSlider, 0f));
            StopAllCoroutines();
            StartCoroutine(GasDash(direction, thrust));
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        FoodItem foodItem = collision.GetComponent<FoodItem>();
        if (foodItem != null)
        {
            playerBody.mass += foodItem.foodData.massValue;
            Destroy(collision.gameObject);

            StartCoroutine(IncreaseSlider(gasSlider, playerBody.mass * 2 / 100));
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
