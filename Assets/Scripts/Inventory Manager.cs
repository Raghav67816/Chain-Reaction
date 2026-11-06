using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [Header("Items")]
    public InventoryItem[] items;

    [Header("Settings")]
    public int maxSlots = 4;
    public int itemPerSlot = 5;

    [Header("UI References")]
    public Image[] itemImages;

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

    /*
    general flow

    each unique items get a slot
    if the item exists and count < maxItemPerSlot
    accept the item and increase the counter

    if the item exists and count >= maxItemPerSlot
    reject the item

    ALSO if the item does not exist, simply add it 
    */
    bool AddItem(InventoryItem item)
    {
        if (inventory.Count >= maxSlots - 1)
        {
            int totalItems = 0;
            foreach (int itemCount in inventory.Values)
            {
                totalItems += itemCount;
            }

            if (totalItems >= maxSlots * itemPerSlot)
            {
                Debug.Log("Inventory is full");
                return false;
            }
        }

        else
        {
            int itemCount;
            if (inventory.TryGetValue(item.name, out itemCount))
            {
                if (itemCount < itemPerSlot)
                {
                    inventory[item.name]++;
                    return true;
                }

                if (itemCount >= itemPerSlot)
                {
                    return false;
                }
            }

            else
            {
                inventory.Add(item.name, 1);
                int itemIndex = inventory.Keys.ToList().IndexOf(item.name);

                UpdateItemUI(item.sprite, itemIndex);
                return true;
            }
        }

        return false;
    }

    void UpdateItemUI(Sprite itemSprite, int itemIndex)
    {
        foreach (Image image in itemImages)
        {
            if (image.sprite == itemSprite)
            {
                return;
            }

            if (image.sprite == null)
            {
                image.sprite = itemSprite;
                Color color = image.color;
                color.a = 1f;

                image.color = color;
                return;
            }
        }
    }
}
