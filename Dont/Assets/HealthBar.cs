using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public TMP_Text healthText;

    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;
        fill.color = gradient.Evaluate(1f);
        UpdateHealthText(health);
    }

    public void SetHealth(float health)
    {
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
        UpdateHealthText(health);
    }

    private void UpdateHealthText(float currentHealth)
    {
        if (healthText != null)
        {
            healthText.text = $"Health: {Mathf.RoundToInt(currentHealth)}/{Mathf.RoundToInt(slider.maxValue)}";
        }
    }
}