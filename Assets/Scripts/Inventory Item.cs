using UnityEngine;

[CreateAssetMenu(fileName = "Inventory Item", menuName = "Scriptable Objects/Item")]
public class IItem : ScriptableObject
{
    public string itemName;
    public int category;
    public int damage;
    public int damageRadius;
}
