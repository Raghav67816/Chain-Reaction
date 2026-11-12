using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Rendering.Universal;
using Unity.Cinemachine;

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
    public Rigidbody2D droneBody;

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

    [Header("Sounds")]
    public AudioClip droneHit;
    public AudioClip droneShoot;
    public AudioClip droneSpark;

    // angles
    private float currentAngle;
    private float targetAngle;
    private GameObject effect;

    // state
    private bool canMove = false;

    void Start()
    {
        Projectile.OnProjectileHit += TakeDamage;

        currentAngle = scanLight.transform.eulerAngles.z;
        targetAngle = currentAngle;

        StartCoroutine(GenerateRandomLookUpAngles());

        intensity = scanLight.intensity;
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

        if (canMove)
        {
            droneBody.linearVelocity = new Vector2(8f, droneBody.linearVelocity.y);
        }
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
        health -= damage;

        if(health <= 0f)
        {
            StartCoroutine(DrainScanLight());
            droneAnimator.SetBool("isDead", true);
        }

        AudioSource.PlayClipAtPoint(droneHit, Camera.main.transform.position);
        Destroy(hitObject);
    }

    IEnumerator DrainScanLight()
    {
        float randomSub = Random.Range(0f, 10f);
        int n = Random.Range(2, 5);
        while (scanLight.intensity > 0f)
        {
            intensity -= randomSub;
            AudioSource.PlayClipAtPoint(droneSpark, Camera.main.transform.position);
            yield return new WaitForSeconds(randomSub/100f);

            if(scanLight.intensity <= 0f){
                scanLight.intensity = 0;
            }
        }
    }

    void SpawnBlastEffect()
    {
        effect = Instantiate(blastEffect, transform.position, Quaternion.identity);
        SpriteRenderer blastRenderer = blastEffect.GetComponent<SpriteRenderer>();
        if(blastRenderer != null)
        {
            blastRenderer.sortingOrder = 1;
        }
    }

    void DestroyBlastEffect()
    {
        Destroy(effect);
        Destroy(gameObject);
    }

    public void MoveDrone()
    {
        canMove = true;
    }

    public void StopDrone()
    {
        canMove = false;
    }

    void OnDestroy()
    {
        Projectile.OnProjectileHit -= TakeDamage;
    }
}
