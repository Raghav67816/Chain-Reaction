using System;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

// Adding support for GIF like animations in Unity's new UI Toolkit

public class SpriteAnimator : MonoBehaviour
{
    [Header("Animaiton")]
    public int fps = 14;
    private int currentFrame = 0;
    public List<Sprite> frames;
    public bool loopAnimation = true;

    [Header("Target")]
    public UIDocument uIDocument;
    public string imageName;
    private Image targetImage;

    private Coroutine animation;


    void Start()
    {
        if(uIDocument != null && fps > 0)
        {
            VisualElement root = uIDocument.rootVisualElement;
            targetImage = root.Q<Image>(imageName);
            PlayAnimation();
        }
    }

    void PlayAnimation()
    {
        if(animation == null)
        {
            animation = StartCoroutine(SpriteAnimation());
        }

        else
        {
            StartCoroutine(animation.ToString());
        }
    }
    void StopAnimation()
    {
        if(animation != null)
        {
            StopCoroutine(animation);
        }
    }

    System.Collections.IEnumerator SpriteAnimation()
    {
        float time = 1f/fps;
        while (true)
        {
            targetImage.sprite = frames[currentFrame];
            yield return new WaitForSeconds(time);
            currentFrame++;

            if(currentFrame >= frames.Count)
            {
                if (loopAnimation)
                {
                    currentFrame = 0;
                }

                else
                {
                    StopAnimation();
                }
            }
        }
    }
}
