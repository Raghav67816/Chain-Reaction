using UnityEngine;

public class Bomb : MonoBehaviour
{

    private float damageR;
    private float baseD;
    private LayerMask? layer;

    public void InitBomb(float damageRadius, float baseDamage, LayerMask targetLayer)
    {
        damageR = damageRadius;
        baseD = baseDamage;

        layer = targetLayer;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(layer != null)
        {
            if(((1 << collision.gameObject.layer) & layer) != 0)
            {
                Debug.Log($"hit {layer}");
            }
        }
    }
}
