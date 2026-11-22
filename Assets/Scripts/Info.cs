using Unity.VisualScripting;
using UnityEngine;

public class Info : MonoBehaviour
{
    public GameObject targetObject;
    public SpriteRenderer interactIcon;
    public LayerMask playerLayer;
    public string infoText;
    public string title = "Info";
    [Range(0f, 10f)] public float triggerRadius = 2f;

    private bool canInteract = false;


    void Update()
    {
        canInteract = Physics2D.OverlapCircle(targetObject.transform.position, triggerRadius, playerLayer);
        if (canInteract)
        {
            interactIcon.enabled = true;
        }

        else
        {
            interactIcon.enabled = false;
        }
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(targetObject.transform.position, triggerRadius);
    }
}
