using System.Linq;
using UnityEngine;

public class FoodManager : MonoBehaviour
{
    public Food[] foodData;
    private SpriteRenderer[] foodSprites;

    void Awake()
    {
        foodSprites = GetComponentsInChildren<SpriteRenderer>();
        if(foodSprites.Count() == 0)
        {
            Debug.LogError("No food items found");
        }

        foreach(SpriteRenderer foodSprite in foodSprites)
        {
            int randomFood = Random.Range(0, foodData.Length);
            Food food = foodData[randomFood];
            foodSprite.sprite = food.foodSprite;

            foodSprite.gameObject.AddComponent<FoodHoverAnimation>();

            BoxCollider2D collider = foodSprite.gameObject.AddComponent<BoxCollider2D>();
            collider.isTrigger = true;

            FoodItem item = foodSprite.gameObject.AddComponent<FoodItem>();
            item.foodData = food;
        }
    }
}
