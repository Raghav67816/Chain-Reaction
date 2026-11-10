using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Rendering.Universal;

public enum DroneState
{
    IDLE,
    WALK,
    WALK_SCAN,
    DEAD
}

public class Drone : MonoBehaviour
{
    [Header("General References")]
    public Transform droneTransform;
    public Animator droneAnimator;

    [Header("Lighting")]
    public Light2D scanLight;
    [Range(0f, 360f)] public float angle;
    private float intensity;


    [Header("Movement")]
    public float normalSpeed = 5f;
    public float alertSpeed = 10f;
    public float scanRadius = 5f;

    [Header("Health")]
    public float health = 100f;
    public float maxHealth = 100f;
    public Image healthBar;

    public GameObject blastEffect;

    // angles
    private float currentAngle;
    private float targetAngle;

    void Start()
    {
        Projectile.OnProjectileHit += TakeDamage;

        currentAngle = scanLight.transform.eulerAngles.z;
        targetAngle = currentAngle;
        StartCoroutine(GenerateRandomLookUpAngles());

        intensity = scanLight.intensity;

        SpriteRenderer blastRenderer = blastEffect.GetComponent<SpriteRenderer>();
        blastRenderer.sortingOrder = 1;
    }

    void Update()
    {
        currentAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, 10f * 50f * Time.deltaTime);
        scanLight.transform.eulerAngles = new Vector3(0f, 0f, currentAngle);

        angle = currentAngle;
    }

    void FixedUpdate()
    {
        healthBar.fillAmount = Mathf.Lerp(
            healthBar.fillAmount,
            health / maxHealth,
            2f * Time.deltaTime
        );

        scanLight.intensity = Mathf.Lerp(
            scanLight.intensity,
            intensity,
            2f * Time.deltaTime
        );
    }

    IEnumerator GenerateRandomLookUpAngles()
    {
        while (true)
        {
            targetAngle = Random.Range(-360f, 360f);
            yield return new WaitForSeconds(2f);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(scanLight.transform.position, scanRadius);
    }

    void TakeDamage(GameObject obj, GameObject hitObject, float damage)
    {
        float newHealth = health - damage;
        if (newHealth <= 0f)
        {
            StartCoroutine(DrainScanLight());
            droneAnimator.SetBool("isDead", true);
        }
        health -= damage;

        Destroy(hitObject);
    }

    IEnumerator DrainScanLight()
    {
        float randomSub = Random.Range(0f, 10f);
        int n = Random.Range(2, 5);
        while (scanLight.intensity > 0f)
        {
            intensity -= randomSub;
            yield return new WaitForSeconds(randomSub/100f);

            if(scanLight.intensity <= 0f){
                scanLight.intensity = 0;
                GameObject effect = Instantiate(blastEffect, transform.position, Quaternion.identity);
                Destroy(gameObject, 1f);
                Destroy(effect, 1f);
            }
        }
    }
}
