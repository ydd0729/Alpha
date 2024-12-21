using Script.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class StatsBar : BillboardBehaviour
{
    [FormerlySerializedAs("slider")] public Slider healthBarSlider;
    public Slider resilienceBarSlider;
    [FormerlySerializedAs("gradient")] public Gradient healthBarGradient;
    [FormerlySerializedAs("fill")] public Image healthBarFill;
    [FormerlySerializedAs("text")] public TextMeshProUGUI healthBarText;

    private float health;
    private float maxHealth;
    private float resilience;
    private float maxResilience;


    public void SetMaxHealth(float maxHealth)
    {
        this.maxHealth = maxHealth;

        healthBarSlider.maxValue = maxHealth;
        healthBarSlider.value = health;

        healthBarFill.color = healthBarGradient.Evaluate(healthBarSlider.normalizedValue);
        healthBarText.text = $"{(int)health} / {(int)maxHealth}";
    }

    public void SetHealth(float health)
    {
        this.health = health;

        healthBarSlider.maxValue = maxHealth;
        healthBarSlider.value = health;

        healthBarFill.color = healthBarGradient.Evaluate(healthBarSlider.normalizedValue);
        healthBarText.text = $"{(int)this.health} / {(int)maxHealth}";
    }
    
    public void SetMaxResilience(float maxResilience)
    {
        this.maxResilience = maxResilience;

        resilienceBarSlider.maxValue = maxResilience;
        resilienceBarSlider.value = resilience;
    }

    public void SetResilience(float resilience)
    {
        this.resilience = resilience;
        
        resilienceBarSlider.maxValue = maxResilience;
        resilienceBarSlider.value = resilience;
    }
}