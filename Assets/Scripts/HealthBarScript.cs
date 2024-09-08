using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    public Slider healthSlider;
    public Slider easeHealthSlider;
    private float currentHealth = 1f;
    private float lerpSpeed = 0.01f;
    private float lastHitTime = 0f;
    private float visibleDuration = 5f;
    private List<Image> images = new List<Image>();
    
    // Start is called before the first frame update
    void Start()
    {
        images.AddRange(GetComponentsInChildren<Image>());
        currentHealth = 1;
        healthSlider.value = currentHealth;
        easeHealthSlider.value = currentHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            lastHitTime = Time.time;
            currentHealth -= 0.1f;
        }

        if (Time.time - lastHitTime <= visibleDuration)
        {
            ToggleVisibility(true);  // Показываем
        }
        else
        {
            ToggleVisibility(false); // Скрываем
        }
        
        
        if (healthSlider.value != currentHealth)
        {
            healthSlider.value = currentHealth;
        }
        if (easeHealthSlider.value != healthSlider.value)
        {
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, healthSlider.value, lerpSpeed);
        }
    }

    public void GetDamageToBar(float damage)
    {
        lastHitTime = Time.time;
        if (damage <= 0 || currentHealth <= 0)
        {
            Debug.Log("Damage is zero or negative. Also current health may be equals to zero.");
        }
        currentHealth -= damage;
    }
    
    private void ToggleVisibility(bool visible)
    {
        // Пробегаемся по всем Image и изменяем их видимость
        foreach (Image img in images)
        {
            img.enabled = visible;
        }
    }

}
