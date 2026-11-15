using UnityEngine;

[CreateAssetMenu(fileName = "Magnet", menuName = "Power Items/Magnet")]
public class Magnet : PowerItem
{
    public override void OnItemUsed(GameObject user)
    {
        Debug.Log($"{itemName} was used");
    }
}
