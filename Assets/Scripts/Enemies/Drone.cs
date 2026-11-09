using System.Collections;
using UnityEngine;
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
    [Header("Lighting")]
    public Light2D scanLight;
    [Range(0f, 360f)] public float angle;


    [Header("Movement")]
    public float normalSpeed = 5f;
    public float alertSpeed = 10f;
    public float scanRadius = 5f;

    [Header("Health")]
    public float health;
    public LineRenderer healthBar;
    public float barWidth = 1.0f;
    private float maxHealth = 100f;

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
        currentAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, 10f*50f*Time.deltaTime);
        scanLight.transform.eulerAngles = new Vector3(0f, 0f, currentAngle);

        angle = currentAngle;
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
        if(obj == transform.gameObject)
        {    
            health -= damage;
            UpdateHealthBar();

            // Destroy(hitObject);
        }
    }

    void UpdateHealthBar()
    {
        float healthPercent = health / maxHealth;
        float newRightX = healthPercent * barWidth;

        healthBar.SetPosition(0, new Vector3(0f, 0f, 0f));
        healthBar.SetPosition(1, new Vector3(newRightX, 0f, 0f));
    }
}
