using Script.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : BillboardBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public TextMeshProUGUI text;

    private float health;
    private float maxHealth;


    public void SetMaxHealth(float maxHealth)
    {
        this.maxHealth = maxHealth;

        slider.maxValue = maxHealth;
        slider.value = health;

        fill.color = gradient.Evaluate(slider.normalizedValue);
        text.text = $"{(int)health} / {(int)maxHealth}";
    }

    public void SetHealth(float health)
    {
        this.health = health;

        slider.maxValue = maxHealth;
        slider.value = health;

        fill.color = gradient.Evaluate(slider.normalizedValue);
        text.text = $"{(int)this.health} / {(int)maxHealth}";
    }
}