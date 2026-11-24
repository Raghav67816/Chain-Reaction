using UnityEngine;

public class PartsDrop : MonoBehaviour
{
    public PowerItem[] powerItems;

    public void DropItem(GameObject target)
    {
        int random = Random.Range(0, powerItems.Length);

        GameObject item = new GameObject(powerItems[random].itemName)
        {
            tag = "Power Item"
        };
        item.transform.position = target.transform.position;

        SpriteRenderer renderer = item.AddComponent<SpriteRenderer>();
        renderer.gameObject.transform.localScale = new Vector3(2f, 2f, 1f);
        renderer.sprite = powerItems[random].itemSprite;

        Rigidbody2D body = item.AddComponent<Rigidbody2D>();
        BoxCollider2D collider = item.AddComponent<BoxCollider2D>();
        collider.isTrigger = false;

        BoxCollider2D triggerBox = item.AddComponent<BoxCollider2D>();
        triggerBox.size = collider.size * 1.2f;
        triggerBox.isTrigger = true;

        InventoryItemMono itemMono = item.AddComponent<InventoryItemMono>();
        InventoryItem inventoryItem = ScriptableObject.CreateInstance<InventoryItem>();

        itemMono.item = inventoryItem;

        inventoryItem.itemName = powerItems[random].itemName;
        inventoryItem.category = "";
        inventoryItem.sprite = powerItems[random].itemSprite;
    }
}
