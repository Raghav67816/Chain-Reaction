using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D projectileBody;
    private CircleCollider2D collider;
    private int targetLayer;
    private int groundLayer;

    public static event Action<GameObject, GameObject, float> OnProjectileHit;

    void Start()
    {
        targetLayer = LayerMask.NameToLayer("Enemy");
        groundLayer = LayerMask.NameToLayer("Ground");
        if (targetLayer != 8 && groundLayer != 6)
        {
            Debug.Log("layer not found");
        }
    }

    public void Intialize(SpriteRenderer activeItemSprite)
    {
        spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = activeItemSprite.sprite;
        spriteRenderer.sortingOrder = 0;

        projectileBody = gameObject.AddComponent<Rigidbody2D>();
        projectileBody.bodyType = RigidbodyType2D.Dynamic;
        projectileBody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        collider = gameObject.AddComponent<CircleCollider2D>();
        collider.isTrigger = true;
        collider.radius = .2f;

        if (gameObject.tag == "Power Item")
        {
            CircleCollider2D outerCollider = gameObject.AddComponent<CircleCollider2D>();
            outerCollider.radius = collider.radius * 1.2f;
            outerCollider.isTrigger = false;
        }
    }

    public void Launch(Vector2 direction)
    {
        projectileBody.linearVelocity = direction.normalized * 10f;
        Destroy(gameObject, 10f);
    }

    public void FixedUpdate()
    {
        if (projectileBody.linearVelocity.sqrMagnitude > 0.1f)
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
        if (collision.gameObject.layer == targetLayer && collider.isTrigger)
        {
            OnProjectileHit.Invoke(collision.gameObject, gameObject, 20f);
            collider.isTrigger = false;
        }
    }
}
