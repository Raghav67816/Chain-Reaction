using UnityEngine;

public class Magnet : PowerItem
{
    public override void OnItemUsed(GameObject user)
    {
        Debug.Log($"{itemName} was used");
    }
}
