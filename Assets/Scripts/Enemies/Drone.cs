using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Drone : MonoBehaviour
{
    [Header("Lighting")]
    public Light2D scanLight;
    [Range(0f, 360f)] public float angle;

    // angles
    private float currentAngle;
    private float targetAngle;

    void Start()
    {
        currentAngle = scanLight.transform.eulerAngles.z;
        targetAngle = currentAngle;
        StartCoroutine(GenerateRandomLookUpAngles());
    }

    void Update()
    {
        currentAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, 10f*50f*Time.deltaTime);
        scanLight.transform.eulerAngles = new Vector3(0f, 0f, angle);
    }

    IEnumerator GenerateRandomLookUpAngles()
    {
        float newAngle = Random.Range(-360f, 360f);
        angle = Mathf.Lerp(angle, newAngle, 2f*Time.deltaTime);

        yield return new WaitForSeconds(5f);
    }
}
