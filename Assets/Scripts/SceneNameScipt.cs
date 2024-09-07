using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SceneNameScipt : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource audioSource;
    public TextMeshProUGUI fadeObject;
    public float showDuration = 4f;
    public float fadeDuration = 2f;
    
    private Color originalColor;
    private float fadeStartTime;
    
    void Start()
    {
        originalColor = fadeObject.color;
        Invoke("StartFading", showDuration);
        audioSource.Play();
    }
    
    void StartFading()
    {
        fadeStartTime = Time.time;
    }

    void Update()
    {
        if (!(fadeStartTime > 0)) return;
        var elapsedTime = Time.time - fadeStartTime;
        if (elapsedTime < fadeDuration)
        {
            var alpha = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            SetAlpha(alpha);
        }
        else
        {
            SetAlpha(0);
        }
    }

    private void SetAlpha(float alpha)
    {
        var color = originalColor;
        color.a = alpha;
        fadeObject.color = color;
    }
}
