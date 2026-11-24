using UnityEngine;

[CreateAssetMenu(fileName = "Power Item", menuName = "Game/Power Item")]
public abstract class PowerItem : ScriptableObject
{
    public Sprite itemSprite;
    public string itemName;
    public abstract void OnItemUsed(GameObject user);
}
