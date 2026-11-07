using UnityEngine;

[CreateAssetMenu(fileName = "InventoryItem", menuName = "Game/InventoryItem")]
public class InventoryItem : ScriptableObject
{
    public string itemName;
    public string category;
    public Sprite sprite;
}
