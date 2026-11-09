using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class Bomber : MonoBehaviour
{
    [Header("Damage Settings")]
    [Range(2f, 10f)] public float damageRadius = 5f;
    public float baseDamage = 8f;
    public LayerMask playerLayer;

    [Header("Object References")]
    public SpriteRenderer bombSprite;
    public Rigidbody2D bombBody;
    public Rigidbody2D bomberBody;
    public GameObject bomb;
    public Transform dropPoint;

    [Header("Sound effects")]
    public AudioClip blastSoundEffect;

    [Header("Animation")]
    public Animator bomberAnimator;
    public Animator bombAnimator;
    public CinemachineImpulseSource impulseSource;

    [Header("Layers")]
    public LayerMask groundLayer;

    private Bomb bombScript;
    private bool isPlayerNear = false;
    public GameObject hitEffect;

    void Start()
    {
        bomberAnimator.enabled = false;
        bombScript = bomb.AddComponent<Bomb>();

        bombScript.InitBomb(4f, 8f, groundLayer, hitEffect, blastSoundEffect);
    }

    void Update()
    {
        isPlayerNear = Physics2D.OverlapCircle(
            dropPoint.transform.position, 4f, playerLayer  
        );

        if (isPlayerNear)
        {
            bomberAnimator.enabled = true;
            bomberAnimator.Play("Open Slot");
        }
    }

    void FixedUpdate()
    {
        bomberBody.linearVelocity = new Vector2(-5f, bomberBody.linearVelocityY);
    }

    void DropBomb()
    {
        if(bomb != null)
        {
            bomb.SetActive(true);
        }
        bombAnimator.Play("Drop");
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(dropPoint.transform.position, 4f);
    }
}
