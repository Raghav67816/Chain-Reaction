using UnityEngine;

public class PartsDrop : MonoBehaviour
{
    public PowerItem[] powerItems;

    public void DropItem(GameObject target)
    {
        int random = Random.Range(0, powerItems.Length);

        // spawn a game object with sprite renderer
        GameObject item = new GameObject(powerItems[random].itemName);
        item.transform.position = target.transform.position;

        SpriteRenderer renderer = item.AddComponent<SpriteRenderer>();
        renderer.sprite = powerItems[random].itemSprite;
        renderer.sortingOrder = -9;

        Rigidbody2D body = item.AddComponent<Rigidbody2D>();
        BoxCollider2D collider = item.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;

        body.AddForce(Vector2.up * 2f, ForceMode2D.Impulse);
    }
}
