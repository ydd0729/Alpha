using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public bool faceCamera;
    public TextMeshProUGUI text;

    private float health;
    private float maxHealth;

    private void LateUpdate()
    {
        if (faceCamera)
        {
            var camera = Camera.main;
            if (camera)
            {
                transform.LookAt(transform.position + camera.transform.forward);
            }
        }
    }

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