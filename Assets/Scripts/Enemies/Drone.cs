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
    }

    IEnumerator GenerateRandomLookUpAngles()
    {
        while (true)
        {
            targetAngle = UnityEngine.Random.Range(-360f, 360f);
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
            StartCoroutine(FlickerScanLight());
            droneAnimator.SetBool("isDead", true);
            // Destroy(gameObject);
        }
        health -= damage;

        Destroy(hitObject);
    }

    IEnumerator FlickerScanLight()
    {
        int r = Random.Range(3, 7);
        float tempIntensity = scanLight.intensity;
        for (int i = 0; i < r; i++)
        {
            float waitTime = Random.Range(0.1f, 1f);
            scanLight.intensity = 0;

            yield return new WaitForSeconds(waitTime);
            scanLight.intensity = tempIntensity;
        }
    }
}
