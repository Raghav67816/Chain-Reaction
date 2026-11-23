using System;
using UnityEngine;

public class HRCCollider : MonoBehaviour
{
    private LayerMask playerLayer;
    public static event Action<int> OnPlayerInHRC;

    void Start()
    {
        playerLayer = LayerMask.NameToLayer("Player");
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.layer == playerLayer)
        {
            OnPlayerInHRC.Invoke(0);
        }
    }
}
