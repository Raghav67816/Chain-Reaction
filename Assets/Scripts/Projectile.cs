using UnityEngine;
using UnityEngine.Timeline;

public class Projectile : MonoBehaviour
{
    private bool isReady = false;
    private GameObject projectile;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D projectileBody;
    private CircleCollider2D collider;

    public void Intialize(SpriteRenderer activeItemSprite)
    {
        spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = activeItemSprite.sprite;
        spriteRenderer.sortingOrder = -9;

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
        Destroy(gameObject, 5f);
    }

    public void FixedUpdate()
    {
        if(projectileBody.linearVelocity.sqrMagnitude > 0.1f)
        {
            float angle = Mathf.Atan2(
                projectileBody.linearVelocity.y,
                projectileBody.linearVelocity.x
            ) * Mathf.Rad2Deg;

            projectileBody.MoveRotation(angle);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(transform.parent);
    }
}
