using UnityEngine;
using UnityEngine.InputSystem;

public class Computer : MonoBehaviour
{
    public SpriteRenderer interactIcon;
    public LayerMask playerLayer;
    public string infoText;
    public string title = "Info";
    [Range(0f, 10f)] public float triggerRadius = 2f;
    
    public InputAction interactAction;

    private bool canInteract = false;


    void Update()
    {
        canInteract = Physics2D.OverlapCircle(gameObject.transform.position, triggerRadius, playerLayer);
        if (canInteract)
        {
            interactIcon.enabled = true;
        }

        else
        {
            interactIcon.enabled = false;
        }
    }

    void OnInteract(InputAction.CallbackContext ctx)
    {
        
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(gameObject.transform.position, triggerRadius);
    }

    void OnEnable()
    {
        interactAction.Enable();

        interactAction.performed += OnInteract;
    }

    void OnDisable()
    {
        interactAction.performed -= OnInteract;

        interactAction.Disable();
    }
}
