using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public Transform interactionCheck;

    // Reference to layers
    public LayerMask computerLayer;

    void Update()
    {
        
    }

    void GetInteractable()
    {
        Collider2D hit = Physics2D.OverlapCircle(interactionCheck.position, 0.5f);
        if(hit.gameObject.layer == computerLayer)
        {
            Debug.Log("computer here.");
        }
    }
}
