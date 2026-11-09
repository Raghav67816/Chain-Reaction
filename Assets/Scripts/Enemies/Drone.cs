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
}
