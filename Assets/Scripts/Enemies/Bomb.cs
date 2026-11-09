using System;
using Unity.Cinemachine;
using UnityEngine;

public class Bomb : MonoBehaviour
{

    private float damageR;
    private float baseD;
    private LayerMask? layer;

    private GameObject hitEffect;
    private AudioClip hitSound;
    private CinemachineImpulseSource impulseSource;

    public static event Action<int> OnBombDropped;

    void Start()
    {
        impulseSource = gameObject.GetComponent<CinemachineImpulseSource>();
    }

    public void InitBomb(
        float damageRadius,
        float baseDamage,
        LayerMask targetLayer,
        GameObject effect,
        AudioClip soundEffect
        )
    {
        damageR = damageRadius;
        baseD = baseDamage;
        layer = targetLayer;
        hitEffect = effect;
        hitSound = soundEffect;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (layer != null)
        {
            if (((1 << collision.gameObject.layer) & layer) != 0)
            {
                GameObject hit = Instantiate(hitEffect, transform.position, Quaternion.identity);

                AudioSource.PlayClipAtPoint(hitSound, Camera.main.transform.position);
                impulseSource.GenerateImpulseWithForce(4f);
                Destroy(transform.gameObject);
                Destroy(hit, .5f);
            }
        }
    }
}
