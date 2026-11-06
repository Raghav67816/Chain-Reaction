using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using TMPro;

public class ItemEntry
{
    public int count;
    public Sprite sprite;
}

public class InventoryManager : MonoBehaviour
{
    [Header("Items")]
    public InventoryItem[] items;

    [Header("Settings")]
    public int maxSlots = 4;
    public int itemPerSlot = 5;

    [Header("UI References")]
    public Image[] itemImages;
    public TextMeshProUGUI[] itemCountTexts;

    private Dictionary<string, ItemEntry> inventory = new Dictionary<string, ItemEntry>();

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
            foreach (ItemEntry entry in inventory.Values)
            {
                totalItems += entry.count;
            }

            if (totalItems >= maxSlots * itemPerSlot)
            {
                Debug.Log("Inventory is full");
                return false;
            }
        }

        else
        {
            ItemEntry itemEntry;
            if (inventory.TryGetValue(item.name, out itemEntry))
            {
                if (itemEntry.count < itemPerSlot)
                {
                    inventory[item.name].count++;
                    UpdateItemCountUI(item, inventory[item.name].count);
                    return true;
                }

                if (itemEntry.count >= itemPerSlot)
                {
                    return false;
                }
            }

            else
            {

                ItemEntry newEntry = new ItemEntry{ count = 1, sprite = item.sprite };

                inventory.Add(item.name, newEntry);
                int itemIndex = inventory.Keys.ToList().IndexOf(item.name);
                UpdateItemCountUI(item, 1);

                UpdateItemUI(item);
                return true;
            }
        }

        return false;
    }

    void UpdateItemUI(InventoryItem item)
    {
        foreach (Image image in itemImages)
        {
            if (image.sprite == item.sprite)
            {
                UpdateItemCountUI(item, 1);
                return;
            }

            if (image.sprite == null)
            {
                image.sprite = item.sprite;
                Color color = image.color;
                color.a = 1f;

                image.color = color;
                return;
            }
        }
    }

    void UpdateItemCountUI(InventoryItem item, int count)
    {
        int itemIndex = inventory.Keys.ToList().IndexOf(item.name);
        itemCountTexts[itemIndex].text = count.ToString();
    }
}
