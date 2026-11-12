using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine.InputSystem;
using System;

public class ItemEntry
{
    public int count;
    public Sprite sprite;
}

public class InventoryManager : MonoBehaviour
{

    public static event Action<Sprite> OnItemEquipped;
    public static event Action<Sprite> OnItemExhausted;

    [Header("Items")]
    public InventoryItem[] items;

    [Header("Settings")]
    public int activeItem = 0;
    public int maxSlots = 4;
    public int itemPerSlot = 5;
    public UnityEngine.Vector2 activeItemEffect;

    [Header("UI References")]
    public Image[] itemImages;
    public TextMeshProUGUI[] itemCountTexts;

    [Header("Input")]
    public InputAction switchAction;

    private Dictionary<string, ItemEntry> inventory = new Dictionary<string, ItemEntry>();

    void Awake()
    {
        Player.OnItemInteraction += OnPlayerInteraction;
        Player.OnItemUsed += OnItemUsed;
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
                    UpdateItemCountUI(item.name, inventory[item.name].count);
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
                UpdateItemCountUI(item.name, 1);
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
                UpdateItemCountUI(item.name, 1);
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

    void UpdateItemCountUI(string itemName, int count)
    {
        int itemIndex = inventory.Keys.ToList().IndexOf(itemName);
        itemCountTexts[itemIndex].text = count.ToString();
    }

    
    void OnItemUsed(Sprite item)
    {
        string[] keys = inventory.Keys.ToArray();

        for(int i=0; i<itemImages.Count(); i++)
        {
            if(itemImages[i].sprite == item)
            {
                ItemEntry entry;
                if(inventory.TryGetValue(keys[i], out entry))
                {
                    entry.count--;
                    UpdateItemCountUI(keys[i], entry.count);

                    if(entry.count < 1)
                    {
                        OnItemExhausted.Invoke(itemImages[i].sprite);
                        itemImages[i].sprite = null;

                        Color c = itemImages[i].color;
                        c.a = 0f;
                        itemImages[i].color = c;

                        Outline outline = itemImages[i].GetComponent<Outline>();
                        outline.effectDistance = Vector2.zero;
                        UpdateItemCountUI(keys[i], 0);

                        inventory.Remove(keys[i]);

                    }
                }
            }
        }
    }

    void SwitchItem(InputAction.CallbackContext ctx)
    {
        float input = ctx.ReadValue<float>();
        if(input == -1 && activeItem > 0)
        {
            activeItem--;
            SetItemActive();

            ItemEntry item;
            if(inventory.TryGetValue(inventory.Keys.ToList()[activeItem], out item))
            {
                Debug.Log(item.sprite.name);
                OnItemEquipped.Invoke(item.sprite);
            }
        }

        if(input == 1 && activeItem < inventory.Keys.Count - 1)
        {
            activeItem++;
            SetItemActive();
            
            ItemEntry item;
            if(inventory.TryGetValue(inventory.Keys.ToList()[activeItem], out item))
            {
                OnItemEquipped?.Invoke(item.sprite);
            }
        }
    }


    void SetItemActive()
    {
        for(int i = 0; i<itemImages.Length; i++)
        {
            if(itemImages[i] == null) continue;

            Outline outline = itemImages[i].GetComponent<Outline>();
            if(outline != null)
            {
                if(i == activeItem)
                {
                    outline.effectDistance = activeItemEffect;
                }

                else
                {
                    outline.effectDistance = Vector2.zero;
                }
            }
        }
    }


    void OnEnable()
    {
        switchAction.performed += SwitchItem;

        switchAction.Enable();
    }

    void OnDisable()
    {
        switchAction.performed -= SwitchItem;

        switchAction.Disable();
    }
}
