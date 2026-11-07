using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using Unity.Cinemachine;
using System;

public class Player : MonoBehaviour
{
    // player objects
    private Animator animator;
    private Rigidbody2D playerBody;
    private AudioSource playerAudio;
    private PlayerControls playerControls;
    private CinemachineImpulseSource impulseSource;

    // Events
    public static event Action<InventoryItem, GameObject> OnItemInteraction;

    [Header("Effects")]
    public ParticleSystem eatBurst;
    public ParticleSystem landEffect;
    public ParticleSystem dashEffect;

    [Header("Sounds")]
    public List<AudioClip> jumpSounds;
    public List<AudioClip> walkSounds;

    [Header("Body")]
    public float bodyMass = 80.0f; // in kg
    public float playerHealth = 100f;
    public Transform foot;
    public Transform back;
    public Transform holdPoint;

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
    private float defaultMoveSpeed = 5f;
    private bool isDashing = false;

    [Header("UI Elements")]
    public Slider healthSlider;
    public Slider gasSlider;

    private SpriteRenderer activeItemSprite;

    void Awake()
    {
        playerControls = new PlayerControls();

        
        activeItemSprite = holdPoint.GetComponent<SpriteRenderer>();

        InventoryManager.OnItemEquipped += OnItemEquipped;
    }

    void Start()
    {
        playerBody = GetComponent<Rigidbody2D>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
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
            playerBody.linearVelocity = new Vector2(
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

    void Jump(InputAction.CallbackContext ctx)
    {
        if (isOnGround)
        {
            playerBody.linearVelocity = new UnityEngine.Vector2(
                playerBody.linearVelocity.x, jumpHeight
            );
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

        moveSpeed = defaultMoveSpeed;
    }

    void PlayWalkSound()
    {
        int randomIndex = UnityEngine.Random.Range(0, walkSounds.Count);
        playerAudio.PlayOneShot(walkSounds[randomIndex]);
    }

    void PlayJumpSound()
    {
        int randomIndex = UnityEngine.Random.Range(0, jumpSounds.Count);
        playerAudio.PlayOneShot(jumpSounds[randomIndex]);
    }

    void ReleaseGas(InputAction.CallbackContext ctx)
    {
        if (gasSlider.value > 0f)
        {
            float thrust = gasSlider.value * 500f;
            Vector2 direction = isFacingRight ? transform.right : -transform.right;

            playerBody.AddForce(
                direction * thrust,
                ForceMode2D.Impulse
            );

            Instantiate(dashEffect, back.transform.position, UnityEngine.Quaternion.identity);

            impulseSource.GenerateImpulseWithForce(1f);

            StartCoroutine(DecreaseSlider(gasSlider, 0f));
            StopAllCoroutines();
            StartCoroutine(GasDash(direction, thrust));
        }
    }

    void PlayLandingEffect()
    {
        Instantiate(landEffect, foot.position, UnityEngine.Quaternion.identity);
    }

    private void SetMoveInput(float value)
    {
        moveInput = value;
    }

    private void OnMoveInput(InputAction.CallbackContext ctx)
    {
        SetMoveInput(ctx.ReadValue<float>());
    }

    private void MoveInputCancel(InputAction.CallbackContext ctx)
    {
        SetMoveInput(0f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        FoodItem foodItem = collision.GetComponent<FoodItem>();
        InventoryItemMono item = collision.GetComponent<InventoryItemMono>();

        if(item != null)
        {
            OnItemInteraction?.Invoke(item?.item, item?.gameObject);
        }

        if (foodItem != null)
        {
            playerBody.mass += foodItem.foodData.massValue;
            float retardValue = Mathf.Abs(foodItem.foodData.massValue / 1000);

            if (moveSpeed - retardValue >= 0f)
            {
                moveSpeed -= retardValue;
            }

            Instantiate(eatBurst, collision.transform.position, UnityEngine.Quaternion.identity);
            Destroy(collision.gameObject);

            StartCoroutine(IncreaseSlider(gasSlider, playerBody.mass * 2 / 100));
        }
    }

    void OnItemEquipped(Sprite itemImage)
    {
        activeItemSprite.sprite = itemImage;
    }

    void OnDisable()
    {
        playerControls.Keyboard.Move.performed -= MoveInputCancel;
        playerControls.Keyboard.Move.canceled -= MoveInputCancel;

        playerControls.Keyboard.Jump.performed -= Jump;

        playerControls.Keyboard.Release.performed -= ReleaseGas;
        playerControls.Disable();
    }

    void OnEnable()
    {
        playerControls.Keyboard.Move.performed += OnMoveInput;
        playerControls.Keyboard.Move.canceled += MoveInputCancel;

        playerControls.Keyboard.Jump.performed += Jump;

        playerControls.Keyboard.Release.performed += ReleaseGas;
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
