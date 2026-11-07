using UnityEngine;
using UnityEngine.Timeline;

public class Projectile : MonoBehaviour
{
    private bool isReady = false;
    private GameObject projectile;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D projectileBody;
    private new CircleCollider2D collider;

    public void Intialize(SpriteRenderer activeItemSprite)
    {
        spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = activeItemSprite.sprite;
        spriteRenderer.sortingOrder = 2;

        projectileBody = gameObject.AddComponent<Rigidbody2D>();
        projectileBody.bodyType = RigidbodyType2D.Dynamic;
        projectileBody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        collider = gameObject.AddComponent<CircleCollider2D>();
        collider.isTrigger = true;
        collider.radius = 0.2f;
    }

    public void Launch(Vector2 direction)
    {
        projectileBody.linearVelocity = direction.normalized * 10f;

        float angle = Mathf.Atan2(direction.y, direction.x);
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        Destroy(gameObject, 5f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(transform.parent);
    }
}
