using UnityEngine;
using System.Collections.Generic;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [Header("Items")]
    public InventoryItem[] items;

    [Header("Settings")]
    public int maxSlots = 5;
    public int itemPerSlot = 5;

    private Dictionary<string, int> inventory = new Dictionary<string, int>();

    void Awake()
    {
        Player.OnItemInteraction += OnPlayerInteraction;
    }

    void OnPlayerInteraction(InventoryItem item, GameObject gameObject)
    {
        Debug.Log("inventory manager got it");
        if (AddItem(item))
        {
            Destroy(gameObject);
        }
    }

    bool AddItem(InventoryItem item)
    {
        Debug.Log($"Checking for {item.name}");
        int itemCount;
        if (inventory.TryGetValue(item.name, out itemCount))
        {
            Debug.Log($"Got the value:{itemCount}");
            if (itemCount < itemPerSlot)
            {
                if (inventory.ContainsKey(item.name))
                {
                    inventory[item.name] += 1;
                }

                else
                {
                    inventory.Add(item.name, 1);
                }

                return true;
            }

            else
            {
                return false;
            }
        }

        else
        {
            inventory.Add(item.name, 1);
            return true;
        }
    }
}
