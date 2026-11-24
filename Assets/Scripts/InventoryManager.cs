using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using System;

public class ItemEntry
{
    public int count;
    public string name;
    public Sprite sprite;
    public string category;
}

public class InventoryManager : MonoBehaviour
{
    public static event Action<Sprite, string> OnItemEquipped;
    public static event Action<Sprite> OnItemExhausted;

    [Header("Settings")]
    public int activeItem = 0;
    public int maxSlots = 4;
    public int itemPerSlot = 5;
    public Vector2 activeItemEffect;

    [Header("UI References")]
    public Image[] itemImages;
    public TextMeshProUGUI[] itemCountTexts;

    [Header("Input")]
    public InputAction switchAction;

    private ItemEntry[] slots;
    private Outline[] itemOutlines;

    void Awake()
    {
        slots = new ItemEntry[maxSlots];
        itemOutlines = new Outline[itemImages.Length];

        for (int i = 0; i < itemImages.Length; i++)
        {
            if (itemImages[i] != null)
            {
                itemOutlines[i] = itemImages[i].GetComponent<Outline>();
            }
            ClearSlotUI(i);
        }

        Player.OnItemInteraction += OnPlayerInteraction;
        Player.OnItemUsed += OnItemUsed;
    }

    void OnPlayerInteraction(InventoryItem item, GameObject obj)
    {
        if (AddItem(item, obj.tag))
        {
            obj.SetActive(false);
        }
    }

    bool AddItem(InventoryItem item, string tag)
    {
        for (int i = 0; i < maxSlots; i++)
        {
            if (slots[i] != null && slots[i].name == item.name && slots[i].count < itemPerSlot)
            {
                slots[i].count++;
                UpdateSlotUI(i);
                return true;
            }
        }

        for (int i = 0; i < maxSlots; i++)
        {
            if (slots[i] == null)
            {
                slots[i] = new ItemEntry
                {
                    name = item.name,
                    count = 1,
                    sprite = item.sprite,
                    category = tag
                };

                UpdateSlotUI(i);

                if (i == activeItem)
                {
                    OnItemEquipped?.Invoke(slots[i].sprite, slots[i].category);
                    SetItemActive();
                }
                return true;
            }
        }

        return false;
    }

    void OnItemUsed(Sprite usedSprite)
    {
        int slotIndex = FindSlotBySprite(usedSprite);
        if (slotIndex == -1) return;

        slots[slotIndex].count--;

        if (slots[slotIndex].count > 0)
        {
            UpdateSlotUI(slotIndex);
        }
        else
        {
            OnItemExhausted?.Invoke(usedSprite);
            slots[slotIndex] = null;
            ClearSlotUI(slotIndex);

            if (slotIndex == activeItem)
            {
                EquipFirstItem();
            }
        }
    }

    void SwitchItem(InputAction.CallbackContext ctx)
    {
        float input = ctx.ReadValue<float>();
        if (Mathf.Abs(input) <= 0.1f) return;

        int direction = input > 0 ? 1 : -1;

        for (int step = 1; step <= maxSlots; step++)
        {
            int nextIndex = (activeItem + direction * step + maxSlots) % maxSlots;
            if (slots[nextIndex] != null)
            {
                activeItem = nextIndex;
                OnItemEquipped?.Invoke(slots[activeItem].sprite, slots[activeItem].category);
                SetItemActive();
                return;
            }
        }
    }

    void EquipFirstItem()
    {
        for (int i = 0; i < maxSlots; i++)
        {
            if (slots[i] != null)
            {
                activeItem = i;
                OnItemEquipped?.Invoke(slots[i].sprite, slots[i].category);
                SetItemActive();
                return;
            }
        }

        activeItem = 0;
        SetItemActive();
    }

    void UpdateSlotUI(int index)
    {
        ItemEntry entry = slots[index];
        if (entry == null)
        {
            ClearSlotUI(index);
            return;
        }

        itemImages[index].sprite = entry.sprite;
        Color c = itemImages[index].color;
        c.a = 1f;
        itemImages[index].color = c;

        itemCountTexts[index].text = entry.count.ToString();
    }

    void ClearSlotUI(int index)
    {
        itemImages[index].sprite = null;
        Color c = itemImages[index].color;
        c.a = 0f;
        itemImages[index].color = c;

        itemCountTexts[index].text = "";

        if (itemOutlines[index] != null)
        {
            itemOutlines[index].effectDistance = Vector2.zero;
        }
    }

    void SetItemActive()
    {
        for (int i = 0; i < itemOutlines.Length; i++)
        {
            if (itemOutlines[i] == null) continue;

            bool isSelectedAndFilled = (i == activeItem && slots[i] != null);
            itemOutlines[i].effectDistance = isSelectedAndFilled ? activeItemEffect : Vector2.zero;
        }
    }

    int FindSlotBySprite(Sprite sprite)
    {
        if (activeItem >= 0 && activeItem < maxSlots && slots[activeItem]?.sprite == sprite)
            return activeItem;

        for (int i = 0; i < maxSlots; i++)
        {
            if (slots[i]?.sprite == sprite) return i;
        }
        return -1;
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

    void OnDestroy()
    {
        Player.OnItemInteraction -= OnPlayerInteraction;
        Player.OnItemUsed -= OnItemUsed;
    }
}
