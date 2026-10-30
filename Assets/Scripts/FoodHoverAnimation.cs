using System;
using UnityEngine;

public class FoodHoverAnimation : MonoBehaviour
{
    [Header("Hover Settings")]
    public float hoverSpeed = 2f;
    public float hoverHeight = 0.2f;

    private Vector2 startPosition;
    private float randomOffset;


    void Start()
    {
        startPosition = transform.position;
        randomOffset = UnityEngine.Random.Range(0f, 100f);
    }

    void Update()
    {
        float newY = startPosition.y + Mathf.Sin(Time.time * hoverSpeed + randomOffset) * hoverHeight;
        transform.localPosition = new Vector2(startPosition.x, newY);
    }
}
