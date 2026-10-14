using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

// Adding support for GIF like animations in Unity's new UI Toolkit

public class SpriteAnimator : MonoBehaviour
{
    [Header("Frames")]
    public List<Sprite> frames = new List<Sprite>();
    public int fps = 14;
    public bool loopAnimation = true;
    private int currentFrame = 0;

    [Header("UI")]
    public VisualTreeAsset uIDocument;
    public String imageName;
    private Image targetImage = null;
    
    private Coroutine animationCoroutine;

    void Start()
    {
        if(frames.Count > 0 && targetImage != null && uIDocument != null)
        {
            PlayAnimation();
        }

        VisualElement root = uIDocument.CloneTree();
        if (root != null)
        {
            Debug.Log("Root is not null");
        }
        
        targetImage = root.Q<Image>(imageName);
        if(targetImage != null)
        {
            Debug.Log("Image found in the visual tree.");
        }
    }

    void PlayAnimation()
    {
        if(animationCoroutine != null) PlayAnimation();
        animationCoroutine = StartCoroutine(SpriteAnimation());
    }

    void StopAnimation()
    {
        if(animationCoroutine != null) StopCoroutine(animationCoroutine);
    }

    private System.Collections.IEnumerator SpriteAnimation()
    {
        float timePerFrame = 1f/fps;
        while (true)
        {
            targetImage.sprite = frames[currentFrame];
            yield return new WaitForSeconds(timePerFrame);

            currentFrame++;

            if(currentFrame >= frames.Count)
            {
                if(loopAnimation) currentFrame++;
                else
                {
                    StopAnimation();
                    break;
                }
            }
        }
    }
}
