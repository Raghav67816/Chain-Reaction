using Unity.VisualScripting;
using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [Header("Items")]
    public InventoryItem[] items;

    [Header("Settings")]
    public int maxSlots = 5;
    public int itemPerSlot = 5;

    void Awake()
    {
        Player.OnItemInteraction += OnPlayerInteraction;
    }

    void OnPlayerInteraction(InventoryItem item, GameObject gameObject)
    {
        Debug.Log("inventory manager got it");
    }
}
