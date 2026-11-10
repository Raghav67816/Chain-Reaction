using System;
using UnityEngine;
using UnityEngine.Timeline;

public class Projectile : MonoBehaviour
{
    private GameObject projectile;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D projectileBody;
    private CircleCollider2D collider;
    private int targetLayer;

    public static event Action<GameObject, GameObject, float> OnProjectileHit;

    void Start()
    {
        targetLayer = LayerMask.NameToLayer("Enemy");
        if(targetLayer != 8)
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
        collider.radius = 0.2f;
    }

    public void Launch(Vector2 direction)
    {
        projectileBody.linearVelocity = direction.normalized * 10f;
        // Destroy(gameObject, 5f);
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
        if(collision.gameObject.layer == targetLayer)
        {
            Debug.Log($"Hit: {collision.name}");
            OnProjectileHit.Invoke(collision.gameObject, gameObject, 20f);
        }
    }
}
