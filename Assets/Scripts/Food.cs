using UnityEngine;

[CreateAssetMenu(fileName = "NewFood", menuName = "Game/Food Item")]
public class Food : ScriptableObject
{
    public string foodName;
    public float massValue;
    public Sprite foodSprite;
}
